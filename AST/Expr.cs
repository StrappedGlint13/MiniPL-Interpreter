using LexicalAnalysis;

namespace MiniPL_Interpreter.AST
{
    public class ExprVar : ASTNode
    {
        ASTNode left;
        ASTNode right;
        public ExprVar(ASTNode left, Token op, ASTNode right) : base(op)
        {
            this.left = left;
            this.right = right;
        }
    }
}