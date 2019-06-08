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

        List<CL_Reproduccion> Playlist = new List<CL_Reproduccion>();
        List<CL_Biblioteca> Biblioteca = new List<CL_Biblioteca>();
        private void Agregar_Lista_Load(object sender, EventArgs e)
        {
            LeerJsonL();
            LeerJsonListas();
            metroComboBox1.ValueMember = "Direccion1";
            metroComboBox1.DisplayMember= "Nombre1";
            metroComboBox1.DataSource = Playlist;
            metroComboBox1.SelectedIndexChanged += new System.EventHandler(this.metroComboBox1_SelectedIndexChanged);
           // dataGridView1.DataSource = Biblioteca;
           // dataGridView1.Refresh();
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
        private void button1_Click(object sender, EventArgs e)
        {

            CL_Reproduccion listas = new CL_Reproduccion();
            listas.Nombre1 = metroTextBox1.Text;
            listas.Direccion1= "C:\\Users\\Patrick\\Documents\\Visual Studio 2015\\Projects\\Reproductor\\Listas\\" + metroTextBox1.Text + ".json";
            GuardarListas(listas);

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

            if (metroComboBox1.SelectedItem == null)
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
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CL_Reproduccion seleccion = metroComboBox1.SelectedItem as CL_Reproduccion;

            if (seleccion == null)
                return;


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }
}
