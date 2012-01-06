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
        public List<Note> Notes = new List<Note>();

        public CapturedImage(Image<Bgr, Byte> image, DateTime captureTime)
        {
            Image = image;
            CaptureTime = captureTime;
            DateTime startTime = DateTime.Now;
            Image<Bgr, Byte> playArea = image.Copy(new Rectangle(218, 262, 272, 131));

            playArea = SmoothImage(playArea);
            playArea = playArea.ThresholdBinary(new Bgr(120, 100, 100), new Bgr(255, 255, 255));
            playArea = SmoothImage(playArea);

            RedTrack = ExtractRedTrack(playArea);
            YellowTrack = ExtractYellowTrack(playArea);
            BlueTrack = ExtractBlueTrack(playArea);
            GreenTrack = ExtractGreenTrack(playArea);

            List<Rectangle> RedTrackRectangles = ExtractFeatureRectangles(RedTrack);
            List<Rectangle> YellowTrackRectangles = ExtractFeatureRectangles(YellowTrack);
            List<Rectangle> BlueTrackRectangles = ExtractFeatureRectangles(BlueTrack);
            List<Rectangle> GreenTrackRectangles = ExtractFeatureRectangles(GreenTrack);
            AddNotesFromRectangles(RedTrackRectangles, NoteType.Red);
            AddNotesFromRectangles(YellowTrackRectangles, NoteType.Yellow);
            AddNotesFromRectangles(BlueTrackRectangles, NoteType.Blue);
            AddNotesFromRectangles(GreenTrackRectangles, NoteType.Green);

            DrawFeatureRectangles(RedTrackRectangles, ref RedTrack, new Bgr(0, 0, 255));
            DrawFeatureRectangles(YellowTrackRectangles, ref YellowTrack, new Bgr(0, 255, 255));
            DrawFeatureRectangles(BlueTrackRectangles, ref BlueTrack, new Bgr(255, 0, 0));
            DrawFeatureRectangles(GreenTrackRectangles, ref GreenTrack, new Bgr(0, 255, 0));

            TimeSpan elapsedTime = DateTime.Now - startTime;

            Debug.WriteLine(elapsedTime.TotalMilliseconds);
        }
        private void AddNotesFromRectangles(List<Rectangle> rectangles,NoteType defaultColor)
        {
            foreach(Rectangle rectangle in rectangles)
            {
                NoteType color = defaultColor;
                if (rectangle.Width / (double)rectangle.Height >= 4.0f)
                    color = NoteType.Orange;
                Notes.Add(new Note(rectangle, color));
            }
        }

        private void DrawFeatureRectangles(List<Rectangle> rectangles,ref Image<Bgr,byte> track,Bgr color)
        {
            foreach (Rectangle rectangle in rectangles.OrderBy(r => r.Y))
            {
                //using white for easy visibility
                //if the the rectangle is 4 times as wide as its high it is an orange note
                if(rectangle.Width/(double)rectangle.Height >= 4.0f)
                {
                    track.Draw(rectangle, new Bgr(255, 255, 255), 2);
                }
                else
                {
                    track.Draw(rectangle, color, 2);
                }
            }
        }
        private List<Rectangle> ExtractFeatureRectangles(Image<Bgr, byte> track)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            Image<Gray, byte> grayScaleImage = new Image<Gray, byte>(track.Bitmap);
            var cannyEdges = grayScaleImage.Canny(new Gray(200), new Gray(20));
            for (Contour<Point> contours =
                     cannyEdges.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                                             Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL);
                 contours != null;
                 contours = contours.HNext)
            {
                Rectangle newRectangle = contours.GetMinAreaRect().MinAreaRect();
                bool intersectionFound = false;
                for (int i = 0; i < rectangles.Count; i++)
                {
                    Rectangle rectangle = rectangles[i];
                    if (newRectangle.IntersectsWith(rectangle) || newRectangle.Contains(rectangle))
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
            return rectangles;
        }

        public static Image<Bgr, byte> ExtractRedTrack(Image<Bgr, byte> playArea)
        {
            Image<Bgr, Byte> redTrack = playArea.Copy(new Rectangle(0, 0, 96, playArea.Height));

            List<Point> leftCover = new List<Point>();
            leftCover.Add(new Point(0, 0));
            leftCover.Add(new Point(45, 0));
            leftCover.Add(new Point(0, playArea.Height -1));
            redTrack.FillConvexPoly(leftCover.ToArray(), new Bgr(0, 0, 0));

            List<Point> rightCover = new List<Point>();
            rightCover.Add(new Point(65, 128));
            rightCover.Add(new Point(91, 0));
            rightCover.Add(new Point(95, 0));
            rightCover.Add(new Point(95, playArea.Height - 1));
            redTrack.FillConvexPoly(rightCover.ToArray(), new Bgr(0, 0, 0));


            return redTrack;
        }
        public static Image<Bgr, byte> ExtractYellowTrack(Image<Bgr, byte> playArea)
        {
            Image<Bgr, Byte> yellowTrack = playArea.Copy(new Rectangle(66, 0, 69, playArea.Height));

            List<Point> leftCover = new List<Point>();
            leftCover.Add(new Point(0, 0));
            leftCover.Add(new Point(20, 0));
            leftCover.Add(new Point(0, 170));
            yellowTrack.FillConvexPoly(leftCover.ToArray(), new Bgr(0, 0, 0));

            return yellowTrack;
        }
        public static Image<Bgr, byte> ExtractBlueTrack(Image<Bgr, byte> playArea)
        {
            Image<Bgr, Byte> blueTrack = playArea.Copy(new Rectangle(134, 0, 69, playArea.Height));

            List<Point> rightCover = new List<Point>();
            rightCover.Add(new Point(45, 0));
            rightCover.Add(new Point(69, playArea.Height - 1));
            rightCover.Add(new Point(69, 0));
            blueTrack.FillConvexPoly(rightCover.ToArray(), new Bgr(0, 0, 0));

            return blueTrack;
        }
        public static Image<Bgr, byte> ExtractGreenTrack(Image<Bgr, byte> playArea)
        {
            Image<Bgr, Byte> greenTrack = playArea.Copy(new Rectangle(176, 0, 96, playArea.Height));

            List<Point> leftCover = new List<Point>();
            leftCover.Add(new Point(0, 0));
            leftCover.Add(new Point(7, 0));
            leftCover.Add(new Point(28, playArea.Height - 1));
            leftCover.Add(new Point(0, playArea.Height -1));
            greenTrack.FillConvexPoly(leftCover.ToArray(), new Bgr(0, 0, 0));

            List<Point> rightCover = new List<Point>();
            rightCover.Add(new Point(50, 0));
            rightCover.Add(new Point(95, playArea.Height -1));
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
