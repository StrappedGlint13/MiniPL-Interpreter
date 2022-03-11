using LexicalAnalysis;
using MiniPL_Interpreter.AST;
using System.Collections.Generic;
using System;
using MiniPL_Interpreter.AST;

namespace MiniPL_Interpreter.SemanticAnalysis
{
    public class SemanticAnalyzer {
        private List<ASTNode> ASTNodes;
        public bool HasSemanticErrors;
        public Dictionary<object, object> identifiers;
        public SemanticAnalyzer(List<ASTNode> ASTNodes)
        {
            this.ASTNodes = new List<ASTNode>();
            this.identifiers = new Dictionary<object, object>();
        }

        public void AnalyzeSemantics(List<ASTNode> ASTNodes)
        {
            foreach (ASTNode node in ASTNodes)
            {
                switch(node.token.terminal) 
                {
                case TokenType.VAR: AnalyzeVarStmt(node); break;
                case TokenType.IDENTIFIER: break;
                case TokenType.PRINT: AnalyzePrint((PrintStmt)node); break;
                case TokenType.READ:  break;
                case TokenType.ASSERT:  break;
                case TokenType.FOR: break;
                default:
                    HasSemanticErrors = true;
                    Console.WriteLine("Some unexpected error occurred.");
                    break;     
                }
            }
        }
        public void AnalyzeVarStmt(ASTNode stmt)
        {
            if (stmt.GetType() == typeof(VarStmt)) 
            {
                VarStmt varStmt = ((VarStmt) stmt); 
                TypeCheckAll((TypeAST)varStmt.type);
                IdentifierAST identifier = ((IdentifierAST)varStmt.identifier);
                if (!identifiers.ContainsKey(identifier.token.lex))
                {
                identifiers.Add(identifier.token.lex, 0);
                }
                else
                {
                HasSemanticErrors = true;
                Console.WriteLine("Semantic Error: Identifiers can be declared once only. At (" + identifier.token.lineNumber + ", " + identifier.token.startPos + ")");
                }       
            } else {
                VarAssignmentStmt varStmt = ((VarAssignmentStmt) stmt); 
                TypeCheckAll((TypeAST)varStmt.type);
                IdentifierAST identifier = ((IdentifierAST)varStmt.identifier);
                if (!identifiers.ContainsKey(identifier.token.lex))
                {
                identifiers.Add(identifier.token.lex, 0);
                }
                else
                {
                HasSemanticErrors = true;
                Console.WriteLine("Semantic Error: Identifiers can be declared once only. At (" + identifier.token.lineNumber + ", " + identifier.token.startPos + ")");
                }
            }
        }

        public void TypeCheckAll(TypeAST type)
        {
            TokenType terminal = type.token.terminal;
            if (terminal != TokenType.INT && terminal != TokenType.STRING 
            && terminal != TokenType.BOOL)
            {
                HasSemanticErrors = true;
                Console.WriteLine("Error: Type should be integer, string or bool.");
            }
        }

        public void AnalyzePrint(PrintStmt stmt)
        {
            
            if (stmt.right == null) return;

            if (stmt.right.GetType() == typeof(OperandAST))
            {
                Token node = stmt.right.token;
                if (node.terminal.Equals(TokenType.IDENTIFIER))
                {
                    isDeclaredGlobally(node.lex);
                }
                
                if (!node.terminal.Equals(TokenType.STRING)
                && !node.terminal.Equals(TokenType.INT) && !node.terminal.Equals(TokenType.IDENTIFIER))
                {
                    SemanticError("Type error: Print value should be identified, string literal or number", node.lineNumber, node.startPos);
                }
            } else {
                SemanticError("There are too many arguments on print statement", stmt.token.lineNumber, stmt.token.startPos);
            }
        }
            
        public void isDeclaredGlobally(object lex)
        {
            Console.WriteLine(identifiers.ContainsKey(lex));
            if (!identifiers.ContainsKey(lex)) 
            {
                Console.WriteLine("Declaration error: Identifier \"" + lex + "\" is not declared before use.");
                HasSemanticErrors = true;
            }
        }

        public void SemanticError(string msg, int lineNumber, int startPos)
        {
            Console.WriteLine("Semantic Error: " + msg + " at (" + lineNumber + ", " + startPos + ")");
            HasSemanticErrors = true;
        }

        public void TypeError(string msg, int lineNumber, int startPos)
        {
            Console.WriteLine("Type Error: " + msg + " at (" + lineNumber + ", " + startPos + ")");
            HasSemanticErrors = true;
        }
    }

    
}



