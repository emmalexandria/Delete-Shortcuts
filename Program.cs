using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;


namespace DeleteShortcuts
{
    class Program
    {
        private static SearchOption searchOption;

        private static string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Desktop";



        static void Main(string[] args)
        {
            FileType[] extensions = Config.readTypes();
            string dir = GetDir();

            List<FileStruct> files = GetFiles(dir, extensions);

            DisplayFiles(files);

            //if (files.Count == 0)
            //{
            //Console.WriteLine("No valid files in this directory");
            //GetDir();
            //}
            //else
            //DisplayFiles(files);



            Environment.Exit(0);

        }

        static string GetDir()
        {
            Console.WriteLine(@"Enter directory to clear of shortcuts    (e.g C:\Users\User\Desktop)");

            string input = Console.ReadLine();


            if (input.Length > 0)
            {
                if (ValidatePath(input))
                    path = input;
                else
                    GetDir();
            }
            
            Console.WriteLine("Search all subdirectories? (Y/N)");
            string answer = Console.ReadLine().ToLower();
            if (answer == "y" || answer == "y")
            {
                searchOption = SearchOption.AllDirectories;

            }
            else if (answer == "n" || answer == "n")
            {
                searchOption = SearchOption.TopDirectoryOnly;
            }

            return path;

        }


        static bool ValidatePath(string input)
        {
            return (!String.IsNullOrEmpty(input) && Directory.Exists(input));
        }

        static List<FileStruct> GetFiles(string dir, FileType[] extensions)
        {
            var files = new List<FileStruct>();

            try
            {
                foreach (FileType file in extensions)
                {
                    string[] path = Directory.GetFiles(dir, file.extension, searchOption);
                    foreach (string s in path)
                    {
                        files.Add(new FileStruct(file.action, file.extension, s, file.moveToDir));
                    }
                }
            }
            catch (System.UnauthorizedAccessException)
            {
                Console.WriteLine(
                    "Unfortunately, directory includes read only directories or files. Cannot get files.\n");
                GetDir();
            }

            return files;
        }

        static List<FileStruct> getByExtension(string _path, string ext, Action action, string moveToPath = "")
        {
            string[] arr = Directory.GetFiles(_path, $"*.{ext}", searchOption);

            var returnList = new List<FileStruct>();
            
            foreach (string element in arr)
            {
                returnList.Add(new FileStruct(action, ext, element, moveToPath));
            }

            return returnList;
        }


        static void DisplayFiles(List<FileStruct> files)
        {

            for (int i = 0; i < files.Count; i++)
            {
                FileStruct cFile = files[i];
                Console.WriteLine($"{i}.) {cFile.Dir}      [{cFile.FileAction.ToString()}]");
            }
            
            Console.WriteLine("If you do not want to delete any files, enter their index and they will be removed!");
            Console.WriteLine("Delete files?  (Y/N)");

            string deleteS = Console.ReadLine();
            if (!Int32.TryParse(deleteS, out var deleteI))
            {
                if (deleteS == "Y")
                {
                    Console.WriteLine("\nDeleting Now");

                }
                else if (deleteS == "N")
                {
                    GetDir();
                }
                else
                {
                    Console.WriteLine("Not a valid input");
                    DisplayFiles(files);
                }
            }
            else
            {
                ParseRemoval(deleteI, files);
            }
            
            
        }
        static void ActOnFiles(List<FileStruct> files)
        {
            foreach (FileStruct current in files)
            {
                File.Delete(current.Dir);
                Console.WriteLine($"File {current} deleted successfully!");
            }
        }

        static void ParseRemoval(int deleteI, List<FileStruct> files)
        {
            try
            {
                deleteI--;

                if (deleteI > files.Count)
                {
                    Console.WriteLine("File does not exist in list!");
                    DisplayFiles(files);
                }
                else
                {
                    files.RemoveAt(deleteI--);
                    Console.Clear();
                    Console.WriteLine("Removed from list!");
                    DisplayFiles(files);
                }

                Console.ReadLine();
            }
            catch(IndexOutOfRangeException)
            {
                Console.Clear();
                Console.WriteLine("Number out of bounds");
                DisplayFiles(files);
            }
        }
    }
}


