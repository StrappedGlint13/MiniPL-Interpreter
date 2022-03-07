using LexicalAnalysis;
using System.Collections.Generic;

namespace MiniPL_Interpreter.AST
{
    public class ASTNode
    {
        public Token token;

        public ASTNode (Token token) 
        {
            this.token = token;
        }
    }

    public class OpAST : ASTNode
    {
        public OpAST (Token token) : base(token){}
    }

    public class OperandAST : ASTNode
    {
        private object value;

        public OperandAST (Token token, object lexeme) : base(token)
        {
            this.value = lexeme;
        }
    }
    
    
}