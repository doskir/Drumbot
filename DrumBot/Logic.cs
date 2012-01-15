using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
        private IDrumHardware _drumHardware;
        public Logic(IDrumHardware drumHardware)
        {
            _drumHardware = drumHardware;
        }
        public List<Note> CurrentNotes = new List<Note>();
        private int _framesSinceLastOrangeNote = 1000;
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
                Note note = oldNote;
                var possibleMatches =
                    newFrame.Notes.Where(
                        nn =>
                        nn.TrackColor == note.TrackColor && nn.Rectangle.Y >= note.Rectangle.Y
                        && nn.MatchedToOldNote == false);
                //find the note closest to the previous position by euclidean distance
                //if we don't have any velocity data yet then this will simply return the previous position
                Rectangle predictedPosition = oldNote.PredictedPosition;

                //what to do if this gives us 0 results ?
                int matchCount = possibleMatches.Count();
                if (matchCount == 0)
                {
                    //went offscreen ?
                    continue;
                }


                //if we have a lot of notes with the right color to pick from we might as well assume that all notes
                //have been classified correctly
                var colorMatches = possibleMatches.Where(pm => pm.Color == oldNote.Color);
                if(colorMatches.Count() > 0)
                    possibleMatches = colorMatches;
                //find the note closest to the predicted positions CENTER by euclidean distance
                var bestMatch =
                    possibleMatches.OrderBy(
                        nn =>
                        Math.Sqrt(Math.Pow(nn.Rectangle.Center().X - predictedPosition.Center().X, 2)
                                  + Math.Pow(nn.Rectangle.Center().Y - predictedPosition.Center().Y, 2))).First();
                bestMatch.PerFrameVelocityX = bestMatch.Rectangle.Center().X - oldNote.Rectangle.Center().X;
                bestMatch.PerFrameVelocityY = bestMatch.Rectangle.Center().Y - oldNote.Rectangle.Center().Y;


                double xMismatch = bestMatch.Rectangle.Center().X - predictedPosition.Center().X;
                double yMismatch = bestMatch.Rectangle.Center().Y - predictedPosition.Center().Y;
                //made up numbers
                if(bestMatch.Color != oldNote.Color && xMismatch < 2 && yMismatch < 3)
                {
                    bestMatch.Color = oldNote.Color;
                }
                //Debug.WriteLine("XDif:{0} \nYDif:{1}", xMismatch, yMismatch);


                //just to make sure we dont accidentially miscolor bass notes
                double ratio = bestMatch.Rectangle.Width/(double) bestMatch.Rectangle.Height;
                if (ratio >= 4 && bestMatch.Color != NoteType.Orange)
                    bestMatch.Color = NoteType.Orange;
                else if (ratio < 3.0)
                    bestMatch.Color = bestMatch.TrackColor;
                //if the ratio is between 3.0 and 4.0 we cannot be certain if it is a normal or a bass note so we keep the original guess
                oldNote.MatchedToNewNote = true;
                bestMatch.MatchedToOldNote = true;
                //keep track of how many times we have seen this note
                bestMatch.DetectedInFrames += oldNote.DetectedInFrames;
                bestMatch.FramesSinceLastDetection = 0;
                updatedNotes.Add(bestMatch);
            }

            //add the new notes
            updatedNotes.AddRange(newFrame.Notes.Where(n => n.MatchedToOldNote == false));
            bool orangeNoteTriggered = false;
            foreach(Note note in CurrentNotes.Where(n=>n.MatchedToNewNote == false && n.FramesSinceLastDetection < 10))
            {
                //get the predicted position in the CURRENT frame
                note.UpdateUsingPrediction();
                //check if the predicted position in the next frame is at the target
                if (note.AtTargetPoint(note.PredictedPosition))
                {
                    if (note.Color == NoteType.Orange)
                    {
                        if (orangeNoteTriggered || _framesSinceLastOrangeNote < 2)
                            continue;
                        orangeNoteTriggered = true;
                    }
                    _drumHardware.HitNote(note.Color);
                }
                else
                {
                    updatedNotes.Add(note);
                }
            }
            if (orangeNoteTriggered)
                _framesSinceLastOrangeNote = 0;
            else
                _framesSinceLastOrangeNote++;
            CurrentNotes = updatedNotes;
        }

    }
}