using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FileManager
{

    public struct Osoba
    {
        public string name;
        public string surname;
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        FileManager fileManager = new FileManager();

        List<string> A = new List<string>();


        private void button1_Click(object sender, EventArgs e)
        {
            Osoba Os = new Osoba();
            Os.name = textBox1.Text;
            Os.surname = textBox2.Text;
            fileManager.Serialize<Osoba>("test",Os);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Osoba Os = fileManager.Deserialize<Osoba>("test");
            textBox1.Text = Os.name;
            textBox2.Text = Os.surname;
        }
    }
}
