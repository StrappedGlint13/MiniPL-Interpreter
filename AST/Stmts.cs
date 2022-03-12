using LexicalAnalysis;
using System.Collections.Generic;

namespace MiniPL_Interpreter.AST
{
    /// <summary>
    /// A class for <stmt> "var" without ":=".
    /// </summary>
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
    /// <summary>
    /// A class for <stmt> "var" with the tail ":=" <expr
    /// </summary>
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
    /// <summary>
    /// A class for identifier declaration statement.
    /// </summary>
    public class IdentifierStmt : ASTNode
    {
        public ASTNode expression;

        public IdentifierStmt(Token statement, ASTNode expression) : base(statement)
        {
            this.expression = expression;
        }
    }
    /// <summary>
    /// A class for print statement.
    /// </summary>
    public class PrintStmt : ASTNode
    {
        public ASTNode right;

        public PrintStmt(Token statement, ASTNode expression) : base(statement)
        {
            this.right = expression;
        }
    }

    /// <summary>
    /// A class for read statement.
    /// </summary>
    public class ReadStmt : ASTNode
    {
        private IdentifierAST identifier;

        public ReadStmt(Token statement, IdentifierAST identifier) : base(statement)
        {
            this.identifier = identifier;
        }
    }
    /// <summary>
    /// A class for assert statement.
    /// </summary>
    public class AssertStmt : ASTNode
    {
        private ASTNode expression;

        public AssertStmt(Token statement, ASTNode expression) : base(statement)
        {
            this.expression = expression;
        }
    }
    /// <summary>
    /// A class for "for" statement."
    /// </summary>
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