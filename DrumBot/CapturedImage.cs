using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace DrumBot
{
    public class CapturedImage
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
            DateTime startTime = DateTime.Now;
            Image = image;
            CaptureTime = captureTime;
            Image<Bgr, Byte> playArea = image.Copy(new Rectangle(218, 262, 272, 131));

            playArea = SmoothImage(playArea);
            playArea = playArea.ThresholdBinary(new Bgr(120, 100, 110), new Bgr(255, 255, 255));
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

            DrawNotes(Notes.Where(n => n.TrackColor == NoteType.Red).ToList(), ref RedTrack);
            DrawNotes(Notes.Where(n => n.TrackColor == NoteType.Yellow).ToList(), ref YellowTrack);
            DrawNotes(Notes.Where(n => n.TrackColor == NoteType.Blue).ToList(), ref BlueTrack);
            DrawNotes(Notes.Where(n => n.TrackColor == NoteType.Green).ToList(), ref GreenTrack);

            TimeSpan elapsedTime = DateTime.Now - startTime;

            Debug.WriteLine("Processing image took {0} ms.",elapsedTime.TotalMilliseconds);
        }
        private void AddNotesFromRectangles(List<Rectangle> rectangles,NoteType trackColor)
        {
            foreach (Rectangle rectangle in rectangles)
            {
                NoteType color = trackColor;
                double ratio = rectangle.Width/(double) rectangle.Height;
                if (ratio >= 3.5)
                    color = NoteType.Orange;
                if (ratio < 1.0)
                {
                    //split it up
                    int pieces = (int) Math.Round(3.5/ratio);
                    for (int i = 0; i < pieces; i++)
                    {
                        Rectangle newRect = new Rectangle(rectangle.X, rectangle.Y + (i*rectangle.Height/pieces),
                                                          rectangle.Width, rectangle.Height/pieces);
                        Notes.Add(new Note(newRect, color, trackColor));
                        Debug.WriteLine("X:{0} Y:{1} W:{2} H:{3}", newRect.X, newRect.Y, newRect.Width, newRect.Height);
                    }
                }
                else
                {
                    Notes.Add(new Note(rectangle, color, trackColor));
                }
                //Debug.WriteLine("Ratio: {0} Color:{1}", ratio, color.ToString());
            }
        }

        public const int LowerBorder = 131 - 20;
        public const int UpperBorder = 25;
        private void DrawNotes(List<Note> notesOfASingleTrack,ref Image<Bgr,byte> track)
        {
            MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 2,1);
            font.thickness = 2;
            foreach(Note note in notesOfASingleTrack)
            {
                Bgr color = new Bgr(0, 0, 0);
                switch(note.Color)
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
                track.Draw(note.Rectangle, color, 2);
                //track.Draw("X" + note.PerFrameVelocityX + ";Y" + note.PerFrameVelocityY, ref font,
                //           new Point(note.Rectangle.Left, note.Rectangle.Bottom), new Bgr(128, 128, 128));

                track.Draw(((int)note.DistanceToTarget).ToString(), ref font,
                          new Point(note.Rectangle.Left,note.Rectangle.Bottom), new Bgr(128, 128, 128));
            }
        }
        private List<Rectangle> ExtractFeatureRectangles(Image<Bgr, byte> track)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            Image<Gray, byte> grayScaleImage = new Image<Gray, byte>(track.Bitmap);
            var cannyEdges = grayScaleImage.Canny(new Gray(400), new Gray(900));
            for (Contour<Point> contours =
                     cannyEdges.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                                             Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL);
                 contours != null;
                 contours = contours.HNext)
            {

                Rectangle newRectangle = contours.BoundingRectangle;
                if (newRectangle.Height < 5 || newRectangle.Width < 20 || newRectangle.Height / newRectangle.Width > 1)
                    continue;
                //if (rectForImage.X < 0)
                //    rectForImage.X = 0;
                //if (rectForImage.Y < 0)
                //    rectForImage.Y = 0;
                var rectImage = grayScaleImage.Copy(newRectangle);
                rectImage.ThresholdBinary(new Gray(1), new Gray(255));
                double average = rectImage.GetAverage().Intensity;
                Debug.WriteLine(average);

                //to prevent false detections
                if (newRectangle.Width < 20 || newRectangle.Height < 5)
                    continue;
                //to prevent cut off large notes from being detected as bass notes further on
                if (newRectangle.Top >= LowerBorder || newRectangle.Bottom < UpperBorder)
                    continue;
                //if (!intersectionFound)
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
