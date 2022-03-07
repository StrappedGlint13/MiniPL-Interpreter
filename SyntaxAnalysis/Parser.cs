using LexicalAnalysis;
using MiniPL_Interpreter.AST;
using System.Collections.Generic;

namespace MiniPL_Interpreter.SyntaxAnalysis
{
    public class Parser
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

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            this.tree = new List<ASTNode>();
            this.identifiers = new List<object>();
            this.currentIndex = 0;
            this.currentToken = tokens[currentIndex];
            //createAST();
            Analyze(tokens);
        }

        public void Analyze(List<Token> tokens)
        {
            while(!EOF)
            {
                ASTNode astNode = Statement();
                if (astNode != null) tree.Add(astNode); 
                match(CurrentToken().terminal, TokenType.SEMICOLON, "';', '(...)' or assignment");
            }
        }
        public ASTNode Statement()
        {   
            ASTNode expression = null;
            Token currentStmt = CurrentToken();
            switch(CurrentToken().terminal) 
                {
                case TokenType.VAR: 
                    currentStmt = CurrentToken();
                    nextToken();
                    Token id = matchId();
                    match(CurrentToken().terminal, TokenType.PUNCTUATION, ':');
                    Token type = Type();
                    if (CurrentToken().terminal == TokenType.ASSIGN) 
                    {
                        nextToken();
                        expression = ExprVariableDeclaration();
                        return new VarAssignmentStmt(id, type, currentStmt, expression);
                        break;
                    }
                    return new VarStmt(id, type, currentStmt); 
                    break;
                case TokenType.IDENTIFIER: 
                    currentStmt = CurrentToken();
                    /*
                    if (!identifiers.Any(ident => ident.Equals(identifier.lex)))
                    {
                        Console.WriteLine("Syntax Error: " + CurrentToken().terminal + " at (" + CurrentToken().lineNumber  + ", " + CurrentToken().startPos + ")");
                    }*/
                    nextToken();
                    match(CurrentToken().terminal, TokenType.ASSIGN, ":=");
                    expression = ExprVariableDeclaration();
                    return new VariableStmt(currentStmt, expression);
                    break;
                case TokenType.PRINT: 
                    currentStmt = CurrentToken();
                    nextToken();
                    expression = ExprVariableDeclaration();
                    return new PrintStmt(currentStmt, expression);
                case TokenType.READ: 
                    currentStmt = CurrentToken();
                    nextToken();
                    Token identifier = matchId();
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
                    Token idFor = matchId();
                    match(CurrentToken().terminal, TokenType.IN, "in");
                    expression = ExprVariableDeclaration();
                    match(CurrentToken().terminal, TokenType.DOUBLEDOTS, "..");
                    ASTNode endingExpression = ExprVariableDeclaration(); 
                    match(CurrentToken().terminal, TokenType.DO, "DO");
                    List<ASTNode> forTree = new List<ASTNode>();
                    while (CurrentToken().terminal != TokenType.END)
                    {
                       ASTNode forNode = Statement();
                       if (forNode != null) forTree.Add(forNode); 
                       match(CurrentToken().terminal, TokenType.SEMICOLON, "';', '(...)' or assignment");
                    }
                    match(CurrentToken().terminal, TokenType.END, "END");
                    match(CurrentToken().terminal, TokenType.FOR, "FOR");
                    return new ForStmt(currentStmt, idFor, expression, endingExpression, forTree);
                default:
                    return new AssertStmt(currentStmt, expression);;
                    break;     
                }
        }

        public ASTNode ExprVariableDeclaration() 
        {
            
            ASTNode left = Operand();

            if (CurrentToken().terminal == TokenType.SEMICOLON)
            {
                return left;
            }

            Token op = null;

            while (operands.Contains(CurrentToken().terminal))
            {
                op = CurrentToken();
                nextToken();
            } 
            ASTNode right = Operand();
            return new ExprVar(left, op, right);
        }

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
                    return null;
                    Console.WriteLine("Syntax Error: " + CurrentToken().terminal + " at (" + CurrentToken().lineNumber  + ", " + CurrentToken().startPos + ")");
                    break;     
                }
        }


        public Token match(TokenType terminal, TokenType type, object lex)
        {
            if (terminal == type) 
            {
                Token t = CurrentToken();
                nextToken();
                return t;            
            } 
            else 
            {
                Console.WriteLine("Syntax error at (" + CurrentToken().lineNumber  + ", " + CurrentToken().startPos + "). Excpected: \"" + lex + "\"");
                return null;
            }
        }

        public Token matchId()
        {
            if (CurrentToken().terminal == TokenType.IDENTIFIER) 
            {   
                Token t = CurrentToken();
                identifiers.Add(t.lex);
                nextToken();
                return t;
            } 
            else 
            {
                return null;
                Console.WriteLine("Error: Not identifier defined at (" + CurrentToken().lineNumber  + ", " + CurrentToken().startPos + ")");
            }
        }

        public Token CurrentToken()
        {
            return currentToken;
        }

        public void nextToken()
        {
            currentIndex++;
            if (currentIndex == tokens.Count)
            {
                EOF = true; 
            } 
            
            else this.currentToken = tokens[currentIndex];
        }


        public void ExprPrint() 
        {

        }



        public Token Type()
        {
            Token t = CurrentToken();
            switch(CurrentToken().terminal) 
                {
                case TokenType.STRING:
                    nextToken();
                    return t;  
                    break;
                case TokenType.INT:
                    nextToken();
                    return t;  
                    break;
                case TokenType.BOOL:
                    nextToken();
                    return t;  
                    break;
                default:
                    return null;
                    Console.WriteLine("Error: Not valid type at (" + CurrentToken().lineNumber  + ", " + CurrentToken().startPos + ")");  
                    break;
                }
        }

    }
        /*
        public void createAST()
        {
            
            while (currentToken < tokens.Length)
            {
            // ASTTree.Add(Statement(tokens.get(currentToken)));
            }*/
            
       
    /*
       

        
    }
    }
    */
}