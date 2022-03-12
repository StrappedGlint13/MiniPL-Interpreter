using LexicalAnalysis;
using MiniPL_Interpreter.AST;
using System.Collections.Generic;
using MiniPL_Interpreter.SemanticAnalysis;
using Mini_PL_Interpreter;

namespace MiniPL_Interpreter.SyntaxAnalysis
{
    /// <summary>
    /// A class for syntax analysis.
    /// </summary>
    public class Parser : Error
    {
        private List<Token> tokens;
        private int currentIndex;
        private Token currentToken;
        public List<ASTNode> tree {get; set;}
        private List<object> identifiers;
        private readonly List<TokenType> operands = new List<TokenType>()
        {
            {TokenType.PLUS}, {TokenType.MINUS}, {TokenType.MULTIPLY}, {TokenType.DIVIDE},
            {TokenType.EQUALS}
        };
        private bool EOF;
        public bool HasSyntaxErrors;
        public bool HasSemanticErrors;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            this.tree = new List<ASTNode>();
            this.identifiers = new List<object>();
            this.currentIndex = 0;
            this.currentToken = tokens[currentIndex];
            Parse(tokens);
        }

        /// <summary>
        /// A method for parsing the list of tokens and creating sentences in the AST.
        /// </summary>
        /// <param name="tokens"></param>
        public void Parse(List<Token> tokens)
        {
            while(!EOF)
            {
                ASTNode astNode = Statement();
                if (astNode != null) 
                {
                    tree.Add(astNode);
                    match(CurrentToken().terminal, TokenType.SEMICOLON, "';', '(...)', \'\"\' or assignment");
                }
                else 
                {
                    Recover(CurrentToken().lineNumber);
                }
            }
        }
        /// <summary>
        /// A method for recovering from the errounes statement to the next "working" statement.
        /// </summary>
        /// <param name="el"></param>
        public void Recover(int el)
        {
            while(CurrentToken().lineNumber == el)            
            {             
                if (EOF)
                {
                    break;
                }
                nextToken();
            }
        }

        /// <summary>
        /// A method that creates <stmts> according Mini-PL grammar.
        /// </summary>
        /// <returns> A node to the AST. </returns>
        public ASTNode Statement()
        {   
            ASTNode expression = null;
            Token currentStmt = CurrentToken();
            switch(CurrentToken().terminal) 
                {
                case TokenType.SEMICOLON: return null; break;
                case TokenType.VAR: 
                    currentStmt = CurrentToken();
                    nextToken();
                    IdentifierAST id = matchId();
                    match(CurrentToken().terminal, TokenType.PUNCTUATION, ':');
                    TypeAST type = Type();
                    if (CurrentToken().terminal == TokenType.ASSIGN) 
                    {
                        nextToken();
                        expression = ExprVariableDeclaration();
                        return new VarAssignmentStmt(currentStmt, id, type, expression);
                        break;
                    }
                    return new VarStmt(currentStmt, id, type); 
                    break;
                case TokenType.IDENTIFIER: 
                    currentStmt = CurrentToken();
                    nextToken();
                    match(CurrentToken().terminal, TokenType.ASSIGN, ":=");
                    expression = ExprVariableDeclaration();
                    return new IdentifierStmt(currentStmt, expression);
                    break;
                case TokenType.PRINT: 
                    currentStmt = CurrentToken();
                    nextToken();
                    expression = ExprVariableDeclaration();
                    return new PrintStmt(currentStmt, expression);
                case TokenType.READ: 
                    currentStmt = CurrentToken();
                    nextToken();
                    IdentifierAST identifier = matchId();
                    return new ReadStmt(currentStmt, identifier);
                case TokenType.ASSERT: 
                    currentStmt = CurrentToken();
                    nextToken();
                    match(CurrentToken().terminal, TokenType.LPARENTHESES, "(");
                    expression = ExprVariableDeclaration();
                    match(CurrentToken().terminal, TokenType.RPARENTHESES, ")");
                    return new AssertStmt(currentStmt, expression);
                case TokenType.FOR: 
                    currentStmt = CurrentToken();
                    nextToken();
                    IdentifierAST idFor = matchId();
                    match(CurrentToken().terminal, TokenType.IN, "in");
                    expression = ExprVariableDeclaration();
                    match(CurrentToken().terminal, TokenType.DOUBLEDOTS, "..");
                    ASTNode endingExpression = ExprVariableDeclaration(); 
                    match(CurrentToken().terminal, TokenType.DO, "DO");
                    List<ASTNode> forTree = new List<ASTNode>();
                    while (CurrentToken().terminal != TokenType.END && !HasSyntaxErrors) // this check is here for avoiding cascading errors
                    {
                        ASTNode forNode = Statement();
                        if (forNode != null) 
                        {
                            forTree.Add(forNode);
                        } else {
                            Recover(CurrentToken().lineNumber);
                        }
                    }
                    match(CurrentToken().terminal, TokenType.END, "END");
                    match(CurrentToken().terminal, TokenType.FOR, "FOR");
                    return new ForStmt(currentStmt, idFor, expression, endingExpression, forTree);
                default:
                    HasSemanticErrors = SemanticError("Semantic Error: You can't start a statement or expression with " + CurrentToken().terminal, 
                    CurrentToken().lineNumber, CurrentToken().startPos);
                    return null;
                    break;     
                }
        }

        
        /// <summary>
        /// A method for creating <expr>.
        /// </summary>
        /// <returns>  A node to the AST. </returns>
        public ASTNode ExprVariableDeclaration() 
        {
            
            ASTNode left = Operand();

            if (left == null) return null;

            if (CurrentToken().terminal == TokenType.SEMICOLON || 
            CurrentToken().terminal == TokenType.DOUBLEDOTS || CurrentToken().terminal == TokenType.DO)
            {
                return left;
            }

            Token op = null;

            while (operands.Contains(CurrentToken().terminal))
            {
                op = CurrentToken();
                nextToken();
            }

            if (op == null)
            {
                HasSyntaxErrors = SyntaxError("No operand provided", CurrentToken().lineNumber, CurrentToken().startPos);
            } 
            ASTNode right = Operand();
            return new ExprVar(left, op, right);
        }
        /// <summary>
        /// A method for creating <Oper>.
        /// </summary>
        /// <returns> A node to the AST. </returns>
        public ASTNode Operand() 
        {
            Token lhs = CurrentToken();
            switch(CurrentToken().terminal) 
                {
                case TokenType.STRING: 
                    nextToken();
                    return new OperandAST(lhs, lhs.lex);
                    break;
                case TokenType.INTEGER:
                    nextToken();
                    return new OperandAST(lhs, lhs.lex);
                    break;
                case TokenType.IDENTIFIER:
                    nextToken();  
                    return new OperandAST(lhs, lhs.lex);
                    break;
                case TokenType.LPARENTHESES:
                    nextToken();
                    ASTNode expression = ExprVariableDeclaration();
                    match(CurrentToken().terminal, TokenType.RPARENTHESES, ")");
                    return expression;
                    break;
                default:
                    HasSyntaxErrors = SyntaxError("No suitable operand provided", CurrentToken().lineNumber, CurrentToken().startPos);
                    return null;
                    break;     
                }
        }

