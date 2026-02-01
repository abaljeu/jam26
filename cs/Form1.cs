
using System.Drawing.Drawing2D;
using System.Media;

namespace mask
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            GameState.theGame.InitGame();

            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            SoundPlayer sound = new SoundPlayer("../../../../background_placeholder_1.wav");
            sound.Play();
        }
    }
}
