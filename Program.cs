using System;
using System.IO;
using LexicalAnalysis;
using MiniPL_Interpreter.SyntaxAnalysis;

namespace Mini_PL_Interpreter
{
    
}

public class Interpreter {
    
    public static void Main(string[] args) {
        Console.Write("Enter the filename: ");
        //string file = "2_example";
        string file = Console.In.ReadLine();

        string language = 
        "";
        if (File.Exists(file)) {    
            language = File.ReadAllText(file);
            char[] line = language.ToCharArray();
            foreach (char ch in line)
            {
                Console.Write(ch);
            }
        } else {
            Console.WriteLine("File does not exists");
        }
        Console.WriteLine("\n");
        Scanner lex = new Scanner(language);
        Parser parser = new Parser(lex.Tokens);

        List<Token> tokens = lex.Tokens;
        
        foreach (Token aPart in tokens)
        {
            Console.WriteLine(aPart);
        }
    }
}
