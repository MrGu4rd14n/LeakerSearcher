﻿/**********************************************************************************************************************************
 *     __                       __                         ____                                  __                               *
 *    /\ \                     /\ \                       /\  _`\                               /\ \                              *
 *    \ \ \         __     __  \ \ \/'\      __   _ __    \ \,\L\_\     __     __     _ __   ___\ \ \___      __   _ __           *
 *     \ \ \  __  /'__`\ /'__`\ \ \ , <    /'__`\/\`'__\   \/_\__ \   /'__`\ /'__`\  /\`'__\/'___\ \  _ `\  /'__`\/\`'__\         *
 *      \ \ \L\ \/\  __//\ \L\.\_\ \ \\`\ /\  __/\ \ \/      /\ \L\ \/\  __//\ \L\.\_\ \ \//\ \__/\ \ \ \ \/\  __/\ \ \/          *
 *       \ \____/\ \____\ \__/.\_\\ \_\ \_\ \____\\ \_\      \ `\____\ \____\ \__/.\_\\ \_\\ \____\\ \_\ \_\ \____\\ \_\          *
 *        \/___/  \/____/\/__/\/_/ \/_/\/_/\/____/ \/_/       \/_____/\/____/\/__/\/_/ \/_/ \/____/ \/_/\/_/\/____/ \/_/          *
 *                                                                                                                                *
 *    Advanced Leaker Searcher in files...                                                                                        *
 *    Coded By Virgula in December 2017                                                                                           *
 *    For more information visit the project page: https://github.com/Virgula0/LeakerSearcher                                     *
 *    Suggestions and bugs: https://github.com/Virgula0/LeakerSearcher/issues                                                     *
 *    Coded in c# from Italy with ❤️                                                                                                                            *
 **********************************************************************************************************************************
 */
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Searcher
{
    class MainClass
    {
        public static ConsoleColor originalColor = Console.ForegroundColor;
        public static ConsoleColor red = ConsoleColor.Red;
        public static ConsoleColor black = ConsoleColor.Black;
        public static ConsoleColor yellow = ConsoleColor.Yellow;
        public static ConsoleColor green = ConsoleColor.Green;
        public static ConsoleColor blue = ConsoleColor.Blue;
        public static ConsoleColor cyan = ConsoleColor.Cyan;
        public static ConsoleColor white = ConsoleColor.White;
        public static ConsoleColor magenta = ConsoleColor.Magenta;
        public static ConsoleColor grey = ConsoleColor.Gray;
        public static ConsoleColor dark_green = ConsoleColor.DarkGreen;

        public static int y = 1, iteraction_public = 0;
        public static bool hasAction = false;
        public static string[] found = new string[1000];
        public static DateTime today = DateTime.Now;
        public static string dirr = Directory.GetCurrentDirectory().ToString();
        public static string di = (dirr + $@"\Results\Results of {today:dd-MM-yy} at {today:HH_mm_ss_ms tt}.txt");

        //THE SEARCH FUNCTION ENGINE

        static public string ContainsGroup(string file, string to_search, int f_count)
        {
            try
            {

                using (var reader = new StreamReader(file))
                {
                    iteraction_public++;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        if (line.Contains(to_search))
                        {
                            y++;
                            found[0] = ($"{y - 1} matches");
                            found[y] = "\n\t" + line + "\tin " + file;
                            hasAction = true;
                        }

                        
                        if (hasAction && reader.EndOfStream && iteraction_public == f_count)
                        {
                            if (!Directory.Exists("Results"))
                                Directory.CreateDirectory("Results");
                            File.WriteAllLines(di, found);      //Create export file
                            return string.Join("", found);
                        }
                    }
                    return "Not Found";
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = red;
                Console.WriteLine("Exception Occurred... Stopping. Are you sure is the correct patch directory?\n{0}", ex);
                return "Not Found";
            }

        }

        // FUNCTION FOR OPTION 3

        static void DirSearch(string sDir, string to_search, string ext, string graphic)
        {
            int numbers = 0, fCount, l = 0, j = 0;
            string extension, contained, tr = "Not Found";
            string type = "*" + ext;

            DirectoryInfo directory = new DirectoryInfo(sDir);
            FileInfo[] files = directory.GetFiles(type, SearchOption.TopDirectoryOnly);

            var filtered = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));

            foreach (var f in filtered)
            {
                numbers++;
            }

            fCount = numbers;

            var watch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    extension = Path.GetExtension(f);
                    bool isHidden = (File.GetAttributes(f) & FileAttributes.Hidden) == FileAttributes.Hidden;

                    if (extension == ext && isHidden == false)
                    {
                        j++;
                        contained = ContainsGroup(f, to_search, fCount);
                        double dProgress = ((double)j / fCount) * 100.0;
                        if (tr.Equals(contained))
                        {
                            if (graphic == "y")
                            {
                                style(to_search, sDir, f, (int)dProgress, 0, null, null);
                            }
                            else
                            {
                                if (hasAction == true && l == 0)
                                {
                                    l = 1;
                                    Console.ForegroundColor = green;
                                    Console.WriteLine("Found something! \t|Please wait until the finish!|");
                                }
                                else
                                    if (hasAction == false && l == 0)
                                {
                                    Console.WriteLine("Not found in {0} \t| Progress: {1:N2}%", Path.GetFileName(f), dProgress);
                                }

                                if (hasAction == true && l == 1)
                                {
                                    Console.ForegroundColor = originalColor;
                                    Console.WriteLine("Scanned: {0} \t| Progress: {1:N2}%", Path.GetFileName(f), dProgress);
                                }
                            }
                        }
                        else
                        {
                            if (graphic == "y")
                            {
                                watch.Stop();
                                var elapsed = watch.Elapsed.TotalMinutes;
                                style(to_search, sDir, f, (int)dProgress, 1, contained, Convert.ToString(elapsed));
                                Console.WriteLine("\nType a key to exit");
                            }
                            else
                            {
                                watch.Stop();
                                var elapsed = watch.Elapsed.TotalMinutes;
                                Console.ForegroundColor = green;
                                Console.WriteLine("Found! "); Console.ForegroundColor = magenta;
                                Console.WriteLine("Results: {0}", contained); Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.WriteLine("Elapsed time: {0:N1} minutes", elapsed); Console.ForegroundColor = originalColor;
                                Console.WriteLine("\nType a key to exit");
                            }
                            Console.ReadLine();
                            System.Environment.Exit(0);
                        }
                    }
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        // FUNCTION FOR OPTION 4

        static void DirSearch_2(string sDir, string to_search, string ext, string graphic)
        {
            int fCount, numbers = 0, l = 0, j = 0;
            string extension, contained, tr = "Not Found";
            string type = "*" + ext;

            DirectoryInfo directory = new DirectoryInfo(sDir);
            FileInfo[] files = directory.GetFiles(type, SearchOption.AllDirectories);

            var filtered = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));

            foreach (var f in filtered)
            {
                numbers++;
            }


            fCount = numbers;

            //fCount = Directory.GetFiles(sDir, type, SearchOption.AllDirectories).Length;

            var watch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        extension = Path.GetExtension(f);
                        bool isHidden = (File.GetAttributes(f) & FileAttributes.Hidden) == FileAttributes.Hidden;
                        if (extension == ext && isHidden == false)
                        {
                            j++;
                            contained = ContainsGroup(f, to_search, fCount);
                            double dProgress = ((double)j / (double)fCount) * 100.0;
                            if (tr.Equals(contained))
                                if (graphic == "y")
                                {
                                    style(to_search, sDir, f, (int)dProgress, 0, null, null);
                                }
                                else
                                {
                                    if (hasAction == true && l == 0)
                                    {
                                        l = 1;
                                        Console.ForegroundColor = green;
                                        Console.WriteLine("Found something! \t|Please wait until the finish!|");
                                    }
                                    else
                                    {
                                        if (hasAction == false && l == 0)
                                        {
                                            Console.WriteLine("Not found in {0} \t| Progress: {1:N2}%", Path.GetFileName(f), dProgress);
                                        }

                                        if (hasAction == true && l == 1)
                                        {
                                            Console.ForegroundColor = originalColor;
                                            Console.WriteLine("Scanned: {0} \t| Progress: {1:N2}%", Path.GetFileName(f), dProgress);
                                        }
                                    }
                                }
                            else
                            {
                                if (graphic == "y")
                                {
                                    watch.Stop();
                                    var elapsed = watch.Elapsed.TotalMinutes;
                                    style(to_search, sDir, f, (int)dProgress, 1, contained, Convert.ToString(elapsed));
                                    Console.WriteLine("\nType a key to exit");
                                }
                                else
                                {
                                    watch.Stop();
                                    var elapsed = watch.Elapsed.TotalMinutes;
                                    Console.ForegroundColor = green;
                                    Console.WriteLine("Found!"); Console.ForegroundColor = magenta;
                                    Console.WriteLine("Results: {0}", contained); Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine("Elapsed time: {0:N1} minutes", elapsed); Console.ForegroundColor = originalColor;
                                    Console.WriteLine("\nType a key to exit");
                                }
                                Console.ReadLine();
                                System.Environment.Exit(0);
                            }
                        }
                    }
                    DirSearch_2(d, to_search, ext, graphic);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        // GRAPHICAL FUNCTION TO WRITE STATUS AND RESULTS

        public static void style(string str, string path_in, string path, int progress, int find, string result, string elapsed)
        {
            if (find != 1 && find != 2 && find != 10)
            {
                if (hasAction == false)
                {
                    Console.Clear();
                    initial();
                    Console.WriteLine("\t------------------------------------------------------------------");
                    Console.ForegroundColor = originalColor;
                    Console.Write("\tString to search: "); Console.ForegroundColor = blue; Console.Write($"{str}"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                    Console.Write("\tSearching in: "); Console.Write($"{path_in}"); Console.ForegroundColor = yellow; Console.WriteLine(); Console.ForegroundColor = originalColor;
                    Console.Write("\tNot Found in: "); Console.ForegroundColor = red; Console.Write($"{path}"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                    Console.Write("\tStatus:"); Console.ForegroundColor = yellow; Console.Write(" Searching..."); Console.WriteLine(); Console.ForegroundColor = originalColor;
                    Console.Write("\tProgress: "); Console.ForegroundColor = magenta; Console.Write($"{progress}%"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                    Console.Write("\t------------------------------------------------------------------");
                }
                else
                {
                    Console.Clear();
                    initial();
                    Console.WriteLine("\t------------------------------------------------------------------");
                    Console.ForegroundColor = originalColor;
                    Console.Write("\tString to search: "); Console.ForegroundColor = blue; Console.Write($"{str}"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                    Console.Write("\tSearching in: "); Console.Write($"{path_in}"); Console.ForegroundColor = yellow; Console.WriteLine(); Console.ForegroundColor = green;
                    Console.Write("\tFound something! Please wait until scan finish!!"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                    Console.Write("\tScanned: "); Console.ForegroundColor = magenta; Console.Write($"{path}"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                    Console.Write("\tStatus:"); Console.ForegroundColor = yellow; Console.Write(" Searching other strings..."); Console.WriteLine(); Console.ForegroundColor = originalColor;
                    Console.Write("\tProgress: "); Console.ForegroundColor = magenta; Console.Write($"{progress}%"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                    Console.Write("\t------------------------------------------------------------------");
                }

            }

            if (find == 1)
            {
                Console.Clear();
                initial();
                Console.WriteLine("\t------------------------------------------------------------------");
                Console.ForegroundColor = originalColor;
                Console.Write("\tString searched: "); Console.ForegroundColor = blue; Console.Write($"{str}"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                Console.Write("\tStatus:"); Console.ForegroundColor = green; Console.Write(" Found!..."); Console.WriteLine(); Console.ForegroundColor = originalColor;
                Console.Write("\tSearched in "); Console.ForegroundColor = green; Console.Write($"{path_in}"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                Console.Write("\tResults: "); Console.ForegroundColor = magenta; Console.Write($"{result}"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                Console.Write("\tExported: "); Console.ForegroundColor = originalColor; Console.Write("A file was created with current date and time in 'Results' dir"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                Console.Write("\tElapsed Time: "); Console.ForegroundColor = grey; Console.Write($"{elapsed} min"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                Console.WriteLine("\t------------------------------------------------------------------");
            }

            if (find == 10)
            {
                Console.Clear();
                initial();
                Console.WriteLine("\t------------------------------------------------------------------");
                Console.ForegroundColor = originalColor;
                Console.Write("\tString searched: "); Console.ForegroundColor = grey; Console.Write($"{str}"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                Console.Write("\tStatus:"); Console.ForegroundColor = magenta; Console.Write(" Finished..."); Console.WriteLine(); Console.ForegroundColor = originalColor;
                Console.Write("\tResult: "); Console.ForegroundColor = red; Console.Write("String searched seems not be in the leak/leaks"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                Console.Write("\t------------------------------------------------------------------");
            }

            if (find == 2)
            {
                Console.Clear();
                initial();
                Console.WriteLine("\t------------------------------------------------------------------");
                Console.ForegroundColor = originalColor;
                Console.Write("\tString to search: "); Console.ForegroundColor = blue; Console.Write($"{str}"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                Console.Write("\tSearching in: "); Console.Write($"{path_in}"); Console.ForegroundColor = yellow; Console.WriteLine(); Console.ForegroundColor = originalColor;
                Console.Write("\tStatus:"); Console.ForegroundColor = yellow; Console.Write(" Searching... Please Wait"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                Console.Write("\tCounted Lines:  "); Console.ForegroundColor = magenta; Console.Write($"{progress}"); Console.WriteLine(); Console.ForegroundColor = originalColor;
                Console.Write("\t------------------------------------------------------------------");
            }

        }

        // INTIAL GRAPHICAL FUNCTION
        public static void initial()
        {
            Console.Title = "L.S Advanced Quick Strings Files Finder";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            int origWidth, width;
            int origHeight, height;
            origHeight = Console.WindowHeight;
            origWidth = Console.WindowWidth;
            width = 120;
            height = 30;
            Console.SetWindowSize(width, height);

            Console.ForegroundColor = dark_green;
            string gg = "" +
                  "\t\t+-------------------------------------------------------------------------------+\n" +
                  "\t\t|  _                _                _____                     _                |\n" +
                  "\t\t| | |              | |              / ____|                   | |               |\n" +
                  "\t\t| | |     ___  ____| | _____ ____  | (___   ___  __ _ _ __ ___| |__   ___ ____  |\n" +
                  "\t\t| | |    / _  / _  | |/ / _    __|  |___ | / _  / __ | ___/ __|  _   / _ |  __| |\n" +
                  "\t\t| | |___|  __/ (_| | <  | __/ |     ____) |  __/ (_| | | | (__| | | |  __/ |    |\n" +
                  "\t\t| |______ ___|_____|_|__/___|_|    |_____/ \\___| ____|_|  \\___|_| |_| ___|_|    |\n" +
                  "\t\t|                                                                               |\n" +
                  "\t\t| Quick and simple alghorithm to search in leaks very fast mode                 |\n" +
                  "\t\t| Coded By Virgula @2017                                                        |\n" +
                  "\t\t| Git: https://github.com/Virgula0/LeakerSearcher                               |\n" +
                  "\t\t| Version: {0.2#Beta}                                                           |\n" +
                  "\t\t+-------------------------------------------------------------------------------+\n";

            Console.WriteLine(gg);
            Console.ForegroundColor = originalColor;
        }

        // MAIN FUNCTION 
        public static void Main(string[] args)
        {

            initial();
            string to_search, contained, path, path_2, graphic, extension;

            string tr = "Not Found", line;

            int choose, n, counter = 0;

            Console.ForegroundColor = cyan;
            Console.WriteLine();
            Console.WriteLine("+-----------------------------------------------------------------------------+");
            Console.WriteLine("|(1) Search in a single file.                                                 |");
            Console.WriteLine("|(2) Search in more files (Example: from 1.txt to n.txt.)                     |");
            Console.WriteLine("|(3) Search in all .ext files in a single path.                               |");
            Console.WriteLine("|(4) Search in all .ext files searching also in subdirectories if there are.  |");
            Console.WriteLine("+-----------------------------------------------------------------------------+");
            Console.ForegroundColor = originalColor;

            do
            {
                Console.Write("Your choose-> ");
                Console.ForegroundColor = ConsoleColor.DarkYellow; choose = Convert.ToInt32(Console.ReadLine()); Console.ForegroundColor = originalColor;
            } while (choose != 1 && choose != 2 && choose != 3 && choose != 4);

            do
            {
                Console.Write("Graphic mode? (y/n)");
                Console.ForegroundColor = ConsoleColor.DarkYellow; graphic = Console.ReadLine().ToLower(); Console.ForegroundColor = originalColor;
            } while (graphic != "y" && graphic != "n");


            do
            {
                Console.WriteLine("Insert string to search: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow; to_search = Console.ReadLine().ToLower().Trim(); Console.ForegroundColor = originalColor;
            } while (to_search == "");


            Console.WriteLine();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var watch_2 = System.Diagnostics.Stopwatch.StartNew();
            string attributes;

            if (choose == 1)
            {
                do
                {
                    do
                    {
                        Console.WriteLine("Insert directory patch + file name (ex C:\\Users\\name\\Desktop\\dir\\file.txt)");
                        Console.ForegroundColor = ConsoleColor.DarkYellow; path = Console.ReadLine(); Console.ForegroundColor = originalColor;
                    } while (path == "");

                    try
                    {
                        attributes = Convert.ToString(Path.GetExtension(path));
                    }
                    catch (Exception)
                    {
                        attributes = "Null";
                    }

                    if (!File.Exists(path))
                    {
                        Console.ForegroundColor = red; Console.WriteLine("\nYour file does not exists. Try Again setting correctly the path dir\n");
                        Console.ForegroundColor = originalColor;
                    }

                    if (attributes != ".txt" && attributes != ".csv" && attributes != ".sql" && attributes != "Null")
                    {
                        Console.ForegroundColor = red; Console.WriteLine("\nExtension not valid try again, only .csv .txt or .sql\n");
                    }


                } while (!File.Exists(path) || (attributes != ".txt" && attributes != ".csv" && attributes != ".sql"));

                if (graphic == "y")
                {
                    StreamReader file = new StreamReader(path);

                    Console.ForegroundColor = ConsoleColor.DarkGray; Console.WriteLine("Please wait... I'm reading the file size...\nThis could take a while if the file is some GBs"); Console.ForegroundColor = originalColor;
                    while ((line = file.ReadLine()) != null)
                    {
                        counter++;
                    }
                }

                var watch_for_this = System.Diagnostics.Stopwatch.StartNew();

                if (graphic == "y")
                    style(to_search, path, path, Convert.ToInt32(counter), 2, null, null);
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue; Console.WriteLine($"Searching in {path}"); Console.ForegroundColor = originalColor;
                }

                contained = ContainsGroup(path, to_search, 1);

                if (!tr.Equals(contained))
                {
                    if (graphic == "y")
                    {
                        watch_for_this.Stop();
                        var elapsed = watch_for_this.Elapsed.TotalMinutes;
                        style(to_search, path, path, Convert.ToInt32(counter), 1, contained, Convert.ToString(elapsed));
                        Console.WriteLine("\nType a key To exit");
                    }
                    else
                    {
                        watch_for_this.Stop();
                        var elapsed = watch_for_this.Elapsed.TotalMinutes;
                        Console.ForegroundColor = green; Console.WriteLine("Found! in {0}\n", path); Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Results: {0}", contained); Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Elapsed Time {0} min", elapsed); Console.ForegroundColor = originalColor;
                        Console.WriteLine("A file was created with current date and time in 'Results' dir");
                        Console.WriteLine("\nType a key To exit");
                    }
                    Console.ReadLine();
                    System.Environment.Exit(0);
                }
            }
            else if (choose == 2)
            {
                int l = 0;

                do
                {
                    do
                    {
                        Console.WriteLine("Insert directory patch-> (Example: C:\\Users\\name\\Desktop\\dir):");
                        Console.ForegroundColor = ConsoleColor.DarkYellow; path = Console.ReadLine(); Console.ForegroundColor = originalColor;
                    } while (path == "");

                    if (!Directory.Exists(path))
                    {
                        Console.ForegroundColor = red; Console.WriteLine("\nYour directory does not exists. Try Again setting correctly the path dir\n");
                        Console.ForegroundColor = originalColor;
                    }
                } while (!Directory.Exists(path));

                do
                {
                    Console.Write("Extension of files [M.B->.txt .csv .sql]->");
                    Console.ForegroundColor = ConsoleColor.DarkYellow; extension = Console.ReadLine().ToLower(); Console.ForegroundColor = originalColor;

                    if (extension != ".txt" && extension != ".csv" && extension != ".sql")
                    {
                        Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Accepted only .txt or .csv or .sql"); Console.ForegroundColor = originalColor;
                    }
                } while (extension != ".txt" && extension != ".csv" && extension != ".sql");

                do
                {
                    Console.Write("How many files to search in?: ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow; n = Convert.ToInt32(Console.ReadLine()); Console.ForegroundColor = originalColor;
                } while (n <= 0);

                Console.ForegroundColor = green;
                Console.WriteLine("\n\nStarted... Please wait for verbose...");
                Console.ForegroundColor = originalColor;

                for (int i = 1; i <= n; i++)
                {
                    path_2 = path + "/" + Convert.ToString(i) + extension;
                    contained = ContainsGroup(path_2, to_search, n);
                    double dProgress = ((double)i / n) * 100.0;
                    if (tr.Equals(contained))
                    {
                        if (graphic == "y")
                            style(to_search, path, path_2, (int)dProgress, 0, null, null);
                        else
                        {
                            if (hasAction == true && l == 0)
                            {
                                l = 1;
                                Console.ForegroundColor = green;
                                Console.WriteLine("Found something! \t|Please wait until the finish!|");
                            }
                            else
                            {
                                if (hasAction == false && l == 0)
                                {
                                    Console.WriteLine("Not found in {0}.txt \t| Progress: {1:N2}%", i, dProgress);
                                }

                                if (hasAction == true && l == 1)
                                {
                                    Console.ForegroundColor = originalColor;
                                    Console.WriteLine("Scanned: {0}.txt \t| Progress: {1:N2}%", i, dProgress);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (graphic == "y")
                        {
                            watch.Stop();
                            var elapsed = watch.Elapsed.TotalMinutes;
                            style(to_search, path, path_2, (int)dProgress, 1, contained, Convert.ToString(elapsed));
                            Console.WriteLine("Type a key to exit...\n");
                        }
                        else
                        {
                            watch.Stop();
                            var elapsed = watch.Elapsed.TotalMinutes;
                            Console.ForegroundColor = green;
                            Console.WriteLine("Found! "); Console.ForegroundColor = magenta;
                            Console.WriteLine("Results: {0}", contained); Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("A file was created with current date and time in 'Results' dir");
                            Console.WriteLine("Elapsed Time {0} min", elapsed); Console.ForegroundColor = originalColor;
                            Console.WriteLine("Type a key to exit...\n");
                        }
                        Console.ReadLine();
                        System.Environment.Exit(0);
                    }
                }
            }
            else if (choose == 3)
            {

                do
                {
                    do
                    {
                        Console.WriteLine("Insert directory patch (Example C:\\Users\\name\\Desktop\\dir):");
                        Console.ForegroundColor = ConsoleColor.DarkYellow; path = Console.ReadLine(); Console.ForegroundColor = originalColor;
                    } while (path == "");

                    if (!Directory.Exists(path))
                    {
                        Console.ForegroundColor = red; Console.WriteLine("\nYour directory does not exists. Try Again setting correctly the path dir\n");
                        Console.ForegroundColor = originalColor;
                    }
                } while (!Directory.Exists(path));

                do
                {
                    Console.Write("Extension of files [M.B->.txt .csv .sql]->");
                    Console.ForegroundColor = ConsoleColor.DarkYellow; extension = Console.ReadLine().ToLower(); Console.ForegroundColor = originalColor;

                    if (extension != ".txt" && extension != ".csv" && extension != ".sql")
                    {
                        Console.ForegroundColor = red; Console.WriteLine("Accepted only .txt .csv or .sql types"); Console.ForegroundColor = originalColor;
                    }
                } while (extension != ".txt" && extension != ".csv" && extension != ".sql");

                Console.ForegroundColor = green;
                Console.WriteLine("\n\nStarted... Please wait for verbose...");
                Console.ForegroundColor = originalColor;

                DirSearch(path, to_search, extension, graphic);

            }
            else if (choose == 4)
            {
                do
                {
                    do
                    {
                        Console.WriteLine("Insert directory patch (Example C:\\Users\\name\\Desktop\\dir):");
                        Console.ForegroundColor = ConsoleColor.DarkYellow; path = Console.ReadLine(); Console.ForegroundColor = originalColor;
                    } while (path == "");

                    if (!Directory.Exists(path))
                    {
                        Console.ForegroundColor = red; Console.WriteLine("\nYour directory does not exists. Try Again setting correctly the path dir\n");
                    }
                } while (!Directory.Exists(path));

                do
                {
                    Console.Write("Extension of files [M.B->.txt .csv .sql]->");
                    Console.ForegroundColor = ConsoleColor.DarkYellow; extension = Console.ReadLine().ToLower(); Console.ForegroundColor = originalColor;

                    if (extension != ".txt" && extension != ".csv" && extension != ".sql")
                    {
                        Console.ForegroundColor = red; Console.WriteLine("Accepted only .txt or .csv or .sql types"); Console.ForegroundColor = originalColor;
                    }

                } while (extension != ".txt" && extension != ".csv" && extension != ".sql");

                DirSearch_2(path, to_search, extension, graphic);
            }
            else
            {
                Console.ForegroundColor = red;
                Console.WriteLine("Command not found!");
                Console.ReadLine();
                System.Environment.Exit(0);
            }

            watch_2.Stop();
            var elapsed_2 = watch_2.Elapsed.TotalMinutes;
            if (graphic == "y")
                style(to_search, null, null, 0, 10, null, Convert.ToString(elapsed_2));
            else
            {
                Console.ForegroundColor = red;
                Console.Write("String seems not to be in leak/s");
                Console.ForegroundColor = originalColor;
                Console.WriteLine("\nType a key to exit\n");
            }

            Console.ReadLine();
            System.Environment.Exit(0);
        }
    }
}