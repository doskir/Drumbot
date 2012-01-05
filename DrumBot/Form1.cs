using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Timer = System.Windows.Forms.Timer;

namespace DrumBot
{
    public partial class Form1 : Form
    {
        private CaptureVideo cv = new CaptureVideo();
        private Thread captureThread;
        private Timer liveViewTimer;
        public Form1()
        {
            InitializeComponent();
            captureThread = new Thread(new ThreadStart(cv.StartCapturing));
            captureThread.Name = "captureThread";
            captureThread.Start();
            liveViewTimer = new Timer();
            liveViewTimer.Interval = 10;
            liveViewTimer.Tick += new EventHandler(liveViewTimer_Tick);
            liveViewTimer.Start();
        }

        void liveViewTimer_Tick(object sender, EventArgs e)
        {
            if (cv.globFrameCount >= 1)
            {
                liveViewPictureBox.Image = cv.MostRecentImage.Bitmap;
            }
            TimeSpan elapsedTime = DateTime.Now - cv.globCaptureStart;
            double fps = (cv.globFrameCount / elapsedTime.TotalMilliseconds) * 1000;
            label1.Text = fps.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cv.Recording = !cv.Recording;
            if(cv.Recording)
            {
                button1.Text = "Stop Recording";
            }
            else
            {
                button1.Text = "Start Recording";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
