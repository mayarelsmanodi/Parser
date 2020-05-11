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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        List<string> GetLines(string Jaconcode)
        {

            List<string> Lines = new List<string>();

            string tmp = "";

            for (int i = 0; i < Jaconcode.Length; i++)
            {
                if (Jaconcode[i] == '\n' || Jaconcode[i] == '\r') continue;
                tmp += Jaconcode[i];

                if (Jaconcode[i] == ';')
                {
                    Lines.Add(tmp);
                    tmp = "";
                }
            }

            return Lines;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SyntaxAnalyser.ErrorList.Clear();
            Scanner.NewLine.Clear();
            
            string code = JaconCode.Text;
            Mainscanner compile = new Mainscanner(code);


            int numberOfElement = compile.tokens.Count();

            DataTable dt = new DataTable();
            dt.Columns.Add("Lexems");
            dt.Columns.Add("Type");

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Lexems");

            List<string> Errors = new List<string>();

            for (int i = 0; i < numberOfElement; ++i)
            {
                
                if (compile.types[i].ToString() == "Error")
                {
                    dt2.Rows.Add(compile.tokens[i].ToString());
                }
                else
                    dt.Rows.Add(compile.tokens[i].ToString(), compile.types[i].ToString());
            }
            dataGridView.DataSource = dt;
            
           // SemanticAnalyser.TraverseTree(compile.root);
            treeView1.Nodes.Add(drawTREE.PrintSemanticTree(compile.root));

            
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}