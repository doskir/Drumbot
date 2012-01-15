using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace DrumBot
{
    public partial class ReplayWindow : Form
    {
        private FakeOutput _fakeOutput;
        public ReplayWindow()
        {
            InitializeComponent();
            _fakeOutput = new FakeOutput();
            logic = new Logic(_fakeOutput);
        }
        public void RefreshPictureBoxes()
        {
            redTrack1PictureBox.Image = previousImage.RedTrack.Bitmap;
            yellowTrack1PictureBox.Image = previousImage.YellowTrack.Bitmap;
            blueTrack1PictureBox.Image = previousImage.BlueTrack.Bitmap;
            greenTrack1PictureBox.Image = previousImage.GreenTrack.Bitmap;
            redTrack2PictureBox.Image = currentImage.RedTrack.Bitmap;
            yellowTrack2PictureBox.Image = currentImage.YellowTrack.Bitmap;
            blueTrack2PictureBox.Image = currentImage.BlueTrack.Bitmap;
            greenTrack2PictureBox.Image = currentImage.GreenTrack.Bitmap;
        }


        private CapturedImage previousImage;
        private CapturedImage currentImage;
        private Logic logic;
        private void button1_Click(object sender, EventArgs e)
        {
            ReloadTracks();
            RefreshPictureBoxes();
            nextImageIndex = 0;
        }
        private void ReloadTracks()
        {
            logic.CurrentNotes.Clear();
            if (previousImage != null)
                previousImage.Dispose();
            if (currentImage != null)
                currentImage.Dispose();



            previousImage = new CapturedImage(new Image<Bgr, byte>(_imageFileList[0]), DateTime.Now);
            currentImage = new CapturedImage(new Image<Bgr, byte>(_imageFileList[1]),previousImage.CaptureTime.AddMilliseconds(40));
            nextImageIndex = 2;
            MostRecentImage = previousImage;
            logic.UpdateAndPredictNotes(MostRecentImage);
            MostRecentImage = currentImage;
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

        private void redTrack1PictureBox_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs) e;
            if (me.X < redTrack1PictureBox.Image.Width && me.Y < redTrack1PictureBox.Image.Height)
            {
                Color pixel = previousImage.RedTrack.Bitmap.GetPixel(me.X, me.Y);
               // Debug.WriteLine("{0},{1}", me.X, me.Y);
                //Debug.WriteLine("{0};{1};{2}", pixel.R, pixel.G, pixel.B);
            }
        }

        private void redTrack2PictureBox_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.X < redTrack2PictureBox.Image.Width && me.Y < redTrack2PictureBox.Image.Height)
            {
                Color pixel = currentImage.RedTrack.Bitmap.GetPixel(me.X, me.Y);
                //Debug.WriteLine("{0},{1}", me.X, me.Y);
                //Debug.WriteLine("{0};{1};{2}", pixel.R, pixel.G, pixel.B);
            }
        }

        private int nextImageIndex = 0;
        private void button2_Click_1(object sender, EventArgs e)
        {
            previousImage = currentImage;
            currentImage = new CapturedImage(new Image<Bgr, byte>(_imageFileList[nextImageIndex]),
                                             previousImage.CaptureTime.AddMilliseconds(40));
            nextImageIndex++;
            MostRecentImage = currentImage;

            logic.UpdateAndPredictNotes(MostRecentImage);
            DrawBigPicture();
            RefreshPictureBoxes();
            //just to get the debug console output
            //foreach(Note n in logic.CurrentNotes)
            //{
            //    double d = n.FramesUntilHit;
            //}
        }

        private string _selectedFolderPath;
        private List<string> _imageFileList = new List<string>(); 
        private void button3_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                _selectedFolderPath = folderBrowserDialog1.SelectedPath;
            }
            ReloadImageFileList();
            ReloadTracks();
            RefreshPictureBoxes();
        }
        private void ReloadImageFileList()
        {
            _imageFileList = Directory.GetFiles(_selectedFolderPath).ToList();
            _imageFileList.Sort();
            nextImageIndex = 0;
        }
    }
}
