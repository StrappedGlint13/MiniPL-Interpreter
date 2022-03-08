using LexicalAnalysis;

namespace MiniPL_Interpreter.AST
{
    public class IdentifierAST : ASTNode
    {
        public IdentifierAST(Token token) : base(token)
        {}
    }

    public class TypeAST : ASTNode
    {
        public TypeAST(Token token) : base(token)
        {}
    }
}