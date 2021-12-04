using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    class Tape
    {
        private string FileName;
        private Buffer WriteBuffer;
        private Buffer ReadBuffer;
        private int Position = 0;
        public static int FileAccessCount { get; private set; } = 0;
        private long TapeSize { get; set; }
        public bool EOF { get => Position * 8 >= TapeSize && ReadBuffer.Empty; }

        public Tape (string fileName)
        {
            FileName = fileName;

            File.Create(fileName).Dispose();
          

            WriteBuffer = new Buffer(Settings.PAGE_SIZE);
            ReadBuffer = new Buffer(Settings.PAGE_SIZE);
        }

        private void Flush()
        {
            if (WriteBuffer.Empty) return;

            FileAccessCount++;
            // if (Settings.DEBUG) Console.WriteLine("Accessing File and Flushing");

            using (BinaryWriter writer = new BinaryWriter(File.Open(FileName, FileMode.Append)))
            {
                foreach (Record record in WriteBuffer)
                {
                    writer.Write(record.Value);
                }

                TapeSize = writer.BaseStream.Length;
            }

            WriteBuffer.Clear();
        }

        public void Write(Record record)
        {
            if (WriteBuffer.Full)
            {
                Flush();
            }

            WriteBuffer.Append(record);
            TapeSize += Settings.RECORD_SIZE;
        }

        public void ResetReader()
        {
            Position = 0;
        }

        public void Clear()
        {
            File.Create(FileName).Dispose();
            ResetReader();
        }

        private void FillBuffer()
        {
            // if (Settings.DEBUG) Console.WriteLine("Accessing File");
            FileAccessCount++;


            using (BinaryReader reader = new BinaryReader(File.Open(FileName, FileMode.Open)))
            {
                TapeSize = reader.BaseStream.Length;

                // Skip the records that are already read
                reader.BaseStream.Seek(Position * Settings.RECORD_SIZE, SeekOrigin.Begin);

                int counter = 0;

                while (counter < Settings.PAGE_SIZE && reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    ReadBuffer.Append(new Record(reader.ReadDouble()));
                    counter++;
                }

                while (!WriteBuffer.Finished && counter < Settings.PAGE_SIZE)
                {
                    ReadBuffer.Append(WriteBuffer.Next());
                }
            }


        }

        public Record Peek()
        {
            if (!WriteBuffer.Empty) Flush();

            if (ReadBuffer.Empty)
            {
                FillBuffer();
            }

            return ReadBuffer.Peek();
        }

        public Record Read()
        {
            if (!WriteBuffer.Empty) Flush();

            if (ReadBuffer.Empty)
            {
                FillBuffer();
            }

            Position++;
            return ReadBuffer.Next();
        }
    }
}
