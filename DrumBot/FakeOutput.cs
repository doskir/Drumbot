using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrumBot
{
    class FakeOutput : IDrumHardware
    {
        public bool WriteAllowed
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void HitNote(NoteType color)
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
