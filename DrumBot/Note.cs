using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DrumBot
{
    public enum NoteType
    {
        Red,
        Yellow,
        Blue,
        Green,
        Orange
    };

    public class Note
    {
        //the rectangle is relative to the track image
        public Rectangle Rectangle;
        public Point Center
        {
            get { return new Point(Rectangle.X + (Rectangle.Width/2), Rectangle.Y + (Rectangle.Height/2)); }
        }
        public Point TargetPoint;
        public NoteType Color;
        public NoteType TrackColor;

        public double DistanceToTarget
        {
            get { return Math.Sqrt(Math.Pow(TargetPoint.X - Center.X, 2) + Math.Pow(TargetPoint.Y - Center.Y, 2)); }
        }

        public bool MatchedToOldNote;
        public bool MatchedToNewNote;
        private double _perFrameVelocityX;
        private double _perFrameVelocityY;
        public int FramesSinceLastDetection = 0;
        public double PerFrameVelocityX
        {
            get { return _perFrameVelocityX; }
            set
            {
                _lastPerFrameVelocityX = _perFrameVelocityX;
                if (AvgVelocityX == 0)
                    AvgVelocityX = value;
                //prevent any odd jumps from ruining our predictions
                if (_lastPerFrameVelocityX > 0 && value > _lastPerFrameVelocityX * 1.3)
                    value = _lastPerFrameVelocityX*1.1;
                AvgVelocityX = (AvgVelocityX + value)/2;
                _perFrameVelocityX = value;
            }
        }

        public double PerFrameVelocityY
        {
            get { return _perFrameVelocityY; }
            set
            {
                _lastPerFrameVelocityY = _perFrameVelocityY;
                if(AvgVelocityY == 0)
                    AvgVelocityY = value;
                //prevent any odd jumps from ruining our predictions
                if (_lastPerFrameVelocityY > 0 && value > _lastPerFrameVelocityY * 1.3)
                    value = _lastPerFrameVelocityY * 1.1;
                AvgVelocityY = (AvgVelocityY + value)/2;
                _perFrameVelocityY = value;
            }
        }

        private double _lastPerFrameVelocityX;
        private double _lastPerFrameVelocityY;
        public double AvgVelocityX;
        public double AvgVelocityY;
        //keep track of how many times we have detected this exact note
        public int DetectedInFrames = 1;
        //velocity seems to multiply by about 1.2 each frame
        public Rectangle PredictedPosition
        {
            get
            {
                Rectangle rectangle = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
                rectangle.X += (int) Math.Round(PerFrameVelocityX);
                rectangle.Y += (int) Math.Round(PerFrameVelocityY);
                return rectangle;
            }
        }

        private const int MaxFrames = 25;
        public int FramesUntilHit
        {
            get
            {
                //move the parts below into a function
                //the velocity increases as the notes get closer to the bottom
                if (PerFrameVelocityY == 0)
                    return Int32.MaxValue;
                Rectangle predictedPositionRectangle = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width,
                                                                     Rectangle.Height);
                int framesWithCurrentVelocity = 1;
                for (; framesWithCurrentVelocity < MaxFrames; framesWithCurrentVelocity++)
                {
                    predictedPositionRectangle.X = predictedPositionRectangle.X
                                                   +
                                                   (int)
                                                   Math.Round(PerFrameVelocityX*Math.Pow(1.1, framesWithCurrentVelocity));
                    predictedPositionRectangle.Y = predictedPositionRectangle.Y
                                                   +
                                                   (int)
                                                   Math.Round(PerFrameVelocityY*Math.Pow(1.1, framesWithCurrentVelocity));
                    if (predictedPositionRectangle.Contains(TargetPoint) || predictedPositionRectangle.Top > TargetPoint.Y)
                        break;

                }
                predictedPositionRectangle = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width,
                                                           Rectangle.Height);

                int framesWithPreviousVelocity = 1;
                for (; framesWithPreviousVelocity < MaxFrames; framesWithPreviousVelocity++)
                {
                    predictedPositionRectangle.X = predictedPositionRectangle.X
                                                   +
                                                   (int)
                                                   Math.Round(_lastPerFrameVelocityX
                                                              *Math.Pow(1.1, framesWithPreviousVelocity));
                    predictedPositionRectangle.Y = predictedPositionRectangle.Y
                                                   + (int)Math.Round(_lastPerFrameVelocityY * Math.Pow(1.1, framesWithPreviousVelocity));
                    if (predictedPositionRectangle.Contains(TargetPoint) ||  predictedPositionRectangle.Top > TargetPoint.Y)
                        break;
                }
                predictedPositionRectangle = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width,
                                                           Rectangle.Height);
                int framesWithAvgVelocity = 1;
                for (; framesWithAvgVelocity < MaxFrames; framesWithAvgVelocity++)
                {
                    predictedPositionRectangle.X = predictedPositionRectangle.X
                                                   + (int) Math.Round(AvgVelocityX*Math.Pow(1.1, framesWithAvgVelocity));
                    predictedPositionRectangle.Y = predictedPositionRectangle.Y
                                                   + (int) Math.Round(AvgVelocityY*Math.Pow(1.1, framesWithAvgVelocity));
                    if (predictedPositionRectangle.Contains(TargetPoint) || predictedPositionRectangle.Top  > TargetPoint.Y)
                        break;
                }
                Debug.WriteLine("Cur:{0}\nPre:{1}\nAvg:{2}\nSeen:{3}\n", framesWithCurrentVelocity,
                 framesWithPreviousVelocity,
                 framesWithAvgVelocity, DetectedInFrames);
                if (framesWithAvgVelocity < MaxFrames)
                    return framesWithAvgVelocity;
                if (framesWithCurrentVelocity < MaxFrames)
                    return framesWithCurrentVelocity;
                if (framesWithPreviousVelocity < MaxFrames)
                    return framesWithPreviousVelocity;
                return Int32.MaxValue;
            }
        }

        public Note(Rectangle rectangle,NoteType color,NoteType trackColor)
        {
            Rectangle = rectangle;
            Color = color;
            TrackColor = trackColor;
            TargetPoint = GetTargetPoint(TrackColor);
        }
        private Point GetTargetPoint(NoteType trackColor)
        {
            //hitbox coordinates are relative to the track image
            switch (trackColor)
            {
                //236,454 - 218,262 - 0,0
                case NoteType.Red:
                    return new Point(18, 192);
                //316,452 - 218,262  - 66,0
                case NoteType.Yellow:
                    return new Point(32, 190);
                //394,450 - 216,262 - 134,0
                case NoteType.Blue:
                    return new Point(44, 188);
                //474,454 - 216,262 - 176,0
                case NoteType.Green:
                    return new Point(82, 192);
                default:
                    throw new Exception();
            }
        }

        public override string ToString()
        {
            return Color.ToString() + " " + DistanceToTarget.ToString();
        }
        public void UpdateUsingPrediction()
        {
            Rectangle = PredictedPosition;
            PerFrameVelocityX = PerFrameVelocityX*1.1;
            PerFrameVelocityY = PerFrameVelocityY*1.1;
            FramesSinceLastDetection++;
        }
        public bool AtTargetPoint()
        {
            return Rectangle.Contains(TargetPoint) || Rectangle.Top > TargetPoint.Y;
        }
    }
}
