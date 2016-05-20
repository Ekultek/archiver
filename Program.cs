using System;
using System.IO;
using System.IO.Compression;

namespace ArchiveCreator
{
    class Program
    {
        //When program is run successfully 
        //this will be the output
        public string Success(string input)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(input);
            return input;
        }

        //When program encounters an error 
        //this will be the output
        public string Warn(string input)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(input);
            return input;
        }

        //When program has information to show
        //this will be the output
        public string Say(string input)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(input);
            return input;
        }

        //Main method
        static void Main(string[] args)
        {
            //These variables are used to create a
            //random string that will be used as the
            //zip files name
            var chars = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var stringChars = new char[8];
            var random = new Random();

            //Info is used as provide the type of
            //information that will be displayed
            //by the program
            Program info = new Program();

            //Create the zip file name
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            string finalString = new String(stringChars);

            info.Say("Starting file extraction..");

            string userName = Environment.UserName;
            string startDir = $"c:/users/{userName}/test_folder";
            string zipDir = $"c:/users/{userName}/archive/{finalString}.zip";
            string dirName = $"c:/users/{userName}/archive";

            //Check if the directory exists
            if (Directory.Exists(dirName))
            {
                info.Say("Directory already exists, resuming");
            }
            else
            {
                //Create it if it doesn't
                Directory.CreateDirectory(dirName);
            }

            try
            {
                ZipFile.CreateFromDirectory(startDir, zipDir);
            }
            catch (Exception e)
            {
                info.Warn($"Error: {e}");
            }
            info.Success($"Extracted files successfully to: {zipDir}");
            info.Say("Press enter to exit..");
            Console.ReadLine();
        }
    }
}