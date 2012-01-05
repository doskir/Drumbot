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
    class ImageProcessing
    {
        //will include hardcoded positions and rotations for rock band 2 drum mode
        //rotations are too slow, DONT USE THEM
        public void ProcessImage(Image<Bgr,Byte> screenshot)
        {
            DateTime startTime = DateTime.Now;
            Image<Bgr, Byte> playArea = screenshot.Copy(new Rectangle(218, 222, 272, 171));
            //trackArea.Save("trackarea.bmp");
            //Image<Bgr, Byte> redTrack = playArea.Copy(new Rectangle(0, 0, 96, 171));
           // redTrack = redTrack.Rotate(-15.0d, new Bgr(0, 0, 0), false);
           // redTrack.Save("redtrack.bmp");
            Image<Bgr, Byte> greenTrack = ExtractGreenTrack(playArea);
            greenTrack = SmoothImage(greenTrack);
            greenTrack.Save("greentrack.bmp");

            TimeSpan elapsedTime = DateTime.Now - startTime;

            Debug.WriteLine(elapsedTime.TotalMilliseconds);
            //Image<Bgr,Byte> redTrack = ne
        }
        public Image<Bgr,byte> ExtractGreenTrack(Image<Bgr,byte> playArea)
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
        public Image<Bgr,byte> SmoothImage(Image<Bgr,byte> image)
        {
            //because of interlacing we need to apply some smoothing to the image
            //gaussian smoothing with a kernel size of 3 looks pretty good so lets keep that one for now
            image = image.SmoothGaussian(3);
            return image;
        }
    }
}
