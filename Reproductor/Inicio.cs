using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.IO;
using MetroFramework;

namespace Reproductor
{
    public partial class Inicio : MetroFramework.Forms.MetroForm
    {
        List<CL_Biblioteca> A = new List<CL_Biblioteca>();
        bool pausa = false;
        public Inicio()
        {
            InitializeComponent();
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            LeerJsonL();
            WR.uiMode = "none";
        }

        private void LeerJsonL()
        {
            //List<CL_Biblioteca> A = new List<CL_Biblioteca>();
            FileStream stream = new FileStream("Biblioteca.json", FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            while (reader.Peek() > -1)
            {
                string lectura = reader.ReadLine();
                CL_Biblioteca libroLeido = JsonConvert.DeserializeObject<CL_Biblioteca>(lectura);
                A.Add(libroLeido);
            }
            reader.Close();
            //Mostrar la lista de libros en el gridview
            dataGridView1.DataSource = A;///Listas_Publicas.Biblioteca;
            dataGridView1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          

            
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            double time = WR.Ctlcontrols.currentPosition;
            
            if (time > 0 && pausa == true)
            {
                WR.Ctlcontrols.currentPosition = time;
                WR.Ctlcontrols.play();
                pausa = false;
            }
            else
            {

                string ruta = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                string cancion = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                leerLetra(dataGridView1.CurrentRow.Cells[4].Value.ToString());
                WR.URL = cancion;
                Image f = Image.FromFile(ruta);
                pictureBox1.Image = f;
                WR.Ctlcontrols.play();
            }

        }
        private void leerLetra(string path)
        {
            textBox1.Text = File.ReadAllText(path);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //WR.Ctlcontrols.stop();
            if (pausa==true)
            {
                WR.Ctlcontrols.stop();
                pausa = false;
                string img = "C:\\Users\\Patrick\\Documents\\Visual Studio 2015\\Projects\\Reproductor\\Icon\\stop.png";
                button4.Image = Image.FromFile(img);
            }
            else { 
            pausa = true;
                string img = "C:\\Users\\Patrick\\Documents\\Visual Studio 2015\\Projects\\Reproductor\\Icon\\pause.png";
                button4.Image = Image.FromFile(img);
                WR.Ctlcontrols.pause();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            A.Clear();
            LeerJsonL();
        }
    }
}
