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
        private Teensy _teensy;
        private Logic _logic;
        public Form1()
        {
            InitializeComponent();
            _teensy = new Teensy("COM3");
            _logic = new Logic(_teensy);
            captureThread = new Thread(new ParameterizedThreadStart(cv.StartCapturing));
            captureThread.Name = "captureThread";
            captureThread.Start(_logic);
            liveViewTimer = new Timer();
            liveViewTimer.Interval = 20;
            liveViewTimer.Tick += new EventHandler(liveViewTimer_Tick);
            liveViewTimer.Start();
        }

        void liveViewTimer_Tick(object sender, EventArgs e)
        {
            if (cv.globFrameCount >= 1)
            {
                //i have no idea whats wrong so lets just ignore the problem for now
                try
                {
                    liveViewPictureBox.Image = cv.GetBigPicture().Bitmap;
                }
                catch (Exception)
                {
                }

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
            _teensy.WriteAllowed = !_teensy.WriteAllowed;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cv.Flagged = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ReplayWindow replayWindow = new ReplayWindow();
            replayWindow.Show();
        }
    }
}
