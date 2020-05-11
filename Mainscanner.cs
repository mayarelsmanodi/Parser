using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public class Mainscanner
    {

        string sourceCode;
        public TDNODE root;
        public List<string> tokens, types;
        Scanner scaner;
        tokentypes tokensClassification;
        List<Token> LT;
        public void compile()
        {
            types = new List<string>();
            int numberOfElement = tokens.Count();
            for (int i = 0; i < numberOfElement; ++i)
            {
                types.Add(tokensClassification.check(tokens[i]));
            }
        }
        public Mainscanner(string sourceCode)
        {
            this.sourceCode = sourceCode;
            scaner = new Scanner();
            tokens = new List<string>();
            tokens = scaner.scan(sourceCode);
            tokensClassification = new tokentypes();
            compile();
            //parseing process
            LT = new List<Token>();
            for (int i = 0; i < tokens.Count(); i++)
            {
                LT.Add(new Token(tokens[i], (Token_Class)Enum.Parse(typeof(Token_Class), types[i])));
            }
            root = SyntaxAnalyser.Parse(LT);
        }
    }
}