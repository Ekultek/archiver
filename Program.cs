using System;
using System.IO;
using System.IO.Compression;

namespace ArchiveCreator
{
    /* 
     * This interface is used as a set point
     * for the information handling. All information
     * will be color coordinated in order to 
     * show the severity of what's happening with
     * the program itself. IE:
     * Red => Bad
     * Green => Good
     */

    public interface Information
    {
        string Say(string input);
        string Success(string input);
        string Warn(string input);
        string FatalErr(string input);
        string MinorErr(string input);
    }

    /* 
     * This class is where the interface is
     * inherited from. Basically this will
     * contain the color coordinating of the
     * information displayed from the interface.
     */

    public class ConsoleReport : Information
    {
        public string Success(string input)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(input);
            return input;
        }

        public string Warn(string input)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(input);
            return input;
        }

        public string Say(string input)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(input);
            return input;
        }

        public string FatalErr(string input)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(input);
            return input;
        }

        public string MinorErr(string input)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(input);
            return input;
        }

    }

    /* 
     * Main class of the program, basically this
     * class is what makes the program actually
     * run.
     */

    class Archive
    {
        static void Zip(string fromDir, string zipName)
        {
            ZipFile.CreateFromDirectory(fromDir, zipName, CompressionLevel.Fastest, true);
        }

        //Main method
        static void Main(string[] args)
        {

            /* 
             * Create the variables that will store
             * the required information. Basically
             * these are the information handling variable
             * that is derived from the class and a random
             * filename so that you won't overwrite
             * one of your zip files.
             */

            Information info = new ConsoleReport();
            string randFileName = Path.GetRandomFileName();

            info.Say("Starting file extraction..");

            /* 
             * These variables are required in order to run
             * the program successfully. The day variable is
             * to add a day of archiving to the zip filename
             * this will help if you have a lot of zip files
             * and a lot of folders in that directory.
             */

            string day = DateTime.Now.ToString("MM-dd-yy ");
            string userName = Environment.UserName;
            string startDir = $"c:/users/{userName}/test_folder";
            string zipDir = $"c:/users/{userName}/archive/{day}{randFileName}.zip";
            string dirName = $"c:/users/{userName}/archive";

            //Check if the directory exists
            info.Say("Attempting to create archive directory..");

            if (Directory.Exists(dirName))
            {
                info.MinorErr("Directory already exists, resuming extraction process");
            }

            else
            {
                //Create it if it doesn't
                info.Warn($"Creating archive directory here: {dirName}");
                Directory.CreateDirectory(dirName);
                info.Say("Directory created, resuming process..");
            }

            try
            {
                //Attempt to extract to zip file
                info.Say($"Attempting to extract files into: {zipDir}");
                Zip(startDir, zipDir);
                info.Success($"Extracted file successfully to: {zipDir}");
            }

            catch (Exception e)
            {

                /* 
                 * Catch any error that occurs during
                 * the archiving stage and log the error
                 * to a text file for further analysis
                 */

                var programPath = System.Reflection.Assembly.GetExecutingAssembly();
                info.FatalErr($"Something went wrong and the program cannot continue, exiting process with error code {e}..");
                info.FatalErr("Writing error to file for further analysis.");
                File.WriteAllText($"{programPath}/log/errorlog.txt", e.ToString());
            }

            info.Say("Press enter to exit..");
            Console.ReadLine();
        }
    }
}