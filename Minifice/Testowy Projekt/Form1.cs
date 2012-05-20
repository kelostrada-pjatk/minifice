using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using ASD.Graph;
using System.Threading;
using System.Diagnostics;

namespace Testowy_Projekt
{
    
    public partial class Form1 : Form
    {
        FileManager fileManager = new FileManager();

        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            int x = 100;
            int y = 100;
            IGraph g = new AdjacencyListsGraph(false, x*y);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (rand.Next(0, 3) != 3)
                        g.AddEdge(i, j, rand.Next(1,3));
                }
            }
            textBox1.Text = "";

            Edge[] path;

            int s = 3, t = 24;

            g.DelEdge(s, t);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (g.AStar(s, t, out path))
            {
                foreach (var p in path)
                    textBox1.Text += p.From + ",";
                textBox1.Text += t;
            }

            textBox2.Text = sw.Elapsed.Milliseconds.ToString();
            
        }

        
    }



}
