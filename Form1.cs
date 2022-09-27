using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace BINARNI_ukol2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                FileStream stream = new FileStream(@"../../cisla.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                BinaryReader binarcti = new BinaryReader(stream);
                BinaryWriter binarpis = new BinaryWriter(stream);

                int cislo;

                for (int i = 0; i < textBox1.Lines.Count(); i++)
                {
                    cislo = Convert.ToInt32(textBox1.Lines[i]);
                    binarpis.Write(cislo);
                    listBox1.Items.Add(cislo);
                }

                binarpis.Seek(0, SeekOrigin.Begin);
                int min = binarcti.ReadInt32();
                int pozice = (int)binarcti.BaseStream.Position;

                while (binarcti.BaseStream.Position < binarcti.BaseStream.Length)
                {
                    int x = binarcti.ReadInt32();
                    if (x < min)
                    {
                        min = x;
                        pozice = Convert.ToInt32(binarcti.BaseStream.Position);
                    }
                }

                binarpis.Seek(-sizeof(Int32), SeekOrigin.End);
                int posledni = binarcti.ReadInt32();
                binarpis.Seek(-sizeof(Int32), SeekOrigin.End);
                binarpis.Write(min);

                binarpis.Seek(pozice - sizeof(Int32), SeekOrigin.Begin);
                binarpis.Write(posledni);
                binarpis.Seek(0, SeekOrigin.Begin);

                while (binarcti.BaseStream.Position < binarcti.BaseStream.Length)
                {
                    cislo = binarcti.ReadInt32();
                    listBox2.Items.Add(cislo);
                }

                stream.Close();
                binarpis.Close();
                binarpis.Close();

            }
            catch
            {
                MessageBox.Show("CHYBA, PRAZDNY RADEK");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int n = Convert.ToInt32(textBox1.Text.ToString());

            FileStream stream = new FileStream(@"../../prvocislacisla.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryWriter binarpis = new BinaryWriter(stream);
            Random random = new Random();

            for (int i = 0; i < n; ++i)
            {
                int cislo = random.Next(1, 50);
                binarpis.Write(cislo);
                listBox1.Items.Add(cislo);
            }
            stream.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FileStream stream = new FileStream(@"../../cisla.dat", FileMode.Open, FileAccess.Read);
            BinaryReader binarpis = new BinaryReader(stream);
            StreamWriter streampis = new StreamWriter(@"../../prvocisla.txt");

            int pocet = 0;
            bool prvocislo = false;
            while (binarpis.BaseStream.Position < binarpis.BaseStream.Length)
            {
                int cislo = binarpis.ReadInt32();
                for (int i = 1; i <= cislo; ++i)
                {
                    if (cislo % i == 0)
                    {
                        ++pocet;
                    }
                    if (pocet <= 2)
                    {
                        prvocislo = true;
                    }
                    else 
                    {
                        prvocislo = false;
                    }
                }
                pocet = 0;

                if (prvocislo)
                {
                    streampis.WriteLine(cislo);
                    listBox2.Items.Add(cislo);
                    prvocislo = false;
                    pocet = 0;
                }
            }
            stream.Close();
            streampis.Close();
        }
    }
}
