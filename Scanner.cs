using System;
using Mini_PL_Interpreter;

namespace LexicalAnalysis
{
    
    public class Scanner : Error {
        public List<Token> Tokens { get; set; }
        private int lineNumber = 1;
        private int startPos = 1;
        private int currentPos = 0;
        private string currentToken = "";
        private bool lineHasSemiColon = false; 
        public bool hasLexicalErrors;

        private Dictionary<string, TokenType> reservedWords = new Dictionary<string, TokenType>()
        {
            {"for", TokenType.FOR},{"do", TokenType.DO},{"end", TokenType.END},{"in", TokenType.IN},
            {"var", TokenType.VAR},{"assert", TokenType.ASSERT},{"print", TokenType.PRINT},
            {"read", TokenType.READ},{"bool", TokenType.BOOL},{"string", TokenType.STRING},{"int", TokenType.INT}
        };

        public Scanner(string lines) 
        {
            Tokens = Scan(lines);
        }
        public List<Token> Scan(string s) 
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
                        NewToken();
                        continue;
                    }
                }
                
                // this maybe in another method
                if (isString && !@char.Equals('"'))
                {
                    currentToken += @char;
                    continue;
                }

                if (isString) 
                {
                    isString = false;
                    tokens.Add(new Token(TokenType.STRING, currentToken+@char, startPos, lineNumber));
                    NewToken();
                    continue;
                } 


                if (isNumber && !IsWhiteSpace(@char) && !(IdentifyOperator(@char.ToString(), startPos, lineNumber).terminal != TokenType.NONE))
                {
                    currentToken += @char;
                    continue;
                }

            
                if (isNumber) 
                {
                    isNumber = false;
                    tokens.Add(new Token(TokenType.INTEGER, currentToken, startPos, lineNumber));
                    NewToken();
                }
                
                if (@char.Equals('t') && currentToken == "in" || @char.Equals(':') || (@char.Equals('=') && currentToken == ":")) 
                {
                    currentToken += @char;
                    currentPos+=1;
                    continue;
                }
       
                if (IsWhiteSpace(@char) || IdentifyOperator(currentToken, startPos, lineNumber).terminal != TokenType.NONE || 
                reservedWords.ContainsKey(currentToken) 
                || IdentifyOperator(@char.ToString(), startPos, lineNumber).terminal != TokenType.NONE) 
                {
                    if (IdentifyOperator(currentToken, startPos, lineNumber).terminal != TokenType.NONE) 
                    {
                        
                        if (IsNumberString(currentToken))
                        {
                            tokens.Add(new Token(TokenType.INTEGER, currentToken, startPos, lineNumber));
                            isNumber = false;
                        } else {
                            tokens.Add(IdentifyOperator(currentToken, startPos, lineNumber));
                        }
                        if (IsNumberChar(@char)) {currentToken = @char.ToString(); isNumber = true;}
                        NewToken();
                    } 
                    else
                    {
                        if (currentToken.Length > 0) {
                            tokens.Add(isIdentifierOrKeyWord(currentToken));
                            NewToken();
                            // line break
                            if (@char.Equals('\n'))
                            {
                                lineNumber+=1;
                                startPos=1;
                                continue;
                            }
                        }

                        if (IdentifyOperator(@char.ToString(), startPos, lineNumber).terminal != TokenType.NONE)
                        {
                            currentPos+=1;
                            tokens.Add(IdentifyOperator(@char.ToString(), startPos, lineNumber));
                            NewToken();
    
                            if (@char.Equals(';')) {
                                lineHasSemiColon = true;
                            } 
                        }
                    } 
                    
                }
                else if (!IsWhiteSpace(@char) && !@char.Equals(';') && !IsEndOfLine(@char))
                {
                    if (@char.Equals('"')) isString = true;
                    if (IsNumberChar(@char)) isNumber = true;
                    currentToken += @char;
                }
                else if (IsEndOfLine(@char))
                {
                    if (!lineHasSemiColon)  
                    {
                        hasLexicalErrors = LexicalError("Line " + lineNumber + " does not have semicolon at the end.");      
                        lineHasSemiColon = false;
                        currentToken = "";
                        lineNumber++;
                        startPos=1;
                        currentPos=1;
                    } 
                    lineHasSemiColon = false;
                    currentToken = "";
                    lineNumber++;
                    startPos=1;
                    currentPos=1;
                }         
            }
            if (!s[s.Length-1].Equals(';') && !s[s.Length-1].Equals('\n')) {
                hasLexicalErrors = LexicalError("Line " + lineNumber + " does not have semicolon at the end.");     
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

        public Token IdentifyOperator(string str, int startPos, int lineNumber)
        { 
            switch(str) 
            {
            case "+":
                return new Token(TokenType.PLUS, str, startPos, lineNumber);
                break;
            case "-":
                return new Token(TokenType.MINUS, str, startPos, lineNumber);
                break;
            case "*":
                return new Token(TokenType.MULTIPLY, str, startPos, lineNumber);
                break;
            case "/":
                return new Token(TokenType.DIVIDE, str, startPos, lineNumber);
                break;
            case "<":
                return new Token(TokenType.LESSTHAN, str, startPos, lineNumber);
                break;
            case "=":
                return new Token(TokenType.EQUALS, str, startPos, lineNumber);
                break;
            case "&":
                return new Token(TokenType.AND, str, startPos, lineNumber);
                break;
            case "!":
                return new Token(TokenType.NOT, str, startPos, lineNumber);
                break;
            case ";":
                return new Token(TokenType.SEMICOLON, str, startPos, lineNumber);
                break;
            case "(":
                return new Token(TokenType.LPARENTHESES, str, startPos, lineNumber);
                break;
            case ")":
                return new Token(TokenType.RPARENTHESES, str, startPos, lineNumber);
                break;
            case ":":
                return new Token(TokenType.PUNCTUATION, str, startPos, lineNumber);
                break;
            case ":=":
                return new Token(TokenType.ASSIGN, str, startPos, lineNumber);
                break;
            default:
                return new Token(TokenType.NONE, str, startPos, lineNumber);
                break;     
            }
        }

        private void NewToken()
        {
           currentToken = "";
           startPos=currentPos;
        }

        private bool IsWhiteSpace(char ch)
        {
            if (ch == ' ')
            {
                return true;
            }
            return false;
        }


        private bool IsEndOfLine(char ch)
        {
            if (ch == '\n')
            {
                return true;
            }
            return false;
        }

        private bool IsNumberChar(char ch)
        {
            if (!char.IsDigit(ch))
                {
                    return false;
                }
            return true;
        }

        private bool IsNumberString(string str)
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



