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
        public NoteType Color;
        public Note(Rectangle rectangle,NoteType color)
        {
            Rectangle = rectangle;
            Color = color;
        }
        public string ToString()
        {
            return Color.ToString();
        }
    }
}
