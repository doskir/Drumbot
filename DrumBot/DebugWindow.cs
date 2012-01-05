using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace DrumBot
{
    public partial class DebugWindow : Form
    {
        public DebugWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Image<Bgr, Byte> screenshot = new Image<Bgr, byte>("screenshot.bmp");
            ImageProcessing processing = new ImageProcessing();
            Image<Bgr, byte> greenTrack;
            processing.ProcessImage(screenshot, out greenTrack);
            pictureBox1.Image = greenTrack.Bitmap;
        }
    }
}
