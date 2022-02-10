using System;
using System.IO;
using LexerAnalysis;

namespace Mini_PL_Interpreter
{
    
}

public class Interpreter {
    
    public static void Main(string[] args) {
        Console.Write("Enter the filename: ");
        string file = Console.In.ReadLine();

        string[] lines = {};
        if (File.Exists(file)) {    
            lines = File.ReadAllLines(file);
            foreach(string x in lines) {
                Console.WriteLine(x);
            }  
        } else {
            Console.WriteLine("File does not exists");
        }
        
        Scanner lex = new Scanner(lines);

        List<Token> tokens = lex.Tokens;
        foreach (Token aPart in tokens)
        {
            Console.WriteLine(aPart);
        }
    }
}
