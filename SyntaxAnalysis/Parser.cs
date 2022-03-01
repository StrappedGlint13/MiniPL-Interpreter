using LexicalAnalysis;
using MiniPL_Interpreter.AST;
using System.Collections.Generic;

namespace MiniPL_Interpreter.SyntaxAnalysis
{
    public class Parser
    {
        private List<Token> tokens;

        private int currentToken;
        private List<ASTNode> ASTtree {get; set;}
  

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            this.ASTtree = new List<ASTNode>();
            this.currentToken = 0;
            //createAST();
            Analyze(tokens);
        }

        public void Analyze(List<Token> tokens)
        {
            for(int i = currentToken; i < tokens.Count; i++)
            {
                Statement(tokens[i]);
              
            }
        }
        public void Statement(Token token)
        {   
            switch(token.TokenType) 
                {
                case TokenType.VAR: ExprVariableDeclaration();
                    break;
                case TokenType.PRINT: ExprPrint();
                    break;
                default:
                    break;     
                }
        }

        public void nextToken()
        {
            currentToken++;
        }

        public void ExprVariableDeclaration() 
        {

        }

        public void ExprPrint() 
        {

        }



        public void Factor ()
        {
            switch(this.tokens[currentToken].terminal) 
                {
                case TokenType.STRING: 
                    break;
                case TokenType.INTEGER:
                    break;
                default:
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