using LexicalAnalysis;
using MiniPL_Interpreter.AST;
using System.Collections.Generic;
using MiniPL_Interpreter.SemanticAnalysis;

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
        public bool HaveSyntaxErrors;
        public bool HaveSemanticErrors;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            this.tree = new List<ASTNode>();
            this.identifiers = new List<object>();
            this.currentIndex = 0;
            this.currentToken = tokens[currentIndex];
            //createAST();
            Parse(tokens);
        }

        public void AnalyzeSemantics(List<ASTNode> ASTNodes)
        {
            foreach (ASTNode node in ASTNodes)
            {
                Console.WriteLine(node);
            }
        }

        public void Parse(List<Token> tokens)
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
                    Console.WriteLine("Semantic Error: You can't start a line with " + CurrentToken().terminal + ". At (" + CurrentToken().lineNumber  + ", " + CurrentToken().startPos + ")");
                    HaveSemanticErrors = true;
                    return null;
                    break;     
                }
        }

        public ASTNode ExprVariableDeclaration() 
        {
            
            ASTNode left = Operand();

            if (left == null) return null;

            if (CurrentToken().terminal == TokenType.SEMICOLON || 
            CurrentToken().terminal == TokenType.DOUBLEDOTS)
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
                    Console.WriteLine("Syntax Error: " + CurrentToken().terminal + " at (" + CurrentToken().lineNumber  + ", " + CurrentToken().startPos + ")");
                    HaveSyntaxErrors = true;
                    return null;
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
                HaveSyntaxErrors = true;
                return null;
            }
        }

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
                Console.WriteLine("Error: Not identifier defined at (" + CurrentToken().lineNumber  + ", " + CurrentToken().startPos + ")");
                HaveSyntaxErrors = true;
                return null;
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

        public void Recover(int el)
        {
            while(CurrentToken().lineNumber == el)
            {
                nextToken();
            }
        }

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
                    Console.WriteLine("Error: Not valid type at (" + CurrentToken().lineNumber  + ", " + CurrentToken().startPos + ")");  
                    HaveSyntaxErrors = true;
                    return null;
                    break;
                }
        }

    }
}