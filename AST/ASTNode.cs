using System;
using LexicalAnalysis;
using System.Collections.Generic;

namespace MiniPL_Interpreter.AST
{
    abstract class ASTNode
    {
        private Token token {get; set;}

        public ASTNode(Token t)
        {
            this.token = t;
        }
    }
}