using System;
using System.IO;
using LexicalAnalysis;
using MiniPL_Interpreter.SyntaxAnalysis;
using MiniPL_Interpreter.AST;
using MiniPL_Interpreter.SemanticAnalysis;

namespace Mini_PL_Interpreter
{
    
}

public class Interpreter {
    
    public static void Main(string[] args) {
        //Console.Write("Enter the filename: ");
        string file = "1_example";
        //string file = Console.In.ReadLine();

        string input = "";
        if (File.Exists(file)) {    
            input = File.ReadAllText(file);
            char[] line = input.ToCharArray();
            foreach (char ch in line)
            {
                Console.Write(ch);
            }
        } else {
            Console.WriteLine("File does not exists");
        }
        Console.WriteLine("\n");
        Scanner scanner = new Scanner(input);
        List<Token> tokens = scanner.Tokens;
        Parser parser = new Parser(tokens);

        List<ASTNode> ASTNodes = parser.tree;
        SemanticAnalyzer semanticAnalyzer = new SemanticAnalyzer(ASTNodes);
        semanticAnalyzer.AnalyzeSemantics(ASTNodes);

        foreach (Token t in tokens)
        {
            Console.WriteLine(t);
        }

        if (scanner.hasLexicalErrors || parser.HasSyntaxErrors || parser.HasSemanticErrors || semanticAnalyzer.HasSemanticErrors) 
        {
            Console.WriteLine("Build failed.");
        } else {
                Console.WriteLine("Abstract Syntax Tree:");
                Console.WriteLine("stmts");
                foreach (ASTNode n in ASTNodes)
                {
                    CreateAST(n, "", false);
                }
        } 
    }
    public static void CreateAST(ASTNode node, string indent, bool last)
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

    public static void CreateVarAssignment(ASTNode node, string indent, bool last)
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

    public static void CreatePrint(ASTNode node, string indent, bool last)
    {
        string rightIndent = indent;
        Console.WriteLine("|  \\");
        NextLeft(indent);
        Console.WriteLine(((PrintStmt)node).token.lex);
        OperandAST operand = ((OperandAST)((PrintStmt)node).right);
        rightIndent = nextRight(rightIndent);
        Console.WriteLine(operand.token.lex);
    }

    public static void NextLeft(string indent)
    {
        Console.Write("|");
        Console.Write(indent + "|-");
    }

    public static string nextRight(string indent)
    {
        Console.Write("|");
        Console.Write(indent);
        Console.WriteLine(" \\");
        Console.Write("|");
        Console.Write(indent + "  |-");
        return indent + "  ";
    }
}