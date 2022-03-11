using LexicalAnalysis;

namespace MiniPL_Interpreter.AST
{
    public class ASTbuilder
    {
        public void CreateAST(ASTNode node, string indent, bool last)
    {
        if (last)
        {
            Console.Write("\\-");
            indent += "  ";
        }
        else
        {
            Console.WriteLine("|-" + node.token.lex + "_stmt");
        }
        if (node.token.terminal == TokenType.VAR)
        {
            CreateVarAssignment(node, "  ", last);
        }

        if (node.token.terminal == TokenType.PRINT)
        {
            CreatePrint(node, "  ", last);
        }
    }

    public void CreateVarAssignment(ASTNode node, string indent, bool last)
    {
        string rightIndent = indent;
        Console.WriteLine("|  \\");
        NextLeft(indent);
        try {
            Console.WriteLine(((VarStmt)node).identifier.token.lex);
            NextLeft(indent);
            Console.WriteLine(((VarStmt)node).type.token.lex);
            return;
        } catch {
            try {
            ExprVar expression = ((ExprVar)((VarAssignmentStmt)node).expression);
            OperandAST operand = null;
            while (true)
            {
                rightIndent = nextRight(indent);
                indent = rightIndent;
                Console.WriteLine(expression.token.lex);
                NextLeft(rightIndent);
                Console.WriteLine(expression.left.token.lex);

                if (expression.left.GetType() == typeof(ExprVar))
                {
                    rightIndent = nextRight(rightIndent);
                    Console.WriteLine(expression.right.token.lex);
                    expression = ((ExprVar)expression.left);
                    
                }
                else if (expression.right.GetType() == typeof(ExprVar))
                {
                    expression = ((ExprVar)expression.right);
                }
                else 
                {
                    rightIndent = nextRight(rightIndent);
                    Console.WriteLine(expression.right.token.lex);
                    break;
                }
            }
            } catch {
                Console.WriteLine(((VarAssignmentStmt)node).identifier.token.lex);
                NextLeft(indent);
                Console.WriteLine(((VarAssignmentStmt)node).type.token.lex);
                OperandAST operand = ((OperandAST)((VarAssignmentStmt)node).expression);
                rightIndent = nextRight(indent);
                indent = rightIndent;
                Console.WriteLine(operand.token.lex);   
            }
            
        }
        
        
    }

    public void CreatePrint(ASTNode node, string indent, bool last)
    {
        string rightIndent = indent;
        Console.WriteLine("|  \\");
        NextLeft(indent);
        Console.WriteLine(((PrintStmt)node).token.lex);
        OperandAST operand = ((OperandAST)((PrintStmt)node).right);
        rightIndent = nextRight(rightIndent);
        Console.WriteLine(operand.token.lex);
    }

    public void NextLeft(string indent)
    {
        Console.Write("|");
        Console.Write(indent + "|-");
    }

    public string nextRight(string indent)
    {
        Console.Write("|");
        Console.Write(indent);
        Console.WriteLine(" \\");
        Console.Write("|");
        Console.Write(indent + "  |-");
        return indent + "  ";
    }
    }
    
}