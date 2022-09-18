using System.IO;
using System.IO.Compression;
using System.Linq;

namespace gzipconv
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[] gzipheader = { 0x1F, 0x8B, 0x08 };

            foreach (string file in args)
            {
                byte[] data = File.ReadAllBytes(file);
                byte[] header = { data[0], data[1], data[2] };
                bool isgzip = Enumerable.SequenceEqual(header, gzipheader);

                if (isgzip)
                {
                    using (var fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                    using (var ms = new MemoryStream(data))
                    using (var gs = new GZipStream(ms, CompressionMode.Decompress))
                        gs.CopyTo(fs);
                }
                else
                {
                    using (var fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                    using (var gs = new GZipStream(fs, CompressionMode.Compress))
                        gs.Write(data, 0, data.Length);
                }
            }
        }
    }
}
