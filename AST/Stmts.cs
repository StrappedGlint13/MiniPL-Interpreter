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
    public class VarAssignmentStmt : ASTNode
    {
        private Token identifier {get; set;}
        private Token type {get; set;}

        private ASTNode expression;

        public VarAssignmentStmt(Token identifier, Token type, Token token, ASTNode expression) : base(token)
        {
            
            this.type = type;
            this.identifier = identifier;
            this.expression = expression;
        }
    }

    public class VariableStmt : ASTNode
    {
        private ASTNode expression;

        public VariableStmt(Token statement, ASTNode expression) : base(statement)
        {
            this.expression = expression;
        }
    }

    public class PrintStmt : ASTNode
    {
        private ASTNode expression;

        public PrintStmt(Token statement, ASTNode expression) : base(statement)
        {
            this.expression = expression;
        }
    }

    public class ReadStmt : ASTNode
    {
        private Token identifier;

        public ReadStmt(Token statement, Token identifier) : base(statement)
        {
            this.identifier = identifier;
        }
    }

    public class AssertStmt : ASTNode
    {
        private ASTNode expression;

        public AssertStmt(Token statement, ASTNode expression) : base(statement)
        {
            this.expression = expression;
        }
    }

    public class ForStmt : ASTNode
    {
        private ASTNode startingCondition;
        private ASTNode endingCondition;
        private Token identifier;
        private List<ASTNode> forTree;


        public ForStmt(Token statement, Token identifier, ASTNode startingCondition, ASTNode endingCondition
        , List<ASTNode> forTree) : base(statement)
        {
            this.identifier = identifier;
            this.startingCondition = startingCondition;
            this.endingCondition = endingCondition;
            this.forTree = forTree;
        }
    }
}