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

namespace Reproductor
{
    public partial class Inicio : Form
    {
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
        }

        private void LeerJsonL()
        {
            List<CL_Biblioteca> A = new List<CL_Biblioteca>();
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
            dataGridView2.DataSource = A;///Listas_Publicas.Biblioteca;
            dataGridView2.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cadena = dataGridView2.CurrentRow.Cells[2].Value.ToString();
            WR.URL = cadena;
            WR.Ctlcontrols.play();
            //que putas
        }
    }
}
