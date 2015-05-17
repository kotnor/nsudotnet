using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mikhailov.Nsudotnet.LinesCounter
{
    class Program
    {
        private static string _extension;
        private static string _path;
        private static int _countLines = 0;
        private static int _allCountLines = 0;
        private static bool _inComment = false;
        private static void PrintHelp()
        {
            Console.WriteLine("First param - extension. Example: *.cs\n" +
                              "Second param - path. If no - current directory");
        }

        private static bool IsComment(string line)
        {

            if (line.StartsWith("//"))
                return true;
            if (_inComment)
            {

                if (line.Contains("*/"))
                {
                    _inComment = false;
                    return (line.EndsWith("*/"));
                }
                else return true;
            }
            if (line.Contains("/*"))
            {

                if (!line.Contains("*/"))
                    _inComment = true;
                return (line.StartsWith("/*"));
            }
            return false;
        }
        static void Main(string[] args)
        {
            switch (args.Length)
            {
                case 1:
                    _extension = args[0];
                    _path = Directory.GetCurrentDirectory();
                    break;
                case 2:
                    _extension = args[0];
                    _path = args[1];
                    break;
                default:
                    PrintHelp();
                    return;
            }
            // Console.ReadLine();
            string[] fileNames = Directory.GetFiles(_path, _extension, SearchOption.AllDirectories);
            foreach (var fileName in fileNames)
            {
                _inComment = false;
                using (var file = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read)))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        if (IsComment(line) || line == "")
                            continue;
                        _countLines++;
                    }
                    _allCountLines += _countLines;
                    Console.WriteLine("In File {0} : {1} lines", fileName, _countLines);
                    _countLines = 0;
                }
            }
            Console.WriteLine("All files have {0} lines", _allCountLines);
            //Con

        }
    }
}
