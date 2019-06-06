using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;
using Newtonsoft.Json;
using MetroFramework;

namespace Reproductor
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {

        List<CL_Biblioteca> Biblio = new List<CL_Biblioteca>();
        int i;
        int progres;
        string direccionPortada;
        string direccionLetra;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LeerJson();
            i = Convert.ToInt16(Biblio.OrderByDescending(l => l.Codigo).ElementAt(0).Codigo.ToString());
            i = i + 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                if (metroTextBox1.Text!=null)
                {

                    metroLabel1.Text = "Iniciando...";
                    progres += 10;
                    metroProgressBar1.Value = progres;
                    portada();
                    letra();
                    MainAsync();

                }
                else
                {
                    MessageBox.Show("ingrese Link del Video");
                }

            }
            else
            {
                MessageBox.Show("ingrese la letra");
            }
        }

        private async Task MainAsync()
        {
            CL_Biblioteca biblioteca = new CL_Biblioteca();
            biblioteca.Codigo = i;
            biblioteca.Portada = direccionPortada;
            //Nuevo Cliente de YouTube
            var client = new YoutubeClient();
            //Lee la URL de youtube que le escribimos en el textbox.
            var videoId = NormalizeVideoId(metroTextBox1.Text);
            progres += 10;
            metroProgressBar1.Value = progres;
            var video = await client.GetVideoAsync(videoId);
            var streamInfoSet = await client.GetVideoMediaStreamInfosAsync(videoId);
            //Busca la mejor resolución en la que está disponible el video.
            var streamInfo = streamInfoSet.Muxed.WithHighestVideoQuality();
            //Compone el nombre que tendrá el video en base a su título y extensión.
            var fileExtension = streamInfo.Container.GetFileExtension();
            var fileName = $"{video.Title}.{fileExtension}";
            //TODO: Reemplazar los caractéres ilegales del nombre
            //fileName = RemoveIllegalFileNameChars(fileName);
            //Activa el timer para que el proceso funcione de forma asincrona
            //tmrVideo.Enabled = true;
            // mensajes indicando que el video se está descargando
            progres += 10;
            metroProgressBar1.Value = progres;
            metroLabel1.Text = "Descargando el video...";
            //TODO: se pude usar una barra de progreso para ver el avance
            //using (var progress = new ProgressBar());
            //Empieza la descarga.
            progres += 10;
            metroProgressBar1.Value = progres;
            await client.DownloadMediaStreamAsync(streamInfo, fileName);
            //Ya descargado se inicia la conversión a MP3
            var Convert = new NReco.VideoConverter.FFMpegConverter();
            //Especificar la carpeta donde se van a guardar los archivos, recordar la \ del final
            String SaveMP3File = @"C:\Users\Patrick\Documents\Visual Studio 2015\Projects\Reproductor\MP3\" + fileName.Replace(".mp4", ".mp3");
            biblioteca.Direccion = SaveMP3File;
            biblioteca.Nombre = fileName;
            biblioteca.Letra =direccionLetra;
            progres += 10;
            metroProgressBar1.Value = progres;
            //Guarda el archivo convertido en la ubicación indicada
            Convert.ConvertMedia(fileName, SaveMP3File, "mp3");
            //Si el checkbox de solo audio está chequeado, borrar el mp4 despues de la conversión
            progres += 10;
            metroProgressBar1.Value = progres;
            File.Delete(fileName);
            progres += 40;
            metroProgressBar1.Value = progres;
            //Indicar que se terminó la conversion
            GuardarJson(biblioteca);
            MessageBox.Show("Vídeo convertido correctamente.");
            metroLabel1.Text = "";
            metroLabel1.Text = "";
            progres = 0;
            metroProgressBar1.Value = progres;
            return;
        }

        private static string NormalizeVideoId(string input)
        {
            string videoId = string.Empty;
            return YoutubeClient.TryParseVideoId(input, out videoId)
                ? videoId
                : input;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Inicio frm = new Inicio();
            frm.ShowDialog();
        }

        private void GuardarJson(CL_Biblioteca p)
        {
            string salida = JsonConvert.SerializeObject(p);
            FileStream stream = new FileStream("Biblioteca.json", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(salida);
            writer.Close();
            //limpiar la lista de autores

            MessageBox.Show("Ingresado Exitosamente!!");

        }
        private void LeerJson()
        {
            FileStream stream = new FileStream("Biblioteca.json", FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            while (reader.Peek() > -1)
            {
                string lectura = reader.ReadLine();
                CL_Biblioteca libroLeido = JsonConvert.DeserializeObject<CL_Biblioteca>(lectura);
                Biblio.Add(libroLeido);
            }
            reader.Close();
            //dataGridView2.DataSource = Agregar;
            //dataGridView2.Refresh();
            //Libro lib = Agregar.OrderBy(al => al.Anio1).First();
            //textBox5.Text = lib.Anio1.ToString();
        }

        private void portada()
        {
            string nombrearchivo;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image f = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = f;
                nombrearchivo = openFileDialog1.FileName.ToString();
                direccionPortada = "C:\\Users\\Patrick\\Documents\\Visual Studio 2015\\Projects\\Reproductor\\PORTADA\\" + i + ".png";
                f.Save("C:\\Users\\Patrick\\Documents\\Visual Studio 2015\\Projects\\Reproductor\\PORTADA\\" + i + ".png");

            }
        }
        private void letra()
        {
            direccionLetra = "C:\\Users\\Patrick\\Documents\\Visual Studio 2015\\Projects\\Reproductor\\PORTADA\\" + i + ".txt";
            FileStream stream = new FileStream(direccionLetra, FileMode.Append, FileAccess.Write);
            StreamWriter write = new StreamWriter(stream);
            write.WriteLine(textBox1.Text);
            write.Close();
        }


    }
}
