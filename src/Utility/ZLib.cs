using System.IO;
using System.Text;
using Ionic.Zlib;

namespace EFTLauncher.Utility
{
    public static class ZLib
    {
        public static string Decompress(byte[] input)
        {
            int outputSize = 2048;
            byte[] output = new byte[outputSize];

            using (MemoryStream ms = new MemoryStream())
            {
                ZlibCodec compressor = new ZlibCodec();
                compressor.InitializeInflate(true);     // expect RPC1950 header

                compressor.InputBuffer = input;
                compressor.AvailableBytesIn = input.Length;
                compressor.NextIn = 0;
                compressor.OutputBuffer = output;

                foreach (var f in new FlushType[] { FlushType.None, FlushType.Finish })
                {
                    int bytesToWrite = 0;
                    do
                    {
                        compressor.AvailableBytesOut = outputSize;
                        compressor.NextOut = 0;
                        compressor.Inflate(f);

                        bytesToWrite = outputSize - compressor.AvailableBytesOut;
                        if (bytesToWrite > 0)
                            ms.Write(output, 0, bytesToWrite);
                    }
                    while ((f == FlushType.None && (compressor.AvailableBytesIn != 0 || compressor.AvailableBytesOut == 0)) ||
                           (f == FlushType.Finish && bytesToWrite != 0));
                }

                compressor.EndInflate();

                return UTF8Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public static byte[] Compress(string input)
        {
            int outputSize = 2048;
            byte[] inputBuffer = UTF8Encoding.UTF8.GetBytes(input);
            byte[] output = new byte[outputSize];
            int lengthToCompress = inputBuffer.Length;

            using (MemoryStream ms = new MemoryStream())
            {
                ZlibCodec compressor = new ZlibCodec();
                compressor.InitializeDeflate(CompressionLevel.BestCompression, true);   // add RPC1950 header

                compressor.InputBuffer = inputBuffer;
                compressor.AvailableBytesIn = lengthToCompress;
                compressor.NextIn = 0;
                compressor.OutputBuffer = output;

                foreach (var f in new FlushType[] { FlushType.None, FlushType.Finish
})
                {
                    int bytesToWrite = 0;
                    do
                    {
                        compressor.AvailableBytesOut = outputSize;
                        compressor.NextOut = 0;
                        compressor.Deflate(f);

                        bytesToWrite = outputSize - compressor.AvailableBytesOut;
                        if (bytesToWrite > 0)
                            ms.Write(output, 0, bytesToWrite);
                    }
                    while ((f == FlushType.None && (compressor.AvailableBytesIn != 0 || compressor.AvailableBytesOut == 0)) ||
                           (f == FlushType.Finish && bytesToWrite != 0));
                }

                compressor.EndDeflate();

                ms.Flush();
                return ms.ToArray();
            }
        }

        public static byte[] ToByteArray(Stream stream)
        {
            if (stream == null || !stream.CanRead)
            {
                return null;
            }

            // convert stream to memorystream
            MemoryStream memoryStream = new MemoryStream();
            byte[] buffer = new byte[16 * 1024];
            int read;

            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                memoryStream.Write(buffer, 0, read);
            }

            return memoryStream.ToArray();
        }
    }
}
