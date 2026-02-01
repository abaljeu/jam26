
using System.Drawing.Drawing2D;
using System.Media;

namespace mask
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            WindowState = FormWindowState.Maximized;

                string dir = Directory.GetCurrentDirectory();
                string audioPath = dir;

#if DEBUG
                audioPath = dir + "\\..\\..\\..\\..";
#endif
                audioPath += "\\audio\\background_placeholder_1.wav";

                SoundPlayer sound = new SoundPlayer(audioPath);
                sound.Play();

        }
    }
}