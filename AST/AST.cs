using LexicalAnalysis;
using System.Collections.Generic;

namespace MiniPL_Interpreter.AST
{
    /// <summary>
    /// Abstract class for general AST node.
    /// </summary>
    abstract public class ASTNode
    {
        public Token token;

        public ASTNode (Token token) 
        {
            this.token = token;
        }
    }

    /// <summary>
    /// AST node for <Oper>
    /// </summary>
    public class OperandAST : ASTNode
    {
        public object value;

        public OperandAST (Token token, object value) : base(token)
        {
            this.value = value;
        }
    }
    
    
}