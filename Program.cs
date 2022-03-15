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
        while(true)
            {
            Console.Write("Enter the filename from the Test_data folder f.ex: 1_example\nQuit typing \"exit\"\nEnter the program name: ");
            string file = Console.In.ReadLine();
            if (file.Equals("exit")) break;
            
            string input = "";
            if (File.Exists("Test_data/" + file)) {    
                input = File.ReadAllText("Test_data/" + file);
                char[] line = input.ToCharArray();
            } else {
                Console.WriteLine("File does not exists");
                continue;
            }
            Console.WriteLine("\n");
            Console.WriteLine("Scanner:");
            Scanner scanner = new Scanner(input);
            List<Token> tokens = scanner.Tokens;

            foreach (Token t in tokens)
            {
                Console.WriteLine(t);
            }

            Parser parser = new Parser(tokens);

            List<ASTNode> ASTNodes = parser.tree;
            

            if (scanner.hasLexicalErrors || parser.HasSyntaxErrors || parser.HasSemanticErrors) 
            {
                Console.WriteLine("\nBuild failed due to given program syntax.");
                continue;
            } 

            SemanticAnalyzer semanticAnalyzer = new SemanticAnalyzer(ASTNodes);
            semanticAnalyzer.AnalyzeSemantics(ASTNodes);

            if (semanticAnalyzer.HasSemanticErrors)
            {
                Console.WriteLine("\nBuild failed due to given program semantics.");
                continue;
            }
            
            ASTbuilder astBuilder = new ASTbuilder();
            Console.WriteLine("\nAbstract Syntax Tree:");
            Console.WriteLine("stmts");
            foreach (ASTNode n in ASTNodes)
            {
                astBuilder.CreateAST(n, "", false);
            }
            Console.Write("\\-$$\n"); 
        } 
    }
}