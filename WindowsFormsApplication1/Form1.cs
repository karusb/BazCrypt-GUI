using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;







namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        //[DllImport("C:\\Users\\Enovas-P02\\source\\repos\\BazCryptLIB\\x64\\Release\\BazCryptLIB.dll", CharSet = CharSet.Ansi , CallingConvention = CallingConvention.Cdecl )]
        //public static extern void init(string msg, int msgL, string password, int pwL, int gen, int algorithm, bool verbose);

        //[DllImport("C:\\Users\\Enovas-P02\\source\\repos\\BazCryptLIB\\x64\\Release\\BazCryptLIB.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern StringBuilder getMsg();


        [DllImport("BazCryptLIB64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string BazCrypt(string MESSAGE, string password, int generations, int algorithm = 0, bool verbose = false);

        //[DllImport("C:\\Users\\Enovas-P02\\source\\repos\\BazCryptLIB\\x64\\Release\\BazCryptLIB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int alicanbalon(int it);

        //[DllImport("C:\\Users\\Enovas-P02\\source\\repos\\BazCryptLIB\\x64\\Release\\BazCryptLIB.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int alicankofte(string it);

        //[DllImport("C:\\Users\\Enovas-P02\\source\\repos\\BazCryptLIB\\x64\\Release\\BazCryptLIB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //[return: MarshalAs(UnmanagedType.LPStr)]
        //public static extern string alicankoftehor(string it);

        //[DllImport("C:\\Users\\Enovas-P02\\source\\repos\\BazCryptLIB\\x64\\Release\\BazCryptLIB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]

        //public static extern int alicankoftehan(string it);
        //[DllImport("C:\\Users\\Enovas-P02\\source\\repos\\BazCryptLIB\\x64\\Release\\BazCryptLIB.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]

        //public static extern void alicankoftecan(StringBuilder it, int len);


        int algo = 0;
        bool filein = false;
        bool fileo = false;
        public Form1()
        {
            InitializeComponent();
        }
        public struct mychararray
        {
            public string ks;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            progressBar1.Value = 0;
            string rr = "0000";
            string oldrr = "0000";
            string toldrr = "0000";
            int ges = 100;
            
            bool verbo = false;
            int.TryParse(GBox.Text, out ges);
            if (filein && (DialogResult.OK == MessageBox.Show("Warning your file will be encrypted!  For the safety of your file, encrypted file will be saved as a new file ending with .baz", "Are you sure to encrypt your file?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)))
            {
                BinaryReader br;
                StreamWriter sw;
                //StreamReader sr;
                //reading from the file
                int i = 0;
                byte[] filebyte;
                br = new BinaryReader(new FileStream(MBox.Text, FileMode.Open));
                //sr = new StreamReader(MBox.Text);
                FileInfo infile = new FileInfo(MBox.Text);
          
                i = Convert.ToInt16(infile.Length);
                MBox.MaxLength = i;
                filebyte = new byte[infile.Length];

                filebyte = br.ReadBytes(i);
                br.Close();
                
                //bw = new BinaryWriter(new FileStream(MBox.Text+".baz", FileMode.CreateNew));
                sw = new StreamWriter(MBox.Text + ".baz");
                MBox.Text = Encoding.UTF8.GetString(filebyte);
                //MBox.Text = sr.ReadToEnd();
                //sr.Close();
                do
                {
                    toldrr = oldrr;
                    oldrr = rr;
                    rr = BazCrypt(MBox.Text, PBox.Text.ToString(), ges, algo, verbo);
                    OBox.Text = rr;
                    if (toldrr == oldrr) progressBar1.Value = 1;
                    if (rr == oldrr && rr == toldrr) progressBar1.Value = 2;
          
                } while (rr != oldrr && rr!=toldrr);
                sw.Write(rr);
                sw.Close();
                //bw.Write(rr);
                //bw.Close();

                filein = false;

            }
            else
            {
                do
                {
                    toldrr = oldrr;
                    oldrr = rr;
                    rr = BazCrypt(MBox.Text.ToString(), PBox.Text.ToString(), ges, algo, verbo);
                    OBox.Text = rr;
                } while (rr != oldrr && rr != toldrr);
                if(fileo)
                {
                    StreamWriter sc;
                    sc = new StreamWriter(fnameBox.Text);
                    sc.Write(rr);
                    sc.Close();
                }
            }




            progressBar1.Value = 3;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                algo = 0;
                radioButton3.Checked = false;
                radioButton2.Checked = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                algo = 1;
                radioButton1.Checked = false;
                radioButton3.Checked = false;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            
            if (radioButton3.Checked)
            {
                algo = 2;
                radioButton1.Checked = false;
                radioButton2.Checked = false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            label2.Text = "Message";
            MBox.Width = PBox.Width;
            ChooseB.Visible = false;
            filein = false;
            checkBox1.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label2.Text = "File";
            MBox.Width = PBox.Width - 200;
            ChooseB.Visible = true;
            filein = true;
            checkBox1.Visible = false;
        }

        private void ChooseB_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();


            openFileDialog1.InitialDirectory = @"C:\";

            openFileDialog1.Title = "Browse Text Files";



            openFileDialog1.CheckFileExists = true;

            openFileDialog1.CheckPathExists = true;



            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|BazCrypt Files (*.baz)|*.baz";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowReadOnly = true;

            filein = true;


            if (openFileDialog1.ShowDialog() == DialogResult.OK)

            {

                MBox.Text = openFileDialog1.FileName;

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {
                label6.Visible = true;
                fnameBox.Visible = true;
                fileo = true;
            }
            else
            {
                label6.Visible = false;
                fnameBox.Visible = false;
                fileo = false;
            }
        }
    }
}
