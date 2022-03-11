namespace Mini_PL_Interpreter 
{
    abstract public class Error 
    {
        public bool SemanticError(string msg, int lineNumber, int startPos)
        {
            Console.WriteLine("Semantic Error: " + msg + " at (" + lineNumber + ", " + startPos + ")");
            return true;
        }

        public bool SyntaxError(string msg, int lineNumber, int startPos)
        {
            Console.WriteLine("Syntax Error: " + msg + " at (" + lineNumber + ", " + startPos + ")");
            return true;
        }

        public bool TypeError(string msg, int lineNumber, int startPos)
        {
            Console.WriteLine("Type Error: " + msg + " at (" + lineNumber + ", " + startPos + ")");
            return true;
        }

        public bool ExpectedError(int lineNumber, int startPos, object lex)
        {
            Console.WriteLine("Syntax error at (" + lineNumber  + ", " + startPos + "). Excpected: \"" + lex + "\"");
            return true;
        }

        public bool ErrorMessage(string msg)
        {
            Console.WriteLine(msg);
            return true;
        }
    }
    
}

