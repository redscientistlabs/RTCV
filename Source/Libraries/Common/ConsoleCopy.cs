namespace RTCV.Common
{
    using System;
    using System.IO;
    using System.Text;

    public class ConsoleCopy : IDisposable
    {
        private FileStream fileStream;
        public StreamWriter FileWriter;
        private TextWriter doubleWriter;
        private TextWriter oldOut;

        private class DoubleWriter : TextWriter
        {
            private TextWriter one;
            private TextWriter two;

            public DoubleWriter(TextWriter one, TextWriter two)
            {
                this.one = one;
                this.two = two;
            }

            public override Encoding Encoding => one.Encoding;

            public override void Flush()
            {
                one.Flush();
                two.Flush();
            }

            public override void Write(char value)
            {
                one.Write(value);
                two.Write(value);
            }
        }

        public ConsoleCopy(string path)
        {
            oldOut = Console.Out;

            try
            {
                var dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.Create(path).Close();
                fileStream = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.Read);
                FileWriter = new StreamWriter(fileStream)
                {
                    AutoFlush = true
                };

                doubleWriter = new DoubleWriter(FileWriter, oldOut);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open file for writing");
                Console.WriteLine(e.Message);
                return;
            }
            Console.SetOut(doubleWriter);
            Console.SetError(doubleWriter);
        }

        public void Dispose()
        {
            Console.SetOut(oldOut);
            if (FileWriter != null)
            {
                FileWriter.Flush();
                FileWriter.Close();
                FileWriter = null;
            }
            if (fileStream != null)
            {
                fileStream.Close();
                fileStream = null;
            }
            if (doubleWriter != null)
            {
                doubleWriter.Dispose();
                doubleWriter = null;
            }
        }
    }
}
