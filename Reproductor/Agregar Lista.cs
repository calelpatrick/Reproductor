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
using System.IO;



namespace Reproductor
{
    public partial class Agregar_Lista : MetroFramework.Forms.MetroForm
    {
       

        public Agregar_Lista()
        {
            InitializeComponent();
        }

        public string codigo;
        public string nombre;
        public string direccion;
        public string portada;
        public string letra;
        string nombreLista;
        bool ingresarAfuera;

        List<CL_Reproduccion> Playlist = new List<CL_Reproduccion>();
        List<CL_Biblioteca> Biblioteca = new List<CL_Biblioteca>();
        private void Agregar_Lista_Load(object sender, EventArgs e)
        {
            ingresarAfuera = false;
            LeerJsonL();
            LeerJsonListas();
            CargarCombo();
/*            metroComboBox1.ValueMember = "Direccion1";
            metroComboBox1.DisplayMember= "Nombre1";
            metroComboBox1.DataSource = Playlist;
            metroComboBox1.SelectedIndexChanged += new System.EventHandler(this.metroComboBox1_SelectedIndexChanged);*/
            // dataGridView1.DataSource = Biblioteca;
            // dataGridView1.Refresh();
            
        }

        private void CargarCombo()
        {
            comboBox1.ValueMember = "Direccion1";
            comboBox1.DisplayMember = "Nombre1";
            comboBox1.DataSource = Playlist;
            comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged_1);
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
                Biblioteca.Add(libroLeido);
            }
            reader.Close();
            //Mostrar la lista de libros en el gridview
            dataGridView1.DataSource = Biblioteca;///Listas_Publicas.Biblioteca;
            dataGridView1.Refresh();
        }


        private void GuardarListas(CL_Reproduccion biblioteca)
        {
            string salida = JsonConvert.SerializeObject(biblioteca);
            FileStream stream = new FileStream("Listas.json", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(salida);
            writer.Close();
            MessageBox.Show("Guardado Exitosamente");
        }
        private void GuardarCancionesdeLista(CL_Biblioteca biblioteca, string direccion)
        {

            if (comboBox1.SelectedItem == null)
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
        private void LeerJsonListas()
        {
            FileStream stream = new FileStream("Listas.json", FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            while (reader.Peek() > -1)
            {
                string lectura = reader.ReadLine();
                CL_Reproduccion libroLeido = JsonConvert.DeserializeObject<CL_Reproduccion>(lectura);
                Playlist.Add(libroLeido);
            }
            reader.Close();
            dataGridView2.DataSource = Playlist;
            dataGridView2.Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
            if (ingresarAfuera==false)
            {
                CL_Biblioteca CANCION = new CL_Biblioteca();
                CANCION.Codigo = Convert.ToInt16(codigo);
                CANCION.Nombre = nombre;
                CANCION.Direccion = direccion;
                CANCION.Portada = portada;
                CANCION.Letra = letra;
                string path = comboBox1.SelectedValue.ToString();
                GuardarCancionesdeLista(CANCION, path);
                ingresarAfuera = true;
            }
            else
            {
                CL_Biblioteca CANCION = new CL_Biblioteca();
                CANCION.Codigo = Convert.ToInt16(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                CANCION.Nombre = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                CANCION.Direccion = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                CANCION.Portada = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                CANCION.Letra = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                string path = comboBox1.SelectedValue.ToString();
                GuardarCancionesdeLista(CANCION, path);
            }


        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           /* CL_Reproduccion seleccion = metroComboBox1.SelectedItem as CL_Reproduccion;

            if (seleccion == null)
                return;
                */

        }
        

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            CL_Reproduccion seleccion2 = comboBox1.SelectedItem as CL_Reproduccion;

            if (seleccion2 == null)
                return;
        }

        
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            File.Delete("C:\\Users\\Patrick\\Documents\\Visual Studio 2015\\Projects\\Reproductor\\Reproductor\\bin\\Debug\\Listas.json");
            string codigo = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            Playlist.RemoveAll(l => l.Nombre1 == codigo);
            dataGridView2.DataSource = null;
            for (int i = 0; i < Playlist.Count; i++)
            {
                GuardarListas(Playlist[i]);
            }
            
            dataGridView2.DataSource = Playlist;
            dataGridView2.Refresh();
            Playlist.Clear();
            LeerJsonListas();
            comboBox1.DataSource = null;
            CargarCombo();
            comboBox1.Refresh();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            CL_Reproduccion listas = new CL_Reproduccion();
            listas.Nombre1 = metroTextBox1.Text;
            listas.Direccion1 = "C:\\Users\\Patrick\\Documents\\Visual Studio 2015\\Projects\\Reproductor\\Listas\\" + metroTextBox1.Text + ".json";
            GuardarListas(listas);
            Playlist.Clear();
            LeerJsonListas();
            comboBox1.DataSource = null;
            CargarCombo();
            comboBox1.Refresh();
            metroTextBox1.Text = "";
        }
    }
    }

