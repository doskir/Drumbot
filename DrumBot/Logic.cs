using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DrumBot
{
    public static class RectangleExtension
    {
        public static Point Center(this Rectangle rect)
        {
            return new Point(rect.Left + rect.Width / 2,
                             rect.Top + rect.Height / 2);
        }
    }
    internal class Logic
    {

        public List<Note> CurrentNotes = new List<Note>();

        public void UpdateAndPredictNotes(CapturedImage newFrame)
        {
            List<Note> updatedNotes = new List<Note>();
            foreach (Note newNote in newFrame.Notes)
                newNote.MatchedToOldNote = false;
            //if we keep the enum with orange at the bottom (highest int conversion) then normal notes will get priority matching
            foreach (Note oldNote in CurrentNotes.OrderByDescending(n => n.DetectedInFrames).ThenBy(n => n.Color))
            {
                oldNote.MatchedToNewNote = false;
                //find a new note that best matches the old one
                //the note has to be in the same track and has to be below the previous position
                //it does NOT have to be detect as the same color because our detection is not perfect
                //we have to account for that and then give it the most likely color
                var possibleMatches =
                    newFrame.Notes.Where(
                        nn =>
                        nn.TrackColor == oldNote.TrackColor && nn.Rectangle.Y >= oldNote.Rectangle.Y);
                //what to do if this gives us 0 results ?
                int matchCount = possibleMatches.Count();
                if(matchCount == 0)
                {
                    //went offscreen ?
                    continue;
                }

                //find the note closest to the previous position by euclidean distance
                //if we don't have any velocity data yet then this will simply return the previous position
                Rectangle predictedPosition =
                    new Rectangle(oldNote.Rectangle.X + (int) Math.Round(oldNote.PerFrameVelocityX),
                                  oldNote.Rectangle.Y + (int) Math.Round(oldNote.PerFrameVelocityY),
                                  oldNote.Rectangle.Width, oldNote.Rectangle.Height);

                //if we have a lot of notes with the right color to pick from we might as well assume that all notes
                //have been classified correctly
                if(possibleMatches.Count(pm => pm.Color == oldNote.Color) > 0)
                {
                    possibleMatches = possibleMatches.Where(pm => pm.Color == oldNote.Color);
                }
                //find the note closest to the predicted positions CENTER by euclidean distance
                var bestMatch =
                    possibleMatches.OrderBy(
                        nn =>
                        Math.Sqrt(Math.Pow(nn.Rectangle.Center().X - predictedPosition.Center().X, 2)
                                  + Math.Pow(nn.Rectangle.Center().Y - predictedPosition.Center().Y, 2))).First();
                bestMatch.PerFrameVelocityX = bestMatch.Rectangle.Center().X - oldNote.Rectangle.Center().X;
                bestMatch.PerFrameVelocityY = bestMatch.Rectangle.Center().Y - oldNote.Rectangle.Center().Y;
                
                //the notes ALWAYS get larger as they drop to the bottom
                if (bestMatch.Rectangle.Width < oldNote.Rectangle.Width || bestMatch.Rectangle.Height < oldNote.Rectangle.Height)
                {
                    bestMatch.Rectangle.Width = oldNote.Rectangle.Width;
                    bestMatch.Rectangle.Height = oldNote.Rectangle.Height;
                }


                double xMismatch = bestMatch.Rectangle.Center().X - predictedPosition.Center().X;
                double yMismatch = bestMatch.Rectangle.Center().Y - predictedPosition.Center().Y;
                //Debug.WriteLine("XDif:{0} \nYDif:{1}", xMismatch, yMismatch);
               

                //this will probably cause problems
                if(bestMatch.Color != oldNote.Color)
                {
                   bestMatch.Color = oldNote.Color;
                }
                //just to make sure we dont accidentially miscolor bass notes
                double ratio = bestMatch.Rectangle.Width / (double)bestMatch.Rectangle.Height;
                if (ratio >= 4 && bestMatch.Color != NoteType.Orange)
                    bestMatch.Color = NoteType.Orange;
                else if(ratio < 3.0)
                    bestMatch.Color = bestMatch.TrackColor;
                //if the ratio is between 3.0 and 4.0 we cannot be certain if it is a normal or a bass note so we keep the original guess
                oldNote.MatchedToNewNote = true;
                bestMatch.MatchedToOldNote = true;
                //keep track of how many times we have seen this note
                bestMatch.DetectedInFrames++;
                updatedNotes.Add(bestMatch);
            }

            //add the new notes
            foreach (Note note in newFrame.Notes.Where(n => n.MatchedToOldNote == false))
            {
                updatedNotes.Add(note);
            }
            CurrentNotes = updatedNotes;
        }

    }
}