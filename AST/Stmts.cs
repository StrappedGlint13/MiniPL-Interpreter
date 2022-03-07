using LexicalAnalysis;
using System.Collections.Generic;

namespace MiniPL_Interpreter.AST
{
    public class VarStmt : ASTNode
    {
        private Token identifier {get; set;}
        private Token type {get; set;}

        public VarStmt(Token identifier, Token type, Token var) : base(var)
        {
            
            this.type = type;
            this.identifier = identifier;
        }
    }
    public class VarDeclarationStmt : ASTNode
    {
        private Token identifier {get; set;}
        private Token type {get; set;}

        private ASTNode expression;

        public VarDeclarationStmt(Token identifier, Token type, Token token, ASTNode expression) : base(token)
        {
            
            this.type = type;
            this.identifier = identifier;
            this.expression = expression;
        }
    }
}