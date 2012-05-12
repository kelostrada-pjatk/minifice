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

namespace Testowy_Projekt
{
    
    public partial class Form1 : Form
    {
        FileManager fileManager = new FileManager();

        public Form1()
        {
            InitializeComponent();

            List<string> ooo = new List<string>();

            ooo.Add("działa?");

            Foo(ooo);

            foreach (var s in ooo)
            {
                textBox1.Text += s;
            }

            /*
            Person Bartek = new Person("Bartek");
            fileManager.Serialize<Person>(@"Bartek", Bartek);
            Person Bartek2 = fileManager.Deserialize<Person>(@"Bartek");
            textBox1.Text = Bartek2.ToString();
        
            */
        }

        public void Foo(List<string> bzebze)
        {
            bzebze.Add("dziala! :)");
        }

    }

    public class Person
    {
        public string personName;
        
        public Test ukryte;

        public Person(string name)
        {
            personName = name;
            ukryte = new Test("ukryte pole");
        }

        public Person()
        {
            personName = null;
            ukryte = new Test();
        }

        public override string ToString()
        {
            return (personName+" "+ukryte);
        }

    }

    
    public class DataItem<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public DataItem()
        {

        }

        public DataItem(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    public class Pole
    {
        [XmlAttribute]
        public string Field;
        public Pole()
        {
            Field = "Pole";
        }
        public Pole(string a)
        {
            Field = a;
        }
        public static implicit operator Pole(string a)
        {
            return new Pole(a);
        }
    }

    public class Test
    {
        
        private string jeszczeBardziejUkryte;
        [XmlElement("ABC")]
        public string JeszczeBardziejUkryte
        {
            get { return jeszczeBardziejUkryte; }
            set { jeszczeBardziejUkryte = value; }
        }

        private Dictionary<Pole, int> slownik = new Dictionary<Pole, int>();

        [XmlArray]
        public List<DataItem<Pole, int>> Slownik
        {
            get
            {
                List<DataItem<Pole, int>> s = new List<DataItem<Pole, int>>(slownik.Count);
                foreach (Pole key in slownik.Keys)
                {
                    s.Add(new DataItem<Pole, int>(key, slownik[key]));
                }
                return s;
            }
            set
            {
                foreach (var di in value)
                {
                    slownik.Add(di.Key, di.Value);
                }
            }
        }

        public Test()
        {
            jeszczeBardziejUkryte = null;
            AktualizacjaSlownika();
        }
        public Test(string ukryte)
        {
            jeszczeBardziejUkryte = ukryte;
            AktualizacjaSlownika();
        }

        public void AktualizacjaSlownika()
        {
            slownik.Add("Kot", 3);
            slownik.Add("Pies", 2);
            slownik.Add("Mysz", 6);
        }

        public override string ToString()
        {
            return (jeszczeBardziejUkryte);
        }
    }


}
