using System;
using System.IO;
using LexicalAnalysis;
using MiniPL_Interpreter.SyntaxAnalysis;
using MiniPL_Interpreter.AST;
using MiniPL_Interpreter.SemanticAnalysis;

namespace Mini_PL_Interpreter
{
    
}

/// <summary>
/// A main class of the interpreter.
/// </summary>
public class Interpreter {
    
    public static void Main(string[] args) {
        //Console.Write("Enter the filename: ");
        string file = "Test_data/1_example";
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

        foreach (Token t in tokens)
        {
            Console.WriteLine(t);
        }

        Parser parser = new Parser(tokens);

        List<ASTNode> ASTNodes = parser.tree;
        SemanticAnalyzer semanticAnalyzer = new SemanticAnalyzer(ASTNodes);
        semanticAnalyzer.AnalyzeSemantics(ASTNodes);

        if (scanner.hasLexicalErrors || parser.HasSyntaxErrors || parser.HasSemanticErrors || semanticAnalyzer.HasSemanticErrors) 
        {
            Console.WriteLine("Build failed.");
        } else {
            ASTbuilder astBuilder = new ASTbuilder();
            Console.WriteLine("Abstract Syntax Tree:");
            Console.WriteLine("stmts");
            foreach (ASTNode n in ASTNodes)
            {
                astBuilder.CreateAST(n, "", false);
            }
            Console.Write("\\-$$\n");
        } 
    }
}