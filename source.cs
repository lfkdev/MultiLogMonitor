using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace LogMonitor
{
    class Program
    {
        static bool Running = true;
        static bool completedRound = false;
        static int ConsolWidth = Console.WindowWidth;
        static int ConsolHeight = Console.WindowHeight;
        static int LineCount = 6;
        static int UpdateTime = 200; // millisecond
        static string startCommand;
        static string OS;
        static readonly string ver = "2.5";
        static List<OutPutField> felder;

        
        public class OutPutField
        {
            public string LogFile { get; set; }
            public string LogName { get; set; }
            public long LogSize { get; set; }
            public int Number { get; set; }
            public int CursorPosition { get; set; }
        }

        static void Main(string[] args)
        {
            Console.CancelKeyPress += delegate {
                while (completedRound == false)
                {
                    Thread.Sleep(50);
                }
                Cleanup();
            };

            try {
                Console.SetCursorPosition(0, -1);
                startCommand = Console.ReadLine();
                if (File.Exists("/bin/bash"))
                {
                    OS = "Linux";
                }
            } catch (Exception) { }

            Console.CursorVisible = false;

            Console.Clear();

            felder = new List<OutPutField>();

            int i = 0;
            int curpos = Console.CursorTop+3;
            foreach (string arg in args)
            {
                if (arg == "-h" || arg == "-H")
                {
                    ShowHelp();
                    Exit(1);
                }
                if (arg == "--about")
                {
                    ShowAbout();
                    Exit(0, "");
                }
                if (!File.Exists(arg))
                {
                    Console.WriteLine("File {0} does not exist!", arg);
                    Exit(1, "");
                }

                string LogFile = Path.GetFileName(arg);
                long size = new FileInfo(arg).Length;
                felder.Add(new OutPutField { LogFile = LogFile, LogName = arg, CursorPosition = curpos, Number = i, LogSize = size });
                curpos += LineCount+2;
                i++;
            }

            if (felder.Count == 0)
            {
                ExitFirst(1, "No log parameter given. Use -h for help.");
            }

            if (((LineCount+2) * felder.Count)+1 >= Console.WindowHeight-2)
            {
                Console.WriteLine("Your terminal window is too small for looking at {0} logs at the same time!\n(Needed {1} rows but got {2} rows.)", felder.Count, (LineCount + 2) * felder.Count, Console.WindowHeight-3);
                ExitFirst(1, "");
            }

            ShowTitle();
            CreateField(felder.Count);

            foreach (var feld in felder)
            {
                RefreshField(feld.LogName, feld.CursorPosition);
            }

            while (Running)
            {
                RefreshFieldsIfNeeded();
                ShowTitle();
                
                Thread.Sleep(UpdateTime);
            }
        }

        private static void ShowAbout()
        {
            Console.WriteLine("\nVersion: {0}\n\nAuthor: Leon Felix Klostermann (c) 2019\nE-Mail: <l.klostermann@protonmail.ch>\nGitHub: github.com/lfkdev\n", ver);
        }

        private static void ShowHelp()
        {
            Console.WriteLine("\nINFO:\nAdd as many logs as you want.\n The more logs are added the more space is needed in the terminal.\n");
            Console.WriteLine("Example usage:");
            Console.WriteLine(" mlogm /tmp/error.log /var/log/access.log");
            Console.WriteLine(" mlogm --about\n");
        }

        private static void RefreshFieldsIfNeeded()
        {
            foreach (var feld in felder)
            {
                long NewSize = new FileInfo(feld.LogName).Length;
                if (NewSize != feld.LogSize)
                {
                    //RedrawLayoutIfNeeded();
                    RefreshField(feld.LogName, feld.CursorPosition);
                }
                feld.LogSize = NewSize;
            }
        }

        private static void RedrawLayoutIfNeeded()
        {
            if (ConsolWidth != Console.WindowWidth)
            {
                foreach (var feld in felder)
                {
                    Console.SetCursorPosition(0, feld.CursorPosition - 1);
                    DrawLine();
                    Console.SetCursorPosition(0, feld.CursorPosition - 1);
                    Console.WriteLine(" ╔═ Log: {0} ", feld);
                    Console.SetCursorPosition(0, feld.CursorPosition + LineCount);
                    DrawLine("down");
                }
                ConsolWidth = Console.WindowWidth;
            }
        }

        private static void RefreshField(string logfilePfad, int curpos)
        {
            completedRound = false;
            try
                {
                FileStream fs = new FileStream(logfilePfad, FileMode.Open, FileAccess.Read);
                fs.Seek(Math.Max(-1024, -fs.Length), SeekOrigin.End);
                byte[] bytes = new byte[1024];
                fs.Read(bytes, 0, 1024);
                string s = Encoding.Default.GetString(bytes);
                //string result = s.Substring(Math.Max(0, s.Length - 400));

                List<string> result = TakeLastLines(s, LineCount+1);
                result.RemoveAt(result.Count - 1); // remove newline
                List<string> result_cuttet = new List<string>();
                    
                foreach (string line in result)
                {
                    if (line != String.Empty)
                    {
                        if (line.Length > ConsolWidth - 6)
                        {
                            string newline = line.Substring(0, ConsolWidth - 6);
                            result_cuttet.Add(" ║ " + newline + Spaces((ConsolWidth - 6) - newline.Length) + " ║");
                        }
                        else
                        {
                            result_cuttet.Add(" ║ " + line + Spaces((ConsolWidth - 6) - line.Length) + " ║");
                        }
                    }
                }

                while (result_cuttet.Count != LineCount)
                {
                    result_cuttet.Add(" ║ "+Spaces((ConsolWidth - 6)) + " ║");
                }


                    
                Console.SetCursorPosition(0, curpos);

                for (int i = 0; i < result_cuttet.Count; i++)
                {
                    Char[] array = result_cuttet[i].ToCharArray();

                    foreach (Char c in array)
                    {
                        if (c == '[' || c == ']')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(c);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(c);
                        }
                    }
                    Console.WriteLine();
                }

                if (ConsolWidth != Console.WindowWidth)
                {
                    DrawLine("down");
                        
                }

                Console.SetCursorPosition(0, Console.CursorTop);
                //ConsolWidth = Console.WindowWidth;
            }
            catch (UnauthorizedAccessException) { Console.WriteLine("No permissions to read file! :( ({0})", logfilePfad); Exit(1); }
            //catch (Exception) { Console.WriteLine("Unknown error occured! (ER: RF)"); Exit(1); }

            if (((LineCount + 2) * felder.Count) + 1 >= Console.WindowHeight - 2)
            {
                Console.WriteLine("Increase your terminal Y Size for looking at {0} logs at the same time!\n(Needed {1} rows but got {2} rows.)", felder.Count, (LineCount + 2) * felder.Count, Console.WindowHeight - 3);
                ExitFirst(1);
            }

            // reset value
            

            completedRound = true;
        }



        private static List<string> TakeLastLines(string text, int count)
        {
            List<string> lines = new List<string>();
            Match match = Regex.Match(text, "^.*$", RegexOptions.Multiline | RegexOptions.RightToLeft);
            while (match.Success && lines.Count < count)
            {
                lines.Insert(0, match.Value);
                match = match.NextMatch();
            }
            return lines;
        }

        private static void CreateField(int howmuch)
        {
            foreach (var field in felder)
            {
                Console.SetCursorPosition(0, field.CursorPosition -1);
                DrawLine();
                Console.SetCursorPosition(0, field.CursorPosition -1);
                Console.WriteLine(" ╔═ Log: {0} ", field.LogFile);
                Console.SetCursorPosition(0, field.CursorPosition);
                Console.Write(NewLine(LineCount));
                DrawLine("down");
            }
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        private static void ShowTitle()
        {
            Console.SetCursorPosition(0, 0);
            string TITLE = " MULTI-LOG MONITOR ";
            int StyleSpace = (ConsolWidth - TITLE.Length) / 2;
            //Console.WriteLine(Lines(StyleSpace) + TITLE + Lines(StyleSpace));
            CenterText("# m-log-m #");
            Console.SetCursorPosition(0, (felder.Count*(LineCount+2))+2);
            Console.Write(" [Displaying last {0} lines of {1} log files (Refreshrate: {2}ms v. {3})]\n", LineCount, felder.Count, UpdateTime, ver);
        }

        private static void CenterText(String text)
        {
            Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2));
            Console.WriteLine(text);
        }

        static string Spaces(int n)
        {
            return new String(' ', n);
        }

        static string NewLine(int n)
        {
            return new String('\n', n);
        }

        static string Lines(int n)
        {
            return new String('#', n);
        }

        private static void DrawLine(string pos = "above")
        {
            int i = 2;
            if (pos == "above") {
                
                Console.Write(" ╔═");
                while (i < Console.WindowWidth - 3)
                {
                    Console.Write("═");
                    i++;
                }
                Console.WriteLine("╗");
            } else
            {
                Console.Write(" ╚");
                while (i < Console.WindowWidth - 3)
                {
                    Console.Write("═");
                    i++;
                }
                Console.WriteLine("═╝");
            }
        }

        private static void Exit(int errcode, string err = "Exiting now.")
        {
            Console.CursorVisible = true;
            Running = false;
            Console.SetCursorPosition(0, ((felder.Count + 2) * 8));
            Console.WriteLine(err);
            Environment.Exit(errcode);
        }

        private static void ExitFirst(int errcode, string err = "Exiting now.")
        {
            Console.CursorVisible = true;
            Running = false;
            Console.WriteLine(err);
            Environment.Exit(errcode);
        }

        private static void Cleanup()
        {
            Console.CursorVisible = true;
            Running = false;
            //Console.Clear();
            Console.SetCursorPosition(0, ((felder.Count + 2) * 8)+3);
            //ClearCurrentConsoleLine();
            Console.WriteLine("Bye!");
            Environment.Exit(0);
        }
    }
}
