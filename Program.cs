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

            //Check if the directory exists
            Say("Attempting to create archive directory..");
            if (Directory.Exists(dirName))
            {
                MinorErr("Directory already exists, resuming extraction process");
            }
            else
            {
                //Create it if it doesn't
                Warn($"Creating archive directory here: {dirName}");
                Directory.CreateDirectory(dirName);
                Say("Directory created, resuming process..");
            }

            try
            {
                //Attempt to extract to zip file
                Say($"Attempting to extract files into: {zipDir}");
                ZipFile.CreateFromDirectory(startDir, zipDir, CompressionLevel.Fastest, true);
                Success($"Extracted files successfully to: {zipDir}");
            }
            catch (Exception e)
            {

                //Catch any error that occurs during
                //the archiving stage and log the error
                //to a text file for further analysis
                var programPath = System.Reflection.Assembly.GetExecutingAssembly();
                FatalErr($"Something went wrong and the program cannot continue, exiting process with error code {e}..");
                FatalErr("Writing error to file for further analysis.");
                File.WriteAllText($"{programPath}/log/errorlog.txt", e.ToString());
            }

            Say("Press enter to exit..");
            Console.ReadLine();
        }
    }
}