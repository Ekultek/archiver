using System;
using System.IO;
using System.IO.Compression;

namespace ArchiveCreator
{
    class Archive
    {

        //These static strings are used for 
        //information handling they will be
        //color coordinated so you can see
        //what kind of information is being 
        //passed to you
        static string Success(string input)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(input);
            return input;
        }

        static string Warn(string input)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(input);
            return input;
        }

        static string Say(string input)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(input);
            return input;
        }

        static string FatalErr(string input)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(input);
            return input;
        }

        static string MinorErr(string input)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(input);
            return input;
        }

        //Main method
        static void Main(string[] args)
        {
            //These variables are used to create a
            //random string that will be used as the
            //zip files name
            var chars = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var randFileName = new char[4];
            var random = new Random();

            //Create the zip file name
            for (int i = 0; i < randFileName.Length; i++)
            {
                randFileName[i] = chars[random.Next(chars.Length)];
            }
            string finalString = new String(randFileName);

            Say("Starting file extraction..");

            string day = DateTime.Now.ToString("MM-dd-yy ");
            string userName = Environment.UserName;
            string startDir = $"c:/users/{userName}/test_folder";
            string zipDir = $"c:/users/{userName}/archive/{day}{finalString}.zip";
            string dirName = $"c:/users/{userName}/archive";
            string[] files = Directory.GetFiles(startDir);

            //Check if the directory exists
            Say("Attempting to create archive directory..");
            if (Directory.Exists(dirName))
            {
                MinorErr("Directory already exists, resuming extraction process");
            }
            else
            {
                //Create it if it doesn't
                Directory.CreateDirectory(dirName);
            }

            try
            {
                //Attempt to extract to zip file
                ZipFile.CreateFromDirectory(startDir, zipDir);
                Success($"Extracted files successfully to: {zipDir}");
            }
            catch (Exception e)
            {
                FatalErr("Something went wrong and the program cannot continue, exiting process..");
            }
            Say("Press enter to exit..");
            Console.ReadLine();
        }
        
    }
}