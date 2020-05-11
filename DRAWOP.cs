using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication4
{
    class DRAWOP
    {
        public static int GetOpWeight(string op)
        {
            int w = 0;
            switch (op)
            {
                case "+":
                case "-":
                    w = 1;
                    break;
                case "*":
                case "/":
                    w = 2;
                    break;

            }
            return w;
        }
        public static bool HasHigherPre(string op1, string op2)
        {
            int wOp1 = GetOpWeight(op1);
            int wOp2 = GetOpWeight(op2);
            if (wOp1 >= wOp2)
                return true;
            else
                return false;
        }

        public static int ApplyOperator(int op1, int op2, string oper)
        {
            int res = 0;
            switch (oper)
            {
                case "+":
                    res = op1 + op2;
                    break;
                case "-":
                    res = op1 - op2;
                    break;
                case "*":
                    res = op1 * op2;
                    break;
                case "/":
                    res = op1 / op2;
                    break;
            }
            return res;
        }

        
        public static float ApplyFloatOperator(float op1, float op2, string oper)
        {
            float res = 0;
            switch (oper)
            {
                case "+":
                    res = op1 + op2;
                    break;
                case "-":
                    res = op1 - op2;
                    break;
                case "*":
                    res = op1 * op2;
                    break;
                case "/":
                    res = op1 / op2;
                    break;
            }
            return res;
        }

        

    }
}