        /// <summary>
        /// A method for generally matching tokens according the grammar.
        /// </summary>
        /// <param name="terminal"></param>
        /// <param name="type"></param>
        /// <param name="lex"></param>
        public void match(TokenType terminal, TokenType type, object lex)
        {
            if (terminal == type) 
            {
                nextToken();       
            } 
            else 
            {
                HasSyntaxErrors = ExpectedError(CurrentToken().lineNumber, CurrentToken().startPos, lex);
            }
        }
        /// <summary>
        /// A method for recognizing the identifier.
        /// </summary>
        /// <returns> A identifier node to the AST. </returns>
        public IdentifierAST matchId()
        {
            Token t = CurrentToken();
            if (t.terminal == TokenType.IDENTIFIER) 
            {   
                identifiers.Add(t.lex);
                nextToken();
                return new IdentifierAST(t);
            } 
            else 
            {
                HasSyntaxErrors = SyntaxError("Not identifier defined", CurrentToken().lineNumber, CurrentToken().startPos);
                return null;
            }
        }
        /// <summary>
        /// A method for returning current token.
        /// </summary>
        /// <returns> Current token. </returns>
        public Token CurrentToken()
        {
            return currentToken;
        }
        /// <summary>
        /// A method for moving on with the tokens.
        /// </summary>
        public void nextToken()
        {
            currentIndex++;
            if (currentIndex == tokens.Count)
            {
                EOF = true; 
            } 
            
            
            else this.currentToken = tokens[currentIndex];
        }
        /// <summary>
        /// A method for recognizing the type.
        /// </summary>
        /// <returns> A type node to the AST </returns>
        public TypeAST Type()
        {
            Token t = CurrentToken();
            switch(CurrentToken().terminal) 
                {
                case TokenType.STRING:
                    nextToken();
                    return new TypeAST(t); 
                    break;
                case TokenType.INT:
                    nextToken();
                    return new TypeAST(t);
                    break;
                case TokenType.BOOL:
                    nextToken();
                    return new TypeAST(t);
                    break;
                default:
                    HasSyntaxErrors = SyntaxError("Not valid type", CurrentToken().lineNumber, CurrentToken().startPos);
                    return null;
                    break;
                }
        }

    }
}