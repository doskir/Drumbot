using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace DrumBot
{
    class CapturedImage
    {
        public Image<Bgr, Byte> Image;
        public DateTime CaptureTime;
        public Image<Bgr, byte> RedTrack;
        public Image<Bgr, byte> YellowTrack;
        public Image<Bgr, byte> BlueTrack;
        public Image<Bgr, byte> GreenTrack; 
        public CapturedImage(Image<Bgr, Byte> image, DateTime captureTime)
        {
            Image = image;
            CaptureTime = captureTime;
            DateTime startTime = DateTime.Now;
            Image<Bgr, Byte> playArea = image.Copy(new Rectangle(218, 222, 272, 171));

            playArea = SmoothImage(playArea);
            playArea = playArea.ThresholdBinary(new Bgr(120, 100, 100), new Bgr(255, 255, 255));
            playArea = SmoothImage(playArea);

            RedTrack = ExtractRedTrack(playArea);
            YellowTrack = ExtractYellowTrack(playArea);
            BlueTrack = ExtractBlueTrack(playArea);
            GreenTrack = ExtractGreenTrack(playArea);

            TimeSpan elapsedTime = DateTime.Now - startTime;

            Debug.WriteLine(elapsedTime.TotalMilliseconds);
        }
        public static Image<Bgr, byte> ExtractRedTrack(Image<Bgr, byte> playArea)
        {
            Image<Bgr, Byte> redTrack = playArea.Copy(new Rectangle(0, 0, 96, 171));

            List<Point> leftCover = new List<Point>();
            leftCover.Add(new Point(0, 0));
            leftCover.Add(new Point(61, 0));
            leftCover.Add(new Point(0, 170));
            redTrack.FillConvexPoly(leftCover.ToArray(), new Bgr(0, 0, 0));

            List<Point> rightCover = new List<Point>();
            rightCover.Add(new Point(65, 170));
            rightCover.Add(new Point(95, 0));
            rightCover.Add(new Point(95, 170));
            redTrack.FillConvexPoly(rightCover.ToArray(), new Bgr(0, 0, 0));


            return redTrack;
        }
        public static Image<Bgr, byte> ExtractBlueTrack(Image<Bgr, byte> playArea)
        {
            Image<Bgr, Byte> blueTrack = playArea.Copy(new Rectangle(134, 0, 69, 171));

            List<Point> rightCover = new List<Point>();
            rightCover.Add(new Point(40, 0));
            rightCover.Add(new Point(69, 170));
            rightCover.Add(new Point(69, 0));
            blueTrack.FillConvexPoly(rightCover.ToArray(), new Bgr(0, 0, 0));

            return blueTrack;
        }
        public static Image<Bgr, byte> ExtractYellowTrack(Image<Bgr, byte> playArea)
        {
            Image<Bgr, Byte> yellowTrack = playArea.Copy(new Rectangle(66, 0, 69, 171));

            List<Point> leftCover = new List<Point>();
            leftCover.Add(new Point(0, 0));
            leftCover.Add(new Point(34, 0));
            leftCover.Add(new Point(0, 170));
            yellowTrack.FillConvexPoly(leftCover.ToArray(), new Bgr(0, 0, 0));

            return yellowTrack;
        }
        public static Image<Bgr, byte> ExtractGreenTrack(Image<Bgr, byte> playArea)
        {
            Image<Bgr, Byte> greenTrack = playArea.Copy(new Rectangle(176, 0, 96, 171));

            List<Point> leftCover = new List<Point>();
            leftCover.Add(new Point(0, 0));
            leftCover.Add(new Point(28, 170));
            leftCover.Add(new Point(0, 170));
            greenTrack.FillConvexPoly(leftCover.ToArray(), new Bgr(0, 0, 0));

            List<Point> rightCover = new List<Point>();
            rightCover.Add(new Point(33, 0));
            rightCover.Add(new Point(93, 170));
            rightCover.Add(new Point(95, 170));
            rightCover.Add(new Point(95, 0));
            greenTrack.FillConvexPoly(rightCover.ToArray(), new Bgr(0, 0, 0));

            return greenTrack;
        }
        public static Image<Bgr, byte> SmoothImage(Image<Bgr, byte> image)
        {
            //median smoothing with a size of 5 works good
            return image.SmoothMedian(5);
        }
        public void Save(string path)
        {
            Image.Save(path);
        }
        public void Dispose()
        {
            Image.Dispose();
            RedTrack.Dispose();
            YellowTrack.Dispose();
            BlueTrack.Dispose();
            GreenTrack.Dispose();
        }
    }
}
