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
        List<CL_Biblioteca> A = new List<CL_Biblioteca>();//Biblioteca principal
        List<CL_Biblioteca> PLaylist = new List<CL_Biblioteca>();// listas de reproduccion
        List<CL_Reproduccion> listasC = new List<CL_Reproduccion>();// Listas de reproduccion ComboBox
        bool pausa = false;
        bool termino = false;

        bool bilioteca = true;
        string direcListaactual;
        public Inicio()
        {
            InitializeComponent();
            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);

            // Enable timer.  
            timer1.Enabled = true;
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            LeerJsonL();
            LeerJsonListas();
            WR.uiMode = "none";
        }
           
        private void LeerJsonL()//Lee la biblioteca por primera vez
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

        private void button1_Click_1(object sender, EventArgs e)//Botton de PLay
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
                label2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                WR.Ctlcontrols.play();
            }

        }
        private void leerLetra(string path)//Lee la letra de la direccion espesificada
        {
            textBox1.Text = File.ReadAllText(path);
        }

        private void button4_Click(object sender, EventArgs e)//Botton de Stop
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

        private void pictureBox2_Click(object sender, EventArgs e)// Picture de Youtube
        {
            Form1 frm = new Form1();
            frm.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e)//Actualiza la Biblioteca
        {
            A.Clear();
            listasC.Clear();
            dataGridView1.DataSource = null;
            comboBox1.DataSource = null;
            LeerJsonL();
            LeerJsonListas();
            CargarCombo();
            // LeerJsonL();
        }

        private void button5_Click(object sender, EventArgs e)//Botton Crear listas nuevas y actualizar
        {
            string codigo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string nombre = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            string direccion = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            string portada = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            string letra = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            // string nombre = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            Agregar_Lista frm = new Agregar_Lista();
            frm.codigo = codigo;
            frm.nombre = nombre;
            frm.direccion = direccion;
            frm.portada = portada;
            frm.letra = letra;
            frm.ShowDialog();
        }

        private void pictureBox4_Click(object sender, EventArgs e)// Picture mis playlist
        {
            /*Mis_Listas frm = new Mis_Listas();
           WR.Ctlcontrols.stop();
           // this.Hide();
            frm.ShowDialog();
           // this.Close();
           */
            bilioteca = false;
            checkBox1.Enabled = false;
            comboBox1.Enabled = true;
            CargarCombo();
               
        }

        private void Inicio_FormClosed(object sender, FormClosedEventArgs e)// Evento Cerrar formulario principal
        {
            //WR.Ctlcontrols.stop();
           // Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)// Botton Siguiente
        {
            if (checkBox2.Checked)
            {
                try
                {
                    int filaActual = dataGridView1.CurrentRow.Index;
                    dataGridView1.CurrentCell = dataGridView1.Rows[CancionAleatorio()].Cells[0];
                    string ruta = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    leerLetra(dataGridView1.CurrentRow.Cells[4].Value.ToString());
                    WR.URL = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    WR.Ctlcontrols.play();
                    Image f = Image.FromFile(ruta);
                    pictureBox1.Image = f;
                    label2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                }
                catch
                {
                    MessageBox.Show("Esta es la ultima cancion de la lista.");

                }
            }
            else
            {
                 try
                {
                    int filaActual = dataGridView1.CurrentRow.Index;
                    dataGridView1.CurrentCell = dataGridView1.Rows[filaActual + 1].Cells[0];
                    string ruta = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    leerLetra(dataGridView1.CurrentRow.Cells[4].Value.ToString());
                    WR.URL = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    WR.Ctlcontrols.play();
                    Image f = Image.FromFile(ruta);
                    pictureBox1.Image = f;
                    label2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                }
                catch
                {
                    MessageBox.Show("Esta es la ultima cancion de la lista.");
               
                }
           }
        }

        private void button2_Click(object sender, EventArgs e) //Botton anterior
        {
            try
            {
                label2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                int filaActual = dataGridView1.CurrentRow.Index;
                dataGridView1.CurrentCell = dataGridView1.Rows[filaActual - 1].Cells[0];
                string ruta = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                leerLetra(dataGridView1.CurrentRow.Cells[4].Value.ToString());
                WR.URL = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                WR.Ctlcontrols.play();
                Image f = Image.FromFile(ruta);
                pictureBox1.Image = f;
            }
            catch
            {
                MessageBox.Show("Esta es la primera cancion");
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e) // picture Botton Eliminar
        {
            if (bilioteca==true)
            {
                if (checkBox1.Checked)
                {
                    DialogResult result = MessageBox.Show("Seguro desea eliminar todos los archivos?", "Eliminar Cancion", MessageBoxButtons.YesNoCancel);

                    if (result == DialogResult.Yes)
                    {
                        File.Delete(@"C:\Users\Patrick\Documents\Visual Studio 2015\Projects\Reproductor\Reproductor\bin\Debug\Biblioteca.json");
                        File.Delete(dataGridView1.CurrentRow.Cells[2].Value.ToString());
                        File.Delete(dataGridView1.CurrentRow.Cells[3].Value.ToString());
                        File.Delete(dataGridView1.CurrentRow.Cells[4].Value.ToString());
                        string codigo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        A.RemoveAll(l => l.Codigo == Convert.ToInt16(codigo));
                        for (int i = 0; i < A.Count; i++)
                        {
                            GuardarBiblioteca(A[i]);
                        }
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = A;
                        dataGridView1.Refresh();
                        checkBox1.Checked = false;
                        MessageBox.Show("Se eliminaron todos los Archivos");
                    }

                }
                else
                {
                    DialogResult result = MessageBox.Show("Desea eliminar la cancion de su Biblioteca?", "Eliminar Cancion", MessageBoxButtons.YesNoCancel);

                    if (result == DialogResult.Yes)
                    {
                        File.Delete(@"C:\Users\Patrick\Documents\Visual Studio 2015\Projects\Reproductor\Reproductor\bin\Debug\Biblioteca.json");
                        //File.Delete(dataGridView1.CurrentRow.Cells[2].Value.ToString());
                        string codigo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        A.RemoveAll(l => l.Codigo == Convert.ToInt16(codigo));
                        for (int i = 0; i < A.Count; i++)
                        {
                            GuardarBiblioteca(A[i]);
                        }
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = A;
                        dataGridView1.Refresh();
                        MessageBox.Show("Eliminado");
                    }
                }

            }
            else
            {
                DialogResult result = MessageBox.Show("Desea eliminar la cancion de su lista?", "Eliminar Cancion", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    string direcc = comboBox1.SelectedValue.ToString();
                    File.Delete(comboBox1.SelectedValue.ToString());
                    //File.Delete(dataGridView1.CurrentRow.Cells[2].Value.ToString());
                    string codigo2 = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    PLaylist.RemoveAll(l => l.Codigo == Convert.ToInt16(codigo2));
                    dataGridView1.DataSource = null;
                    for (int i = 0; i < PLaylist.Count; i++)
                    {
                        GuardarCancionesdeLista(PLaylist[i],direcc);
                    }
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = PLaylist;
                    dataGridView1.Refresh();
                    MessageBox.Show("Eliminado");
                }
            }
        }
        private void GuardarCancionesdeLista(CL_Biblioteca biblioteca, string direccion)// Actualizar Lista
        {

            if (direccion == "")
            {
                MessageBox.Show("Por favor seleccione un nombre de la lista");
            }
            else
            {

                string salida = JsonConvert.SerializeObject(biblioteca);
                FileStream stream = new FileStream(direccion, FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(salida);
                writer.Close();
                MessageBox.Show("Agregado exitosamente");
            }

        }
        private void GuardarBiblioteca(CL_Biblioteca biblioteca) // Guardar Biblioteca
        {
            string salida = JsonConvert.SerializeObject(biblioteca);
            FileStream stream = new FileStream("Biblioteca.json", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(salida);
            writer.Close();
        }

        private void LeerJsonCanciones(string path)// Lee las conciones de una lista con su direccion
        {
            
            PLaylist.Clear();
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            while (reader.Peek() > -1)
            {
                string lectura = reader.ReadLine();
                CL_Biblioteca libroLeido = JsonConvert.DeserializeObject<CL_Biblioteca>(lectura);
                PLaylist.Add(libroLeido);
            }
            reader.Close();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = PLaylist;
            dataGridView1.Refresh();
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)// funcion cambio de elemento seleccionado en el combo box1
        {
            CL_Reproduccion seleccion = comboBox1.SelectedItem as CL_Reproduccion;

            if (seleccion == null)
                return;

            LeerJsonCanciones(seleccion.Direccion1);
           

        }

        private void CargarCombo()// CArga el ComboBox
        {
            comboBox1.ValueMember = "Direccion1";
            comboBox1.DisplayMember = "Nombre1";
            comboBox1.DataSource = listasC;
            comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
        }
        private void LeerJsonListas()// Lee las listas para el comboBox
        {
            FileStream stream = new FileStream("Listas.json", FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            while (reader.Peek() > -1)
            {
                string lectura = reader.ReadLine();
                CL_Reproduccion libroLeido = JsonConvert.DeserializeObject<CL_Reproduccion>(lectura);
                listasC.Add(libroLeido);
            }
            reader.Close();
        }
//        player.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(player_PlayStateChange);

        private void WR_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            //WR.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(WR_PlayStateChange);
            if (e.newState == 8)
            {
                termino = true;
                
                // WR.URL = ();
                // WR.Ctlcontrols.play();
            } 
            
        }

        private int CancionAleatorio()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, A.Count);
            return randomNumber;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                if (termino == true)
                {
                    WR.Ctlcontrols.stop();
                    dataGridView1.CurrentCell = dataGridView1.Rows[CancionAleatorio()].Cells[0];
                    string ruta = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    WR.URL = dataGridView1.Rows[CancionAleatorio()].Cells[2].Value.ToString();
                    WR.Ctlcontrols.play();
                    Image f = Image.FromFile(ruta);
                    pictureBox1.Image = f;
                    label2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    leerLetra(dataGridView1.CurrentRow.Cells[4].Value.ToString());
                    termino = false;
                }
            }
            else
            {
                if (termino==true)
                 {
                        int filaActual = dataGridView1.CurrentRow.Index;
                        dataGridView1.CurrentCell = dataGridView1.Rows[filaActual + 1].Cells[0];
                        string ruta = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                        leerLetra(dataGridView1.CurrentRow.Cells[4].Value.ToString());
                        WR.URL = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                        WR.Ctlcontrols.play();
                        Image f = Image.FromFile(ruta);
                        pictureBox1.Image = f;
                        label2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
           
                }
            }
        }
    }
}
