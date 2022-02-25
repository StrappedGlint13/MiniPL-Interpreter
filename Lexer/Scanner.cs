using System;

namespace LexerAnalysis
{
    

    public class Scanner {
        public List<Token> Tokens { get; set; }
        private int lineNumber = 1;
        private int startPos = 1;

        private int currentPos = 0;
        private string currentToken = "";
        
        private bool lineHasSemiColon = false; 

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
            
            List<Token> tokens =new List<Token>();
            foreach (char @char in line)
            {
                currentPos+=1;
                
                // doubleDot
                if (@char.Equals('.'))
                {                
                    if (currentToken != ".") 
                    {
                        isNumber = false;
                        tokens.Add(new Token(TokenType.INTEGER, currentToken, startPos, lineNumber));
                        startPos=currentPos;
                        currentToken = ".";
                        continue;
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.DOUBLEDOTS, currentToken+@char, startPos, lineNumber));
                        move();
                        continue;
                    }
                }
                
                // this maybe in another method
                if (isString && !@char.Equals('"'))
                {
                    currentToken += @char;
                    continue;
                }

                if (isNumber && !isWhiteSpace(@char) && !(checkToken(@char.ToString(), startPos, lineNumber).terminal != TokenType.NONE))
                {
                    currentToken += @char;
                    continue;
                }

                if (isString) 
                {
                    isString = false;
                    tokens.Add(new Token(TokenType.STRING, currentToken+@char, startPos, lineNumber));
                    move();
                    continue;
                } 

                if (isNumber) 
                {
                    isNumber = false;
                    tokens.Add(new Token(TokenType.INTEGER, currentToken, startPos, lineNumber));
                    move();
                }
                
                if (@char.Equals('t') && currentToken == "in" || @char.Equals(':') || (@char.Equals('=') && currentToken == ":")) 
                {
                    currentToken += @char;
                    currentPos+=1;
                    continue;
                }
       
                if (isWhiteSpace(@char) || checkToken(currentToken, startPos, lineNumber).terminal != TokenType.NONE || 
                reservedWords.ContainsKey(currentToken) || checkToken(@char.ToString(), startPos, lineNumber).terminal != TokenType.NONE) 
                {
                    if (checkToken(currentToken, startPos, lineNumber).terminal != TokenType.NONE) 
                    {
                        
                        if (isNumberString(currentToken))
                        {
                            tokens.Add(new Token(TokenType.INTEGER, currentToken, startPos, lineNumber));
                            isNumber = false;
                        } else {
                            tokens.Add(checkToken(currentToken, startPos, lineNumber));
                        }
                        if (isNumeric(@char)) {currentToken = @char.ToString(); isNumber = true;}
                        move();
                    } 
                    else
                    {
                        if (currentToken.Length > 0) {
                            tokens.Add(isIdentifierOrKeyWord(currentToken));
                            move();
                        }

                        if (checkToken(@char.ToString(), startPos, lineNumber).terminal != TokenType.NONE)
                        {
                            tokens.Add(checkToken(@char.ToString(), startPos, lineNumber));
                            move();
    
                            if (@char.Equals(';')) {
                                lineHasSemiColon = true;
                            } 
                        }
                    } 
                    
                }
                else if (!isWhiteSpace(@char) && !@char.Equals(';') && !isEndofLine(@char))
                {
                    if (@char.Equals('"')) isString = true;
                    if (isNumeric(@char)) isNumber = true;
                    currentToken += @char;
                }
                else if (isEndofLine(@char))
                {
                    if (!lineHasSemiColon) Console.WriteLine("Lexical Error: Line {0} does not have semicolon at the end.", lineNumber);      
                    lineHasSemiColon = false;
                    currentToken = "";
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
                return new Token(TokenType.SEMICOLON, ch, startPos, lineNumber);
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

        private void move()
        {
           currentToken = "";
           startPos=currentPos;
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



