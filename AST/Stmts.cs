using LexicalAnalysis;
using System.Collections.Generic;

namespace MiniPL_Interpreter.AST
{
    public class VarStmt : ASTNode
    {
        public IdentifierAST identifier {get; set;}
        public TypeAST type {get; set;}

        public VarStmt(Token statement, IdentifierAST identifier, TypeAST type) : base(statement)
        { 
            this.type = type;
            this.identifier = identifier;
        }
    }
    public class VarAssignmentStmt : ASTNode
    {
        public IdentifierAST identifier {get;}
        public TypeAST type {get;}
        public ASTNode expression;

        public VarAssignmentStmt(Token statement, IdentifierAST identifier, TypeAST type, ASTNode expression) : base(statement)
        {
            
            this.type = type;
            this.identifier = identifier;
            this.expression = expression;
        }
    }

    public class VariableStmt : ASTNode
    {
        public ASTNode expression;

        public VariableStmt(Token statement, ASTNode expression) : base(statement)
        {
            this.expression = expression;
        }
    }

    public class PrintStmt : ASTNode
    {
        public ASTNode right;

        public PrintStmt(Token statement, ASTNode expression) : base(statement)
        {
            this.right = expression;
        }
    }

    public class ReadStmt : ASTNode
    {
        private IdentifierAST identifier;

        public ReadStmt(Token statement, IdentifierAST identifier) : base(statement)
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
        private ASTNode identifier;
        private List<ASTNode> forTree;


        public ForStmt(Token statement, ASTNode identifier, ASTNode startingCondition, ASTNode endingCondition
        , List<ASTNode> forTree) : base(statement)
        {
            this.identifier = identifier;
            this.startingCondition = startingCondition;
            this.endingCondition = endingCondition;
            this.forTree = forTree;
        }
    }
}