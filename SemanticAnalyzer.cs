using LexicalAnalysis;
using MiniPL_Interpreter.AST;
using System.Collections.Generic;
using System;
using Mini_PL_Interpreter;

namespace MiniPL_Interpreter.SemanticAnalysis
{
    public class SemanticAnalyzer : Error {
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
                case TokenType.IDENTIFIER: AnalyzeIdentifier((VariableStmt)node); break;
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

        public void AnalyzeIdentifier(VariableStmt stmt)
        {
            ASTNode or = ((ASTNode)stmt);
            object value = or.value;
            Console.WriteLine(value);
            identifierIsAlreadyInUse(stmt.token.lex, value, stmt.token.lineNumber, stmt.token.startPos);
        }

        public void identifierIsAlreadyInUse(object lex, object value, int lineNumber, int startPos)
        {
            if (!identifiers.ContainsKey(lex))
            {
            identifiers.Add(lex, value);
            }
            else
            {
            HasSemanticErrors = SemanticError("There are too many arguments on print statement", lineNumber, startPos);
            }       
        }

        public void AnalyzeVarStmt(ASTNode stmt)
        {
            if (stmt.GetType() == typeof(VarStmt)) 
            {
                VarStmt varStmt = ((VarStmt) stmt); 
                TypeCheckAll((TypeAST)varStmt.type);
                IdentifierAST identifier = ((IdentifierAST)varStmt.identifier);
                identifierIsAlreadyInUse(identifier.token.lex, null, stmt.token.lineNumber, stmt.token.startPos);
            } else {
                VarAssignmentStmt varStmt = ((VarAssignmentStmt) stmt); 
                TypeCheckAll((TypeAST)varStmt.type);
                IdentifierAST identifier = ((IdentifierAST)varStmt.identifier);
                identifierIsAlreadyInUse(identifier.token.lex, null, stmt.token.lineNumber, stmt.token.startPos);
            }
        }

        public void TypeCheckAll(TypeAST type)
        {
            if (type == null) return;
            
            TokenType terminal = type.token.terminal;
            if (terminal != TokenType.INT && terminal != TokenType.STRING 
            && terminal != TokenType.BOOL)
            {
                HasSemanticErrors = ErrorMessage("Error: Type should be integer, string or bool.");
            }
        }

        public void AnalyzePrint(PrintStmt stmt)
        {
            
            if (stmt.right == null) {
                HasSemanticErrors = SemanticError("No argument given for print", stmt.token.lineNumber, stmt.token.startPos);
                return;
            }

            if (stmt.right.GetType() == typeof(OperandAST))
            {
                Token node = stmt.right.token;
                if (node.terminal.Equals(TokenType.IDENTIFIER))
                {
                    isDeclaredGlobally(node.lex);
                }
                
                if (!node.terminal.Equals(TokenType.STRING)
                && !node.terminal.Equals(TokenType.INTEGER) && !node.terminal.Equals(TokenType.IDENTIFIER))
                {
                    HasSemanticErrors = SemanticError("Type error: Print value should be identified, string literal or number", node.lineNumber, node.startPos);
                }
            } else {
                HasSemanticErrors = SemanticError("There are too many arguments on print statement", stmt.token.lineNumber, stmt.token.startPos);
            }
        }
            
        public void isDeclaredGlobally(object lex)
        {
            if (!identifiers.ContainsKey(lex)) 
            {
                HasSemanticErrors = ErrorMessage("Declaration error: Identifier \"" + lex + "\" is not declared before use.");
            }
        }
    }

    
}



