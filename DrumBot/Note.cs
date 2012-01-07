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
        public double PerFrameVelocityX;
        public double PerFrameVelocityY;
        //keep track of how many times we have detected this exact note
        public int DetectedInFrames = 1;
        public double FramesUntilHit
        {
            get
            {
                //the velocity increases as the notes get closer to the bottom
                double distancePerFrame = Math.Sqrt(Math.Pow(PerFrameVelocityX, 2) + Math.Pow(PerFrameVelocityY, 2));
                double combFramesUntilHit = DistanceToTarget/Math.Abs(distancePerFrame);
                double yFramesUntilHit = Math.Abs((TargetPoint.Y - Center.Y)/PerFrameVelocityY);
                double avg = (combFramesUntilHit  + yFramesUntilHit)/2;
                //Debug.WriteLine(Color.ToString());
                //Debug.WriteLine("X:{0}\nY:{1}", Center.X, Center.Y);
                //Debug.WriteLine("AFrames:{0}", avg);
                //Debug.WriteLine("CFrames:{0}", combFramesUntilHit);
                //Debug.WriteLine("---------");
                return Math.Max(yFramesUntilHit, avg);
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
    }
}
