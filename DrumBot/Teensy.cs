using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace DrumBot
{
    class Teensy : IDisposable,IDrumHardware
    {
        private SerialPort port;
        private bool _writeAllowed;
        public bool WriteAllowed
        {
            get { return _writeAllowed; }
            set { _writeAllowed = value; }
        }

        public Teensy(string serialPortName)
        {
            try
            {
                port = new SerialPort(serialPortName, 9600);
                Open();
                WriteAllowed = true;
            }
            catch
            {
                
            }
        }
        private void SendString(string s)
        {
            if (WriteAllowed)
                port.Write(s);
        }



        public void HitNote(NoteType color)
        {
            SendString(color.ToString().ToUpper()[0].ToString());
        }

        public void Open()
        {
            port.Open();
        }

        public void Close()
        {
            port.Close();
        }

        public void Dispose()
        {
            port.Close();
            port.Dispose();
        }
    }
}
