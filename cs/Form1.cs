
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
            Music.startIntroMusic();
        }
    }
}