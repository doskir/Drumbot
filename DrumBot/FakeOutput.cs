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

        public bool RedHit;
        public bool YellowHit;
        public bool BlueHit;
        public bool GreenHit;
        public bool OrangeHit;
        public void HitNote(NoteType color)
        {
            switch (color)
            {
                case NoteType.Red:
                    RedHit = true;
                    break;
                case NoteType.Yellow:
                    YellowHit = true;
                    break;
                case NoteType.Blue:
                    BlueHit = true;
                    break;
                case NoteType.Green:
                    GreenHit = true;
                    break;
                case NoteType.Orange:
                    OrangeHit = true;
                    break;
            }
        }
        public void ResetHitIndicators()
        {
            RedHit = false;
            YellowHit = false;
            BlueHit = false;
            GreenHit = false;
            OrangeHit = false;
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
