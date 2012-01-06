using System;
using System.Collections.Generic;
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
    
    class Note
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
        public double MovementPerFrame;

        public double DistanceToTarget
        {
            get { return Math.Sqrt(Math.Pow(TargetPoint.X - Center.X, 2) + Math.Pow(TargetPoint.Y - Center.Y, 2)); }
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
                case NoteType.Red:
                    return new Point(18, 193);
                case NoteType.Yellow:
                    return new Point(25, 188);
                case NoteType.Blue:
                    return new Point(45, 191);
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
