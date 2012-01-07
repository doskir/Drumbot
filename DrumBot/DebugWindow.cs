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
        private Logic logic = new Logic();
        private void button1_Click(object sender, EventArgs e)
        {
            ReloadTracks();
            RefreshPictureBoxes();
            imageId = 477;
        }
        private void ReloadTracks()
        {
            logic.CurrentNotes.Clear();
            ImageProcessing processing = new ImageProcessing();
            if (firstImage != null)
                firstImage.Dispose();
            if (secondImage != null)
                secondImage.Dispose();

            firstImage = new CapturedImage(new Image<Bgr, byte>(@"D:\drumbot\panic attack partial\" + "000475.bmp"), DateTime.Now);
            secondImage = new CapturedImage(new Image<Bgr, byte>(@"D:\drumbot\panic attack partial\" + "000476.bmp"),
                                firstImage.CaptureTime.AddMilliseconds(40));
            MostRecentImage = firstImage;
            logic.UpdateAndPredictNotes(MostRecentImage);
            MostRecentImage = secondImage;
            logic.UpdateAndPredictNotes(MostRecentImage);
            DrawBigPicture();





        }

        public CapturedImage MostRecentImage;
        private Image<Bgr, byte> CurrentDisplayedImage;
        private void DrawBigPicture()
        {
            MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 1, 1);
            font.thickness = 2;
            Image<Bgr, byte> newImage = MostRecentImage.Image.Clone();
            foreach (Note note in logic.CurrentNotes.Where(n=>n.DetectedInFrames > 1))
            {
                Point playAreaOffset = new Point(218, 262);
                Point trackOffset;
                switch (note.TrackColor)
                {
                    case NoteType.Red:
                        trackOffset = new Point(0, 0);
                        break;
                    case NoteType.Yellow:
                        trackOffset = new Point(66, 0);
                        break;
                    case NoteType.Blue:
                        trackOffset = new Point(134, 0);
                        break;
                    case NoteType.Green:
                        trackOffset = new Point(176, 0);
                        break;
                    default:
                        trackOffset = new Point(0, 0);
                        break;
                }
                Rectangle drawRectangle = new Rectangle(playAreaOffset.X + trackOffset.X + note.Rectangle.X,
                                                        playAreaOffset.Y + trackOffset.Y + note.Rectangle.Y,
                                                        note.Rectangle.Width, note.Rectangle.Height);
                Bgr color = new Bgr(0, 0, 0);
                switch (note.Color)
                {
                    case NoteType.Red:
                        color = new Bgr(0, 0, 255);
                        break;
                    case NoteType.Yellow:
                        color = new Bgr(0, 255, 255);
                        break;
                    case NoteType.Blue:
                        color = new Bgr(255, 0, 0);
                        break;
                    case NoteType.Green:
                        color = new Bgr(0, 255, 0);
                        break;
                    case NoteType.Orange:
                        color = new Bgr(255, 255, 255);
                        break;
                }
                newImage.Draw(drawRectangle, color, 2);
                newImage.Draw(note.FramesUntilHit.ToString("##.##"), ref font,
                              new Point(drawRectangle.Left, drawRectangle.Bottom), new Bgr(0,0,0));
                //newImage.Draw("X" + note.PerFrameVelocityX + ";Y" + note.PerFrameVelocityY, ref font,
                //              new Point(drawRectangle.Left, drawRectangle.Bottom), new Bgr(0,0,0));
                //newImage.Draw(((int) note.DistanceToTarget).ToString(), ref font,
                //              new Point(drawRectangle.Left, drawRectangle.Bottom), new Bgr(128, 128, 128));
            }

            if (CurrentDisplayedImage != null)
                CurrentDisplayedImage.Dispose();
            CurrentDisplayedImage = newImage;
            pictureBox1.Image = CurrentDisplayedImage.Bitmap;
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

        private void redTrack1PictureBox_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs) e;
            if (me.X < redTrack1PictureBox.Image.Width && me.Y < redTrack1PictureBox.Image.Height)
            {
                Color pixel = firstImage.RedTrack.Bitmap.GetPixel(me.X, me.Y);
                Debug.WriteLine("{0},{1}", me.X, me.Y);
                Debug.WriteLine("{0};{1};{2}", pixel.R, pixel.G, pixel.B);
            }
        }

        private void redTrack2PictureBox_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.X < redTrack2PictureBox.Image.Width && me.Y < redTrack2PictureBox.Image.Height)
            {
                Color pixel = secondImage.RedTrack.Bitmap.GetPixel(me.X, me.Y);
                Debug.WriteLine("{0},{1}", me.X, me.Y);
                Debug.WriteLine("{0};{1};{2}", pixel.R, pixel.G, pixel.B);
            }
        }

        private int imageId = 477;
        private void button2_Click_1(object sender, EventArgs e)
        {
            secondImage = new CapturedImage(new Image<Bgr, byte>(@"D:\drumbot\panic attack partial\" + imageId.ToString().PadLeft(6, '0') + ".bmp"), DateTime.Now);
            MostRecentImage = secondImage;
            logic.UpdateAndPredictNotes(MostRecentImage);
            DrawBigPicture();
            RefreshPictureBoxes();
            //just to get the debug console output
            //foreach(Note n in logic.CurrentNotes)
            //{
            //    double d = n.FramesUntilHit;
            //}
            imageId++;
        }
    }
}
