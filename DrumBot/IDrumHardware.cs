using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrumBot
{
    interface IDrumHardware : IDisposable
    {
        bool WriteAllowed { get; set; }
        void HitNote(NoteType color);
        void Open();
        void Close();
    }
}
