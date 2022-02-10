using System;

namespace LexerAnalysis
{
    

    public class Scanner {
        public List<Token> Tokens { get; set; }
        private int lineNumber = 1;
        public Scanner(string[] lines) {
            Tokens = Scan(lines);
        }

        public List<Token> Scan(string[] lines) 
        {
            List<Token> tokens = new List<Token>();
            foreach(string l in lines) 
            {
                tokens.AddRange(analyzeString(l));
            }
            return tokens;
        }

        public List<Token> analyzeString(string s) 
        {
            char[] line = s.ToCharArray();
            string str = "";
            List<Token> tokens =new List<Token>();
            foreach (char ch in line)
            {
                if (ch.Equals(';')) 
                {
                    if (str.Length > 0) {
                        tokens.Add(checkToken(str, lineNumber));
                    }
                    
                    tokens.Add(new Token(TokenType.ENDOFLINE, ";", lineNumber));
                    lineNumber++;
                    
                }
                else if (!checkToken(ch.ToString(), lineNumber)
                .terminal.Equals(TokenType.REST) && !isWhiteSpace(ch))
                {
                    if (str.Length > 0) {
                        tokens.Add(checkToken(str, lineNumber));
                    }
                    tokens.Add(checkToken(ch.ToString(), lineNumber));
                    str = "";
                }
                else if (!isWhiteSpace(ch))
                {
                    str = str + ch;
                }
                else 
                {
                    if (str.Length > 0) {
                        tokens.Add(checkToken(str, lineNumber));
                    }
                    str = "";
                }
            }
            return tokens;
        }

        public Token checkToken(string str, int lineNumber)
        {
            switch(str) 
            {
            case "+":
                return new Token(TokenType.PLUS, str, lineNumber);
                break;
            case "-":
                return new Token(TokenType.MINUS, str, lineNumber);
                break;
            case "*":
                return new Token(TokenType.MULTIPLY, str, lineNumber);
                break;
            case "/":
                return new Token(TokenType.DIVIDE, str, lineNumber);
                break;
            case "<":
                return new Token(TokenType.LESSTHAN, str, lineNumber);
                break;
            case "=":
                return new Token(TokenType.EQUALS, str, lineNumber);
                break;
            case "&":
                return new Token(TokenType.AND, str, lineNumber);
                break;
            case "!":
                return new Token(TokenType.NOT, str, lineNumber);
                break;
            case "var":
                return new Token(TokenType.VAR, str, lineNumber);
                break;
            case "for":
                return new Token(TokenType.FOR, str, lineNumber);
                break;
            case "end":
                return new Token(TokenType.END, str, lineNumber);
                break;
            case "in":
                return new Token(TokenType.IN, str, lineNumber);
                break;
            case "do":
                return new Token(TokenType.DO, str, lineNumber);
                break;
            case "read":
                return new Token(TokenType.READ, str, lineNumber);
                break;
            case "print":
                return new Token(TokenType.PRINT, str, lineNumber);
                break;
            case "int":
                return new Token(TokenType.INT, str, lineNumber);
                break;
            case "string":
                return new Token(TokenType.STRING, str, lineNumber);
                break;
            case "bool":
                return new Token(TokenType.BOOL, str, lineNumber);
                break;
            case "assert":
                return new Token(TokenType.ASSERT, str, lineNumber);
                break;
            case ";":
                return new Token(TokenType.ENDOFLINE, str, lineNumber);
                break;
            case "(":
                return new Token(TokenType.LPARENTHESES, str, lineNumber);
                break;
            case ")":
                return new Token(TokenType.RPARENTHESES, str, lineNumber);
                break;
            case ":":
                return new Token(TokenType.TYPE, str, lineNumber);
                break;
            case "\"":
                return new Token(TokenType.DOUBLEQUOTES, str, lineNumber);
                break;
            default:
                return new Token(TokenType.REST, str, lineNumber);
                break;
            
            }

        }

        public bool isWhiteSpace(char ch)
        {
            if (ch == ' ')
            {
                return true;
            }
            return false;
        }
    }
}



