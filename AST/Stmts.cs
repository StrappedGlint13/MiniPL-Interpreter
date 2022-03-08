using LexicalAnalysis;
using System.Collections.Generic;

namespace MiniPL_Interpreter.AST
{
    public class VarStmt : ASTNode
    {
        private IdentifierAST Identifier {get; set;}
        private TypeAST Type {get; set;}

        public VarStmt(Token statement, IdentifierAST Identifier, TypeAST Type) : base(statement)
        { 
            this.Type = Type;
            this.Identifier = Identifier;
        }
    }
    public class VarAssignmentStmt : ASTNode
    {
        public IdentifierAST Identifier {get; set;}
        public TypeAST Type {get; set;}
        public ASTNode expression;

        public VarAssignmentStmt(Token statement, IdentifierAST Identifier, TypeAST Type, ASTNode expression) : base(statement)
        {
            
            this.Type = Type;
            this.Identifier = Identifier;
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
        private ASTNode right;

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