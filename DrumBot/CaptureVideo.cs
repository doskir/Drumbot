using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace DrumBot
{
    class CaptureVideo
    {
        public Queue<CapturedImage> ImageQueue = new Queue<CapturedImage>();
        public bool Recording = false;
        private Capture globCapture;
        public DateTime globCaptureStart;
        public int globFrameCount;

        private MMTimer captureTimer = new MMTimer();

        private Thread writerThread;
        public bool Flagged;
        private int _flagId;
        public void ImageWriter()
        {
            int frameCount = 0;
            while (true)
            {
                if(Flagged)
                {
                    _flagId++;
                    System.IO.Directory.CreateDirectory(@"D:\flagged\" + _flagId);
                    CapturedImage[] imageArray = ImageQueue.ToArray();
                    for(int i = 0; i < imageArray.Length;i++)
                    {
                        CapturedImage image = imageArray[i];
                        string filePath = string.Format(@"D:\flagged\{0}\{1}.png", _flagId, i);
                        image.Save(filePath);
                    }
                    Flagged = false;
                }
                //keep the last 100 images in the buffer
                if (ImageQueue.Count > 100)
                {
                    if (globCaptureStart == DateTime.MinValue)
                        globCaptureStart = DateTime.Now;
                    CapturedImage image = ImageQueue.Peek();
                    if (Recording)
                    {
                        image.Save("D:\\captures\\" + frameCount.ToString().PadLeft(5, '0') + ".png");
                        frameCount++;
                    }
                    ImageQueue.Dequeue();
                    image.Dispose();
                }
                Thread.Sleep(1);
            }
        }

        private Logic _logic;
        public void StartCapturing(object logic)
        {
            _logic = (Logic)logic;
            int fps = 25;

            writerThread = new Thread(ImageWriter);
            writerThread.Name = "writerThread";
            writerThread.Start();

            globCapture = new Capture();
            globCapture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 576);
            globCapture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 704);
            globFrameCount = 0;
            // start timer
            captureTimer.Timer += new EventHandler(captureTimer_Timer);
            captureTimer.Start((uint) (1000/fps), true);
            //now capturing
        }
        public CapturedImage MostRecentImage;
        public Image<Bgr,byte> GetBigPicture()
        {
            MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 1, 1);
            font.thickness = 2;
            Image<Bgr, byte> newImage = MostRecentImage.Image.Clone();
            foreach (Note note in _logic.CurrentNotes.Where(n => n.DetectedInFrames > 1))
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
                              new Point(drawRectangle.Left, drawRectangle.Bottom), new Bgr(0, 0, 0));
                //newImage.Draw("X" + note.PerFrameVelocityX + ";Y" + note.PerFrameVelocityY, ref font,
                //              new Point(drawRectangle.Left, drawRectangle.Bottom), new Bgr(0,0,0));
                //newImage.Draw(((int) note.DistanceToTarget).ToString(), ref font,
                //              new Point(drawRectangle.Left, drawRectangle.Bottom), new Bgr(128, 128, 128));
            }
            return newImage;
        }
        void captureTimer_Timer(object sender, EventArgs e)
        {
            DateTime captureTime = DateTime.Now;
            Image<Bgr, Byte> image = globCapture.QueryFrame();
            CapturedImage capturedImage = new CapturedImage(image, captureTime);
            MostRecentImage = capturedImage;
            _logic.UpdateAndPredictNotes(capturedImage);
            //anything below this line should not affect the main part of the bot
            TimeSpan timeElapsed = DateTime.Now - captureTime;
            if (timeElapsed.TotalMilliseconds > 45)
            {
                Debug.WriteLine("WARNING: Capture took {0} ms.", timeElapsed.TotalMilliseconds);
            }
            ImageQueue.Enqueue(capturedImage);
            globFrameCount++;

        }
        public void StopCapturing()
        {
            captureTimer.Stop();
            captureTimer.Dispose();
            globCapture.Dispose();
            writerThread.Abort();
        }
    }
}
