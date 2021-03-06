namespace LexicalAnalysis
{
    public enum TokenType 
        {
            PLUS,
            MINUS,
            MULTIPLY,
            DIVIDE,
            LESSTHAN,
            EQUALS,
            AND,
            NOT,
            VAR,
            FOR,
            END,
            IN,
            DO,
            READ,
            PRINT,
            INT,
            STRING,
            INTEGER,
            BOOL,
            ASSERT,
            SEMICOLON,
            LPARENTHESES,
            RPARENTHESES,
            PUNCTUATION,
            IDENTIFIER,
            NONE,
            ASSIGN,
            DOUBLEDOTS,
        }
    
    public class Token
    {
        public TokenType terminal { get; set; }
        public object lex { get; set; }
        public int startPos {get; set; }
        public int lineNumber { get; set; }

        public Token(TokenType terminal, object lex, int startPos, int lineNumber)
        {
            this.terminal = terminal;
            this.lex = lex;
            this.startPos = startPos;
            this.lineNumber = lineNumber;
        }
        

        public override string ToString()
        {
            return "Token: " + terminal.ToString() +  " Value: " + lex.ToString() +  " at (" + lineNumber.ToString() + ", " + startPos.ToString() + ")";
        }
    }
}