namespace Mini_PL_Interpreter 
{
    /// <summary>
    /// An abstract class for error handling.
    /// </summary>
    abstract public class Error 
    {
        /// <summary>
        /// A method for semantic errors.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="lineNumber"></param>
        /// <param name="startPos"></param>
        /// <returns>bool</returns>
        public bool SemanticError(string msg, int lineNumber, int startPos)
        {
            Console.WriteLine("Semantic Error: " + msg + " at (" + lineNumber + ", " + startPos + ")");
            return true;
        }
        /// <summary>
        /// A method for syntax errors.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="lineNumber"></param>
        /// <param name="startPos"></param>
        /// <returns>bool</returns>
        public bool SyntaxError(string msg, int lineNumber, int startPos)
        {
            Console.WriteLine("Syntax Error: " + msg + " at (" + lineNumber + ", " + startPos + ")");
            return true;
        }

        /// <summary>
        /// A method for lexical errors.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>bool</returns>
        public bool LexicalError(string msg)
        {
            Console.WriteLine("Lexical Error: " + msg);
            return true;
        }
        /// <summary>
        /// A method for errors with expecting message.
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <param name="startPos"></param>
        /// <param name="lex"></param>
        /// <returns>bool</returns>
        public bool ExpectedError(int lineNumber, int startPos, object lex)
        {
            Console.WriteLine("Syntax error at (" + lineNumber  + ", " + startPos + "). Excpected: \"" + lex + "\"");
            return true;
        }
        /// <summary>
        /// A method for general error message.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>bool</returns>
        public bool ErrorMessage(string msg)
        {
            Console.WriteLine(msg);
            return true;
        }
    }
    
}

