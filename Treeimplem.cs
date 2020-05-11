using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public class TDNODE
    {
        public List<TDNODE> children = new List<TDNODE>();
        public Token token;
        public string Name;
        public string stringVal = "";
        public string datatype = "";
        public double value = Int32.MinValue;
        
        public TDNODE()
        {

        }
        public TDNODE(string N)
        {
            this.Name = N;
        }
    }
    class SyntaxAnalyser
    {
        public static List<Token> LT;
        static int i = 0;
        public static List<String> ErrorList = new List<string>();
        static bool bing = false;
        public static TDNODE Parse(List<Token> Tokens)
        {
            TDNODE root;
            LT = Tokens;
            //write your parser code
            root = Program();
            return root;
        }

        
        public static TDNODE match(Token_Class t)
        {

            TDNODE match = new TDNODE();
            if (i < LT.Count() && LT[i].token_type == t)
            {
                match.token = LT[i];
                bing = false;
                i++;
            }
            else
            {
                if (i >= LT.Count)
                {
                    string s = "Some essential lines doesn't exist.";
                    ErrorList.Add(s);
                }
                else if (!bing)
                {
                    int j = Scanner.NewLine.Count() - 1;
                    while (!(i <= Scanner.NewLine[j] && i > Scanner.NewLine[j - 1]))
                    {
                        j--;
                    }
                    if (i + 1 < LT.Count && LT[i + 1].token_type == t)
                    {
                        string s = "Extra ";
                        s += LT[i].lex;
                        ErrorList.Add(s);
                        match.token = LT[i + 1];
                        i += 2;
                    }
                    else if (i == Scanner.NewLine[j - 1] + 1)
                    {
                        string s = "Expected ";
                        s += t.ToString();
                        s += " here.";
                        ErrorList.Add(s);
                        bing = true;
                    }
                    else //if (i + 1 < LT.Count && LT[i + 1].token_type != t)
                    {
                        string s = "Expected ";
                        s += t.ToString();
                        s += " instead of ";
                        s += LT[i].lex;
                        ErrorList.Add(s);
                        i++;
                    }
                }

            }
            return match;
        }
        public static TDNODE Number()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("Number", Token_Class.other);
                node.children.Add(match(Token_Class.constant));
            }
            return node;
        }

        public static TDNODE Reserver_keyword()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("Reserver_keyword", Token_Class.reservedKeyword);

                if (LT[i].token_type == Token_Class.reservedKeyword)
                    node.children.Add(match(Token_Class.reservedKeyword));
                else if (LT[i].token_type == Token_Class.Else)
                    node.children.Add(match(Token_Class.Else));
                else if (LT[i].token_type == Token_Class.Elseif)
                    node.children.Add(match(Token_Class.Elseif));
                else if (LT[i].token_type == Token_Class.IF)
                    node.children.Add(match(Token_Class.IF));
                else if (LT[i].token_type == Token_Class.then)
                    node.children.Add(match(Token_Class.then));
                else if (LT[i].token_type == Token_Class.Return)
                    node.children.Add(match(Token_Class.Return));
                else if (LT[i].token_type == Token_Class.repeat)
                    node.children.Add(match(Token_Class.repeat));
                else if (LT[i].token_type == Token_Class.until)
                    node.children.Add(match(Token_Class.until));
            }
            return node;
        }
        public static TDNODE String()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("string", Token_Class.String);

                node.children.Add(match(Token_Class.String));
            }
            return node;
        }
        public static TDNODE Comment_State()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("comment_state", Token_Class.comment);

                node.children.Add(match(Token_Class.comment));
            }
            return node;
        }

        public static TDNODE Identifiers()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("identifier", Token_Class.Identifier);

                node.children.Add(match(Token_Class.Identifier));
            }
            return node;
        }
        public static TDNODE Factor()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("factor", Token_Class.other);
                if (i < LT.Count)
                {
                    if (LT[i].token_type == Token_Class.Identifier && i + 1 < LT.Count && LT[i + 1].token_type == Token_Class.LeftBracket)
                        node.children.Add(Function_Call());
                    else if (LT[i].token_type == Token_Class.Identifier)
                        node.children.Add(Identifiers());
                    else if (LT[i].token_type == Token_Class.String)
                        node.children.Add(String());
                    else if (LT[i].token_type == Token_Class.constant)
                        node.children.Add(Number());
                    else if (LT[i].lex == '('.ToString())
                    {
                        node.children.Add(match(Token_Class.LeftBracket));
                        node.children.Add(Expression());
                        node.children.Add(match(Token_Class.RightBracket));
                    }
                }
            }
            return node;
        }
        public static TDNODE Function_Part()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("function_part", Token_Class.other);

                //if (LT[i].lex == '('.ToString())
                // {
                node.children.Add(match(Token_Class.LeftBracket));
                if (i < LT.Count && LT[i].token_type != Token_Class.RightBracket)
                    node.children.Add(Expression());
                //  }
                while (LT[i].token_type != Token_Class.RightBracket)
                {
                    node.children.Add(match(Token_Class.comma));
                    node.children.Add(Expression());
                }
                //if (LT[i].lex == ')'.ToString())
                node.children.Add(match(Token_Class.RightBracket));
            }
            return node;
        }
        public static TDNODE Term()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("Term", Token_Class.other);
                node.children.Add(Factor());
                node.children.Add(TermOP());
            }
            return node;
        }
        public static TDNODE TermOP()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("Term op", Token_Class.other);

                if (i < LT.Count && (LT[i].token_type == Token_Class.MulOp || LT[i].token_type == Token_Class.Error))
                {
                    node.children.Add(match(Token_Class.MulOp));
                    node.children.Add(Factor());
                    node.children.Add(TermOP());
                }
                else
                {
                    TDNODE child = new TDNODE();
                    child.token = new Token(" ", Token_Class.Epsilon);
                    node.children.Add(child);
                }
            }
            return node;
        }
        public static TDNODE Function_Call()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("function call", Token_Class.other);

                node.children.Add(Identifiers());
                node.children.Add(Function_Part());
            }
            return node;
        }
        
        public static TDNODE AddOp()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("AddOp", Token_Class.AddOp);
                node.children.Add(match(Token_Class.AddOp));
            }
            return node;
        }

        public static TDNODE MulOp()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("MulOp", Token_Class.MulOp);
                node.children.Add(match(Token_Class.MulOp));
            }
            return node;
        }

        public static TDNODE Expression()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("Expression", Token_Class.other);

                node.children.Add(Term());
                node.children.Add(Exp());
            }
            return node;
        }
        public static TDNODE Exp()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("ExpressionOP", Token_Class.other);
                if (LT[i].token_type == Token_Class.AddOp || LT[i].token_type == Token_Class.Error)
                {
                    node.children.Add(AddOp());
                    node.children.Add(Term());
                    node.children.Add(Exp());
                }
                else
                {
                    TDNODE child = new TDNODE();
                    child.token = new Token("", Token_Class.Epsilon);
                    node.children.Add(child);
                }
            }
            return node;
        }

        public static TDNODE Assignment_Statement()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("Assignment_Statement", Token_Class.other);

                node.children.Add(Identifiers());
                node.children.Add(match(Token_Class.AssigmentOp));
                node.children.Add(Expression());
                node.children.Add(match(Token_Class.semicolon));
            }
            return node;

        }

        public static TDNODE Datatype()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("Datatype", Token_Class.dataType);

                node.children.Add(match(Token_Class.dataType));
            }
            return node;
        }

        public static TDNODE Declaration_Statement()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("Declaration_Statement", Token_Class.other);

                node.children.Add(Datatype());
                node.children.Add(Identifiers());
                if (i < LT.Count && (LT[i].token_type != Token_Class.comma && LT[i].token_type != Token_Class.semicolon))
                {
                    node.children.Add(match(Token_Class.AssigmentOp));
                    node.children.Add(Expression());
                }
                while (i < LT.Count && LT[i].token_type != Token_Class.semicolon)
                {
                    node.children.Add(match(Token_Class.comma));
                    node.children.Add(Identifiers());
                    if (i < LT.Count && (LT[i].token_type != Token_Class.comma && LT[i].token_type != Token_Class.semicolon))
                    {
                        node.children.Add(match(Token_Class.AssigmentOp));
                        node.children.Add(Expression());
                    }
                }

                //if (LT[i].lex == ';'.ToString())
                node.children.Add(match(Token_Class.semicolon));
            }
            return node;
        }

        public static TDNODE Read_Statement()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("Read_Statement", Token_Class.reservedKeyword);

                node.children.Add(match(Token_Class.reservedKeyword));
                node.children.Add(Identifiers());
                node.children.Add(match(Token_Class.semicolon));
            }
            return node;

        }

        public static TDNODE Write_Statement()
        {
            TDNODE node = new TDNODE();
            if (i < LT.Count)
            {
                node.token = new Token("Write_Statement", Token_Class.reservedKeyword);

                node.children.Add(match(Token_Class.reservedKeyword));
                node.children.Add(Expression());
                node.children.Add(match(Token_Class.semicolon));
            }
            return node;

        }
        
        //18
        public static TDNODE Return_Statement()
        {
            TDNODE return_statement = new TDNODE();
            if (i < LT.Count)
            {
                return_statement.token = new Token("Return_Statement", Token_Class.Return);
                return_statement.children.Add(match(Token_Class.Return));
                return_statement.children.Add(Expression());
                return_statement.children.Add(match(Token_Class.semicolon));
            }
            return return_statement;
        }

        //19
        public static TDNODE Condition_Operator()
        {
            TDNODE condition_op = new TDNODE();
            if (i < LT.Count)
            {
                condition_op.token = new Token("Condition_Operator", Token_Class.other);
                if (LT[i].token_type == Token_Class.LessThanOp)
                    condition_op.children.Add(match(Token_Class.LessThanOp));
                else if (LT[i].token_type == Token_Class.GreaterThanOp)
                    condition_op.children.Add(match(Token_Class.GreaterThanOp));
                else if (LT[i].token_type == Token_Class.IsEqualOp)
                    condition_op.children.Add(match(Token_Class.IsEqualOp));
                else if (LT[i].token_type == Token_Class.NotEqualOp)
                    condition_op.children.Add(match(Token_Class.NotEqualOp));
            }
            return condition_op;
        }

        //20
        public static TDNODE Condition_Statement()
        {
            TDNODE condition_statement = new TDNODE();
            if (i < LT.Count)
            {
                condition_statement.token = new Token("Condition_Statement", Token_Class.other);
                condition_statement.children.Add(Condition_Term());
                condition_statement.children.Add(Condition_Statement_2());
            }
            return condition_statement;
        }

        public static TDNODE Condition_Statement_2()
        {
            TDNODE cond_stmt_2 = new TDNODE();
            if (i < LT.Count)
            {
                cond_stmt_2.token = new Token("Condition_Statement_2", Token_Class.other);
                if (LT[i].token_type == Token_Class.Or_Operator)
                {
                    cond_stmt_2.children.Add(OrOp());
                    cond_stmt_2.children.Add(Condition_Term());
                    cond_stmt_2.children.Add(Condition_Statement_2());
                }
                else
                {
                    TDNODE child = new TDNODE();
                    child.token = new Token(" ", Token_Class.Epsilon);
                    cond_stmt_2.children.Add(child);
                }
            }
            return cond_stmt_2;
        }

        //21
        public static TDNODE Condition_Term()
        {
            TDNODE cond_term = new TDNODE();
            if (i < LT.Count)
            {
                cond_term.token = new Token("Condition_Term", Token_Class.other);
                cond_term.children.Add(Condition());
                cond_term.children.Add(Condition_Term_2());
            }
            return cond_term;
        }

        public static TDNODE Condition_Term_2()
        {
            TDNODE cond_term_2 = new TDNODE();
            if (i < LT.Count)
            {
                cond_term_2.token = new Token("Condition_Term_2", Token_Class.other);
                if (LT[i].token_type == Token_Class.And_Operator)
                {
                    cond_term_2.children.Add(AndOp());
                    cond_term_2.children.Add(Condition());
                    cond_term_2.children.Add(Condition_Term_2());
                }
                else
                {
                    TDNODE child = new TDNODE();
                    child.token = new Token("", Token_Class.Epsilon);
                    cond_term_2.children.Add(child);
                }

            }
            return cond_term_2;
        }

        //22
        public static TDNODE Condition()
        {
            TDNODE cond = new TDNODE();
            if (i < LT.Count)
            {
                cond.token = new Token("Condition", Token_Class.other);
                cond.children.Add(Expression());
                cond.children.Add(Condition_Operator());
                cond.children.Add(Expression());
            }
            return cond;
        }

        //23
        public static TDNODE AndOp()
        {
            TDNODE and_op = new TDNODE();
            if (i < LT.Count)
            {
                and_op.token = new Token("AndOp", Token_Class.And_Operator);
                and_op.children.Add(match(Token_Class.And_Operator));
            }
            return and_op;
        }

        //24
        public static TDNODE OrOp()
        {
            TDNODE or_op = new TDNODE();
            if (i < LT.Count)
            {
                or_op.token = new Token("OrOp", Token_Class.Or_Operator);
                or_op.children.Add(match(Token_Class.Or_Operator));
            }
            return or_op;
        }
        //
        //25
        public static TDNODE If_Statement()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count)
            {
                nd.token = new Token("If_Statement", Token_Class.IF);
                nd.children.Add(match(Token_Class.IF));
                nd.children.Add(Condition_Statement());
                nd.children.Add(match(Token_Class.then));
                nd.children.Add(Statements());
                nd.children.Add(d_If_Statement());
            }
            return nd;
        }
        public static TDNODE d_If_Statement()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count)
            {
                nd.token = new Token("d_If_Statement", Token_Class.other);
                if (i < LT.Count && LT[i].token_type == Token_Class.Elseif)
                {
                    nd.children.Add(Else_If_Statement());
                    nd.children.Add(d_If_Statement());
                }
                else
                    nd.children.Add(dd_If_Statement());
            }
            return nd;
        }
       public static TDNODE dd_If_Statement()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count)
            {
                nd.token = new Token("dd_If_Statement", Token_Class.other);
                if (i < LT.Count && LT[i].token_type == Token_Class.Else)
                    nd.children.Add(Else_Statement());
                nd.children.Add(match(Token_Class.end));
            }
            return nd;
        }
        
        public static TDNODE Else_If_Statement()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count)
            {
                nd.token = new Token("Else_If_Statement", Token_Class.Elseif);
                nd.children.Add(match(Token_Class.Elseif));
                nd.children.Add(Condition_Statement());
                nd.children.Add(match(Token_Class.then));
                nd.children.Add(Statements());
            }
            return nd;
        }
        //27
        public static TDNODE Else_Statement()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count)
            {
                nd.token = new Token("Else_Statement", Token_Class.Else);
                nd.children.Add(match(Token_Class.Else));
                nd.children.Add(Statements());
            }
            return nd;
        }
        public static TDNODE Statement()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count)
            {
                nd.token = new Token("Statement", Token_Class.other);
                if (i < LT.Count)
                {
                    if (LT[i].token_type == Token_Class.Identifier && i + 1 < LT.Count && LT[i + 1].token_type == Token_Class.LeftBracket)
                    {
                        nd.children.Add(Function_Call());
                        nd.children.Add(match(Token_Class.semicolon));
                    }
                    else if (LT[i].token_type == Token_Class.Identifier) nd.children.Add(Assignment_Statement());
                    else if (LT[i].token_type == Token_Class.dataType) nd.children.Add(Declaration_Statement());
                    else if (LT[i].token_type == Token_Class.reservedKeyword && LT[i].lex == "write") nd.children.Add(Write_Statement());
                    else if (LT[i].token_type == Token_Class.reservedKeyword && LT[i].lex == "read") nd.children.Add(Read_Statement());
                    else if (LT[i].token_type == Token_Class.Return) nd.children.Add(Return_Statement());
                    else if (LT[i].token_type == Token_Class.IF) nd.children.Add(If_Statement());
                    else if (LT[i].token_type == Token_Class.repeat) nd.children.Add(Repeat_Statement());
                    else if (LT[i].token_type == Token_Class.comment) nd.children.Add(Comment_State());
                }
            }
            return nd;
        }
        public static TDNODE Statements()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count)
            {
                nd.token = new Token("Statements", Token_Class.other);
                bool flag = false;
                if (LT[i].token_type == Token_Class.Return)
                {
                    int j = i + 1;
                    while (!flag && j < LT.Count && LT[j].token_type != Token_Class.RightCurlyBracket && LT[j].token_type != Token_Class.LeftCurlyBracket)
                    {
                        if (LT[j].token_type == Token_Class.Return) flag = true;
                        j++;
                    }
                }
                if (i < LT.Count && (LT[i].token_type == Token_Class.Identifier || LT[i].token_type == Token_Class.dataType || LT[i].token_type == Token_Class.reservedKeyword && LT[i].lex == "write" || LT[i].token_type == Token_Class.reservedKeyword && LT[i].lex == "read" || (flag && LT[i].token_type == Token_Class.Return) || LT[i].token_type == Token_Class.IF || LT[i].token_type == Token_Class.repeat || LT[i].token_type == Token_Class.comment))
                {
                    nd.children.Add(Statement());
                    nd.children.Add(Statements());
                }
                else
                {
                    TDNODE child = new TDNODE();
                    child.token = new Token(" ", Token_Class.Epsilon);
                    nd.children.Add(child);
                }
            }
            return nd;
        }
        //28
        public static TDNODE Repeat_Statement()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count)
            {
                nd.token = new Token("Repeat_Statement", Token_Class.other);
                nd.children.Add(match(Token_Class.repeat));
                nd.children.Add(Statements());
                nd.children.Add(match(Token_Class.until));
                nd.children.Add(Condition_Statement());
            }
            return nd;
        }
        //29
        public static TDNODE FunctionName()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count)
            {
                nd.token = new Token("FunctionName", Token_Class.other);
                nd.children.Add(Identifiers());
            }
            return nd;
        }
        //30
        public static TDNODE Parameter()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count)
            {
                nd.token = new Token("Parameter", Token_Class.other);
                nd.children.Add(Datatype());
                nd.children.Add(Identifiers());
            }
            return nd;
        }
        //31
        public static TDNODE Function_Declaration()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count)
            {
                nd.token = new Token("Function_Declaration", Token_Class.other);
                nd.children.Add(Datatype());
                nd.children.Add(FunctionName());
                nd.children.Add(match(Token_Class.LeftBracket));
                nd.children.Add(ParameterList());
                nd.children.Add(match(Token_Class.RightBracket));
            }
            return nd;
        }
        public static TDNODE ParameterList()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count && LT[i].token_type == Token_Class.dataType)
            {
                nd.token = new Token("ParameterList", Token_Class.other);
                nd.children.Add(Parameter());
                nd.children.Add(d_ParameterList());
            }
            else
            {
                TDNODE child = new TDNODE();
                child.token = new Token("Epsilon", Token_Class.Epsilon);
                nd.children.Add(child);
            }
            return nd;
        }
        public static TDNODE d_ParameterList()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count)
            {
                nd.token = new Token("d_ParameterList", Token_Class.other);
                if (i < LT.Count && LT[i].token_type == Token_Class.comma)
                {
                    nd.children.Add(match(Token_Class.comma));
                    nd.children.Add(Parameter());
                    nd.children.Add(d_ParameterList());
                }
                else
                {
                    TDNODE child = new TDNODE();
                    child.token = new Token("Epsilon", Token_Class.Epsilon);
                    nd.children.Add(child);
                }
            }
            return nd;
        }
        //32
        public static TDNODE Function_Body()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count)
            {
                nd.token = new Token("Function_Body", Token_Class.other);
                nd.children.Add(match(Token_Class.LeftCurlyBracket));
                nd.children.Add(Statements());
                nd.children.Add(Return_Statement());
                nd.children.Add(match(Token_Class.RightCurlyBracket));
            }
            return nd;
        }
        //33
        public static TDNODE Function_Statement()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count)
            {
                nd.token = new Token("Function_Statement", Token_Class.other);
                nd.children.Add(Function_Declaration());
                nd.children.Add(Function_Body());
            }
            return nd;
        }
        //34
        public static TDNODE Main_Function()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count)
            {
                nd.token = new Token("Main_Function", Token_Class.other);
                nd.children.Add(Datatype());
                nd.children.Add(match(Token_Class.reservedKeyword));
                nd.children.Add(match(Token_Class.LeftBracket));
                nd.children.Add(match(Token_Class.RightBracket));
                nd.children.Add(Function_Body());
            }
            return nd;
        }
        //35
        public static TDNODE Program()
        {
            TDNODE nd = new TDNODE();
            if (i < LT.Count)
            {
                nd.token = new Token("Program", Token_Class.other);
                if (i + 1 < LT.Count && LT[i + 1].token_type == Token_Class.Identifier)
                {
                    nd.children.Add(Function_Statement());
                    nd.children.Add(Program());
                }
                else
                    nd.children.Add(Main_Function());
            }
            return nd;
        }
        //use this function to print the parse tree in TreeView Toolbox
        public static TreeNode PrintParseTree(TDNODE root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(TDNODE root)
        {
            if (root == null || root.token == null)
                return null;
            TreeNode tree = new TreeNode(root.token.lex);
            if (root.children.Count == 0)
                return tree;
            foreach (TDNODE child in root.children)
            {
                if (child == null || child.token == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}