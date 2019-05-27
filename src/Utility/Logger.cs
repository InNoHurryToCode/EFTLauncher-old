using System;
using System.IO;

namespace EFTLauncher.Utility
{
    public static class Logger
    {
        private static string filePath;                 // log file location
        private static string fileName;                 // log file name
        public static string log { get; private set; }  // logged text

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
            // store text in log
            log += text + Environment.NewLine;

            // write the text to the file
            string file = filePath + fileName + ".log";
            using (StreamWriter sw = new StreamWriter(File.Open(file, FileMode.Append)))
            {
                sw.WriteLine(text);
            }

            // show logged text in the console
            Console.WriteLine(text);
        }

        public static void DeleteAllLogs()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);

            // delete all files
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            // delete all subdirectories
            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                dir.Delete(true);
            }

            // create new log and inform of deletion
            Log("ALERT: Deleted all log files");
        }
    }
}