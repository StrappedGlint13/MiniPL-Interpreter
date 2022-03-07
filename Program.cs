using System;
using System.IO;
using LexicalAnalysis;
using MiniPL_Interpreter.SyntaxAnalysis;
using MiniPL_Interpreter.AST;

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
        Scanner lex = new Scanner(input);
        Parser parser = new Parser(lex.Tokens);

        List<Token> tokens = lex.Tokens;
        
        foreach (Token t in tokens)
        {
            Console.WriteLine(t);
        }

        List<ASTNode> ASTNodes = parser.tree;

        foreach (ASTNode n in ASTNodes)
        {
            Console.WriteLine(n);
        }
    }
}
