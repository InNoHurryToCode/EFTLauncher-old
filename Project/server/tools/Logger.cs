using System;
using System.IO;

namespace EFTServer.server.tools
{
    public static class Logger
    {
        private static string filePath; // log file location
        private static string fileName; // log file name

        public static void SetFilePath(string path)
        {
            filePath = path;
        }

        public static void SetFileName(string name)
        {
            fileName = name;
        }

        public static void Log(string text)
        {
            // get file
            string file = filePath + fileName + ".log";

            // write the text to the log
            using (StreamWriter sw = new StreamWriter(File.Open(file, FileMode.Append)))
            {
                sw.WriteLine(text);
            }

            // show logged text in the console
            Console.WriteLine(text);
        }
    }
}