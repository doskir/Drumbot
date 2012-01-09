using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace DrumBot
{
    class Teensy : IDisposable
    {
        private SerialPort port;
        public bool WriteAllowed;
        public Teensy(string serialPortName)
        {
            port = new SerialPort(serialPortName, 9600);
            port.Open();
            WriteAllowed = true;
        }
        public void SendString(string s)
        {
            if (WriteAllowed)
                port.Write(s);
        }

        public void HitNote(NoteType color)
        {
            SendString(color.ToString().ToUpper()[0].ToString());
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
