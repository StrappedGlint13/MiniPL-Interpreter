using System;

namespace LexerAnalysis
{
    

    public class Scanner {
        public List<Token> Tokens { get; set; }
        private int lineNumber = 1;
        private int startPos = 1;

        private int currentPos = 0;

        private static readonly Dictionary<string, TokenType> reservedWords = new Dictionary<string, TokenType>()
        {
            {"for", TokenType.FOR},
            {"do", TokenType.DO},
            {"end", TokenType.END},
            {"in", TokenType.IN},
            {"var", TokenType.VAR},
            {"assert", TokenType.ASSERT},
            {"print", TokenType.PRINT},
            {"read", TokenType.READ},
            {"bool", TokenType.BOOL},
            {"string", TokenType.STRING},
            {"int", TokenType.INT}
        };

        public Scanner(string lines) {
            Tokens = scan(lines);
        }
        public List<Token> scan(string s) 
        {
            char[] line = s.ToCharArray();
            bool isString = false;
            bool isNumber = false;
            string str = "";
            List<Token> tokens =new List<Token>();
            foreach (char ch in line)
            {
                currentPos+=1;
                
                // doubleDot
                if (ch.Equals('.'))
                {                
                    if (str != ".") 
                    {
                        isNumber = false;
                        tokens.Add(new Token(TokenType.INTEGER, str, startPos, lineNumber));
                        startPos=currentPos;
                        str = ".";
                        continue;
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.DOUBLEDOTS, str+ch, startPos, lineNumber));
                        startPos=currentPos;
                        str = "";
                        continue;
                    }
                }
                
                // this maybe in another method
                if (isString && !ch.Equals('"'))
                {
                    str += ch;
                    continue;
                }

                if (isNumber && !isWhiteSpace(ch) && !(checkToken(ch.ToString(), startPos, lineNumber).terminal != TokenType.NONE))
                {
                    str += ch;
                    continue;
                }

                if (isString) 
                {
                    isString = false;
                    tokens.Add(new Token(TokenType.STRING, str+ch, startPos, lineNumber));
                    str = "";
                    startPos=currentPos;
                    continue;
                } 

                if (isNumber) 
                {
                    isNumber = false;
                    tokens.Add(new Token(TokenType.INTEGER, str, startPos, lineNumber));
                    str = "";
                    startPos=currentPos;
                }
                
                if (ch.Equals('t') && str == "in" || ch.Equals(':') || (ch.Equals('=') && str == ":")) 
                {
                    str += ch;
                    continue;
                }
       
                if (isWhiteSpace(ch) || ch.Equals(';') || checkToken(str, startPos, lineNumber).terminal != TokenType.NONE || 
                reservedWords.ContainsKey(str) || checkToken(ch.ToString(), startPos, lineNumber).terminal != TokenType.NONE) 
                {
                    if (checkToken(str, startPos, lineNumber).terminal != TokenType.NONE) 
                    {
                        
                        if (isNumberString(str))
                        {
                            tokens.Add(new Token(TokenType.INTEGER, str, startPos, lineNumber));
                            isNumber = false;
                        } else {
                            tokens.Add(checkToken(str, startPos, lineNumber));
                        }

                        str = "";
                        if (isNumeric(ch)) {str = ch.ToString(); isNumber = true;}
                        startPos=currentPos;
                        
                    } 
                    else
                    {
                        if (str.Length > 0) {
                            tokens.Add(isIdentifierOrKeyWord(str));
                            str = "";
                            startPos=currentPos;
                        }

                        if (checkToken(ch.ToString(), startPos, lineNumber).terminal != TokenType.NONE)
                        {
                            if (ch.Equals(';')) {
                                tokens.Add(new Token(TokenType.ENDOFLINE, ";", startPos, lineNumber));
                                str = "";
                                currentPos=startPos; 
                            } else {
                                tokens.Add(checkToken(ch.ToString(), startPos, lineNumber));
                            }
                           
                        }
                    } 
                    
                }
                else if (!isWhiteSpace(ch) && !ch.Equals(';') && !isEndofLine(ch))
                {
                    if (ch.Equals('"')) isString = true;
                    if (isNumeric(ch)) isNumber = true;
                    str += ch;
                }
                else if (isEndofLine(ch))
                {
                    str = "";
                    lineNumber++;
                    startPos=1;
                    currentPos=1; 
                }
                
            }
            return tokens;
        }

        public Token isIdentifierOrKeyWord(String str)
        {
            if (reservedWords.ContainsKey(str))
            {
                if (!reservedWords.TryGetValue(str, out TokenType type))
                {
                    return new Token(type, str, startPos, lineNumber); // throw error here
                }
                
                return new Token(type, str, startPos, lineNumber);
            }
            else 
            {
                return new Token(TokenType.IDENTIFIER, str, startPos, lineNumber);
            }
        }

        public Token checkToken(string ch, int startPos, int lineNumber)
        { 
            switch(ch) 
            {
            case "+":
                return new Token(TokenType.PLUS, ch, startPos, lineNumber);
                break;
            case "-":
                return new Token(TokenType.MINUS, ch, startPos, lineNumber);
                break;
            case "*":
                return new Token(TokenType.MULTIPLY, ch, startPos, lineNumber);
                break;
            case "/":
                return new Token(TokenType.DIVIDE, ch, startPos, lineNumber);
                break;
            case "<":
                return new Token(TokenType.LESSTHAN, ch, startPos, lineNumber);
                break;
            case "=":
                return new Token(TokenType.EQUALS, ch, startPos, lineNumber);
                break;
            case "&":
                return new Token(TokenType.AND, ch, startPos, lineNumber);
                break;
            case "!":
                return new Token(TokenType.NOT, ch, startPos, lineNumber);
                break;
            case ";":
                return new Token(TokenType.ENDOFLINE, ch, startPos, lineNumber);
                break;
            case "(":
                return new Token(TokenType.LPARENTHESES, ch, startPos, lineNumber);
                break;
            case ")":
                return new Token(TokenType.RPARENTHESES, ch, startPos, lineNumber);
                break;
            case ":":
                return new Token(TokenType.PUNCTUATION, ch, startPos, lineNumber);
                break;
            case ":=":
                return new Token(TokenType.ASSIGN, ch, startPos, lineNumber);
                break;
            default:
                return new Token(TokenType.NONE, ch, startPos, lineNumber); // throw lexical error
                break;
            
            }

        }

        private bool isWhiteSpace(char ch)
        {
            if (ch == ' ')
            {
                return true;
            }
            return false;
        }


        private bool isEndofLine(char ch)
        {
            if (ch == '\n')
            {
                return true;
            }
            return false;
        }

        private bool isNumeric(char ch)
        {
            if (!char.IsDigit(ch))
                {
                    return false;
                }
            return true;
        }

        private bool isNumberString(string str)
        {
            foreach (char ch in str)
            {
                if (!char.IsDigit(ch))
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}



