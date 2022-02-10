namespace LexerAnalysis
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
            BOOL,
            ASSERT,
            REST,
            ENDOFLINE,
            LPARENTHESES,
            RPARENTHESES,
            TYPE,
            DOUBLEQUOTES
        }
    
    public class Token
    {
        public TokenType terminal { get; set; }
        public string lex { get; set; }

        public int lineNumber { get; set; }

        public Token(TokenType terminal, string lex, int lineNumber)
        {
            this.terminal = terminal;
            this.lex = lex;
            this.lineNumber = lineNumber;
        }

        public override string ToString()
        {
            return base.ToString() + ": " + terminal.ToString() + " " + lex.ToString() + " " + lineNumber.ToString();
        }
    }
}