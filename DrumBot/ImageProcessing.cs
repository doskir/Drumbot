using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace DrumBot
{
    class ImageProcessing
    {
        //will include hardcoded positions and rotations for rock band 2 drum mode
        //rotations are too slow, DONT USE THEM
        public static void ProcessImage(Image<Bgr,Byte> screenshot,out Image<Bgr,byte> redTrack,out Image<Bgr,byte> yellowTrack,out Image<Bgr,byte> blueTrack,out Image<Bgr,byte> greenTrack)
        {
            DateTime startTime = DateTime.Now;
            Image<Bgr, Byte> playArea = screenshot.Copy(new Rectangle(218, 222, 272, 171));

            playArea = SmoothImage(playArea);
            playArea = playArea.ThresholdBinary(new Bgr(120, 100, 100), new Bgr(255, 255, 255));
            playArea = SmoothImage(playArea);

            redTrack = ExtractRedTrack(playArea);
            yellowTrack = ExtractYellowTrack(playArea);
            blueTrack = ExtractBlueTrack(playArea);
            greenTrack = ExtractGreenTrack(playArea);

            TimeSpan elapsedTime = DateTime.Now - startTime;

            Debug.WriteLine(elapsedTime.TotalMilliseconds);
            //Image<Bgr,Byte> redTrack = ne
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
            Image<Bgr, Byte> blueTrack = playArea.Copy(new Rectangle(134, 0,69, 171));

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
        public static Image<Bgr,byte> ExtractGreenTrack(Image<Bgr,byte> playArea)
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
        public static Image<Bgr,byte> SmoothImage(Image<Bgr,byte> image)
        {
            //median smoothing with a size of 5 works good
            return image.SmoothMedian(5);
        }

        
    }
}
