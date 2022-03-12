using LexicalAnalysis;

namespace MiniPL_Interpreter.AST
{
    /// <summary>
    /// A class for <Expr>
    /// </summary>
    public class ExprVar : ASTNode
    {
        public ASTNode left;
        public ASTNode right;

        public ExprVar(ASTNode left, Token op, ASTNode right) : base(op)
        {
            this.left = left;
            this.right = right;
        }
    }
}