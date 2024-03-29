//Most of this code is lifted from Bizhawk
//https://github.com/tasvideos/bizhawk
//Thanks guys

namespace RTCV.UI.Input
{
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Pipes;
    using System.Threading;
    using SlimDX.DirectInput;

    public static class IPCKeyInput
    {
        public static void Initialize()
        {
            var t = new Thread(IPCThread);
            t.IsBackground = true;
            t.Start();
        }

        static List<KeyEvent> PendingEventList = new List<KeyEvent>();
        static List<KeyEvent> EventList = new List<KeyEvent>();

        static void IPCThread()
        {
            string pipeName = string.Format("rtc-pid-{0}-IPCKeyInput", System.Diagnostics.Process.GetCurrentProcess().Id);

            for (; ; )
            {
                using (NamedPipeServerStream pipe = new NamedPipeServerStream(pipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 1024, 1024))
                {
                    try
                    {
                        pipe.WaitForConnection();

                        BinaryReader br = new BinaryReader(pipe);

                        for (; ; )
                        {
                            int e = br.ReadInt32();
                            bool pressed = (e & 0x80000000) != 0;
                            lock (PendingEventList)
                            {
                                PendingEventList.Add(new KeyEvent { Key = (Key)(e & 0x7FFFFFFF), Pressed = pressed });
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        public static IEnumerable<KeyEvent> Update()
        {
            EventList.Clear();

            lock (PendingEventList)
            {
                EventList.AddRange(PendingEventList);
                PendingEventList.Clear();
            }

            return EventList;
        }
    }
}
