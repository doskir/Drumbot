using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        public void ImageWriter()
        {
            int frameCount = 0;
            while (true)
            {
                while (ImageQueue.Count > 0)
                {
                    if (globCaptureStart == DateTime.MinValue)
                        globCaptureStart = DateTime.Now;
                    CapturedImage image = ImageQueue.Peek();
                    if(image.Image == MostRecentImage)
                    {
                        break;
                    }
                    if (Recording)
                    {
                        image.Save("D:\\captures\\" + frameCount.ToString().PadLeft(5, '0') + ".bmp");
                        frameCount++;
                    }
                    ImageQueue.Dequeue();
                    image.Dispose();
                }
                Thread.Sleep(1);
            }
        }

        public Image<Bgr, Byte> MostRecentImage;

        public void StartCapturing()
        {
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

        void captureTimer_Timer(object sender, EventArgs e)
        {
            DateTime captureTime = DateTime.Now;
            Image<Bgr, Byte> image = globCapture.QueryFrame();
            MostRecentImage = image;
            CapturedImage capturedImage = new CapturedImage(image, captureTime);
            ImageQueue.Enqueue(capturedImage);
            globFrameCount++;
            TimeSpan timeElapsed = DateTime.Now - captureTime;
            if (timeElapsed.TotalMilliseconds > 45)
            {
                System.Diagnostics.Debug.WriteLine("WARNING: Capture took {0} ms.", timeElapsed.TotalMilliseconds);
            }
        }
        public void StopCapturing()
        {
            captureTimer.Stop();
            captureTimer.Dispose();
            globCapture.Dispose();
            writerThread.Abort();
        }
        public class CapturedImage
        {
            public Image<Bgr, Byte> Image;
            public DateTime CaptureTime;
            public CapturedImage(Image<Bgr,Byte> image,DateTime captureTime)
            {
                Image = image;
                CaptureTime = captureTime;
            }
            public void Save(string path)
            {
                Image.Save(path);
            }
            public void Dispose()
            {
                Image.Dispose();
            }
        }
    }
}
