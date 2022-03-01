using LexicalAnalysis;
using System.Collections.Generic;

namespace MiniPL_Interpreter.AST
{
    class VARstmt : ASTNode
    {
        private string identifier {get; set;}
        private string type {get; set;}

        private Token token;

        public VARstmt(Token token) : base(token)
        {
            this.token = token;
        }
    }
}