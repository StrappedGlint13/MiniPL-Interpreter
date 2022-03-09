using LexicalAnalysis;
using MiniPL_Interpreter.AST;
using System.Collections.Generic;
using System;
using MiniPL_Interpreter.AST;

namespace MiniPL_Interpreter.SemanticAnalysis
{
    public class SemanticAnalyzer {
        private List<ASTNode> ASTNodes;
        public bool HaveSemanticErrors;
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
                    HaveSemanticErrors = true;
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
                    HaveSemanticErrors = true;
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
                HaveSemanticErrors = true;
                Console.WriteLine("Error: Type should be integer, string or bool.");
            }
        }

        public void AnalyzePrint(PrintStmt stmt)
        {
            if (stmt.right.GetType() == typeof(OperandAST))
            {
                Token ast = stmt.right.token;
                if (ast.terminal.Equals(TokenType.IDENTIFIER))
                {
                    isDeclaredGlobally(ast.lex);
                }
                
                if (!ast.terminal.Equals(TokenType.STRING)
                && !ast.terminal.Equals(TokenType.INT))
                {
                    Console.WriteLine("Type error: Print value should be String or Integer at (" + ast.lineNumber + ", " + ast.startPos + ")");
                    HaveSemanticErrors = true;
                }
            }
        }
            
        public void isDeclaredGlobally(object lex)
        {
            Console.WriteLine(identifiers.ContainsKey(lex));
            if (!identifiers.ContainsKey(lex)) 
            {
                Console.WriteLine("Type error: Identifier \"" + lex + "\" is not declared before use.");
                HaveSemanticErrors = true;
            }
        }
    }

    
}



