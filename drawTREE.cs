using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public class value
    {
        public double val;
        public string stringVal;
        public string dataType;
        public string scope;
        public value()
        {
            val = Int32.MinValue;
            dataType = "";
            scope = "";
            stringVal = "";
        }
    };

    public class FunctionValue
    {
        public string datatype;
        public List<string> parmsDatatypes;
        public List<string> parmsNames;
        public int numOfParms;
        public double returnValue;
        public string returnStringVal;
        public FunctionValue()
        {
            datatype = "";
            numOfParms = 0;
            returnValue = Int32.MinValue;
            parmsDatatypes = new List<string>();
            parmsNames = new List<string>();
            returnStringVal = "";
        }

    }

    
    class drawTREE
    {
        
        
        public static List<KeyValuePair<string, int>> nestedVars = new List<KeyValuePair<string, int>>();
        public static List<KeyValuePair<TDNODE, string>> DeclaredFunctions = new List<KeyValuePair<TDNODE, string>>();
        public static Dictionary<KeyValuePair<string, string>, value> symbolTable = new Dictionary<KeyValuePair<string, string>, value>();
        public static List<string> error = new List<string>();
    

       

        
       
        public static TreeNode PrintSemanticTree(TDNODE root)
        {
            TreeNode tree = new TreeNode("Syntax Tree");
            TreeNode treeRoot = PrintsyntaxTree(root);
            tree.Expand();
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintsyntaxTree(TDNODE root)
        {
            if (root == null || root.token == null)
                return null;

            TreeNode tree;
            if (root.value == Int32.MinValue && root.datatype == "")
                //tree = new TreeNode(root.Name);
                tree = new TreeNode(root.token.lex);
            else if (root.value != Int32.MinValue && root.datatype == "")
                //tree = new TreeNode(root.Name + " & its value is: " + root.value);
                tree = new TreeNode(root.token.lex + " & its value is: " + root.value);
            else if (root.value == Int32.MinValue && root.datatype != "")
                //tree = new TreeNode(root.Name + " & its datatype is: " + root.datatype);
                tree = new TreeNode(root.token.lex + " & its datatype is: " + root.datatype);
            else
                //tree = new TreeNode(root.Name + " & its value is: " + root.value + " & datatype is: " + root.datatype);
                tree = new TreeNode(root.token.lex + " & its value is: " + root.value + " & datatype is: " + root.datatype);
            tree.Expand();
            if (root.children.Count == 0)
                return tree;
            foreach (TDNODE child in root.children)
            {
                if (child == null || child.token == null)
                    continue;
                tree.Nodes.Add(PrintsyntaxTree(child));
            }
            return tree;
        }
    }

}