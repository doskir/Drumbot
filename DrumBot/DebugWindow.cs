using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            ReloadTracks();
            RefreshPictureBoxes();
        }
        public void RefreshPictureBoxes()
        {
            redTrack1PictureBox.Image = firstImage.RedTrack.Bitmap;
            yellowTrack1PictureBox.Image = firstImage.YellowTrack.Bitmap;
            blueTrack1PictureBox.Image = firstImage.BlueTrack.Bitmap;
            greenTrack1PictureBox.Image = firstImage.GreenTrack.Bitmap;
            redTrack2PictureBox.Image = secondImage.RedTrack.Bitmap;
            yellowTrack2PictureBox.Image = secondImage.YellowTrack.Bitmap;
            blueTrack2PictureBox.Image = secondImage.BlueTrack.Bitmap;
            greenTrack2PictureBox.Image = secondImage.GreenTrack.Bitmap;
        }


        private CapturedImage firstImage;
        private CapturedImage secondImage;
        private void button1_Click(object sender, EventArgs e)
        {
            ReloadTracks();
            RefreshPictureBoxes();
        }
        private void ReloadTracks()
        {
            ImageProcessing processing = new ImageProcessing();
            if (firstImage != null)
                firstImage.Dispose();
            if (secondImage != null)
                secondImage.Dispose();

            firstImage = new CapturedImage(new Image<Bgr, byte>("screenshot.bmp"), DateTime.Now);
            secondImage = new CapturedImage(new Image<Bgr, byte>("screenshot2.bmp"),
                                            firstImage.CaptureTime.AddMilliseconds(40));

        }
        private void ApplyThresholding()
        {
            int redThreshold = (int)thresholdRedNumericUpDown.Value;
            int greenThreshold = (int)thresholdGreenNumericUpDown.Value;
            int blueThreshold = (int) thresholdRedNumericUpDown.Value;
            int redMaximum = (int)maximumRedNumericUpDown.Value;
            int greenMaximum = (int)maximumGreenNumericUpDown.Value;
            int blueMaximum = (int)maximumRedNumericUpDown.Value;
            //filter out the background
            firstImage.RedTrack = firstImage.RedTrack.ThresholdToZero(new Bgr(blueThreshold, greenThreshold, redThreshold));
            //track = track.ThresholdTrunc(new Bgr(blueMaximum, greenMaximum, redMaximum));
            //track = track.ThresholdToZero(new Bgr(blueThreshold, greenThreshold, redThreshold));
            //track = track.ThresholdBinary(new Bgr(blueThreshold, greenThreshold, redThreshold),
                                                    //new Bgr(blueMaximum, greenMaximum, redMaximum));
            secondImage.RedTrack = secondImage.RedTrack.ThresholdTrunc(new Bgr(blueMaximum, greenMaximum, redMaximum));
            secondImage.RedTrack = secondImage.RedTrack.ThresholdToZero(new Bgr(blueThreshold, greenThreshold, redThreshold));

            //track2 = track2.ThresholdBinary(new Bgr(blueThreshold, greenThreshold, redThreshold),
                                       //new Bgr(blueMaximum, greenMaximum, redMaximum));
            //RGB
            //255,90,255
            //0,255,0
        }
        private Image<Gray,byte> ExtractAndDrawFeatures(Image<Bgr,byte> track)
        {
            DateTime startTime = DateTime.Now;
            List<Rectangle> rectangles = new List<Rectangle>();

            Image<Emgu.CV.Structure.Gray, byte> grayScaleImage = new Image<Gray, byte>(track.Bitmap);
            var cannyEdges = grayScaleImage.Canny(new Gray(200), new Gray(20));
            Image<Emgu.CV.Structure.Gray, byte> outputImage = new Image<Gray, byte>(grayScaleImage.Width,
                                                                                    grayScaleImage.Height);
            for (Contour<Point> contours = 
                cannyEdges.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL);
                contours != null; contours = contours.HNext)
            {
                Rectangle newRectangle = contours.GetMinAreaRect().MinAreaRect();
                bool intersectionFound = false;
                for (int i = 0; i < rectangles.Count; i++)
                {
                    Rectangle rectangle = rectangles[i];
                    if (newRectangle.IntersectsWith(rectangle) ||newRectangle.Contains(rectangle))
                    {
                        rectangle.Intersect(newRectangle);
                        intersectionFound = true;
                        break;
                    }
                }
                if (newRectangle.Width < 30 || newRectangle.Height < 5)
                    continue;
                if (!intersectionFound)
                    rectangles.Add(newRectangle);
            }
            foreach(Rectangle rectangle in rectangles)
            {
                outputImage.Draw(rectangle, new Gray(255), 1);
            }
            TimeSpan elapsedTime = DateTime.Now - startTime;
            Debug.WriteLine("Contour: {0} ms.", elapsedTime.TotalMilliseconds);
            return outputImage;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //this code here causes a memory overflow, remember to always dispose bitmaps
            ReloadTracks();
            RefreshPictureBoxes();
            /*
            redTrack1PictureBox.Image = ExtractAndDrawFeatures(redTrack1).Bitmap;
            yellowTrack1PictureBox.Image = ExtractAndDrawFeatures(yellowTrack1).Bitmap;
            blueTrack1PictureBox.Image = ExtractAndDrawFeatures(blueTrack1).Bitmap;
            greenTrack1PictureBox.Image = ExtractAndDrawFeatures(greenTrack1).Bitmap;
            redTrack2PictureBox.Image = ExtractAndDrawFeatures(redTrack2).Bitmap;
            yellowTrack2PictureBox.Image = ExtractAndDrawFeatures(yellowTrack2).Bitmap;
            blueTrack2PictureBox.Image = ExtractAndDrawFeatures(blueTrack2).Bitmap;
            greenTrack2PictureBox.Image = ExtractAndDrawFeatures(greenTrack2).Bitmap;
            */
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs) e;
            if (me.X < redTrack1PictureBox.Image.Width && me.Y < redTrack1PictureBox.Image.Height)
            {
                Color pixel = firstImage.RedTrack.Bitmap.GetPixel(me.X, me.Y);
                Debug.WriteLine("{0},{1}", me.X, me.Y);
                Debug.WriteLine("{0};{1};{2}", pixel.R, pixel.G, pixel.B);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.X < redTrack2PictureBox.Image.Width && me.Y < redTrack2PictureBox.Image.Height)
            {
                Color pixel = secondImage.RedTrack.Bitmap.GetPixel(me.X, me.Y);
                Debug.WriteLine("{0},{1}", me.X, me.Y);
                Debug.WriteLine("{0};{1};{2}", pixel.R, pixel.G, pixel.B);
            }
        }
    }
}
