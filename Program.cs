using System;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Linq;

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

    public interface ILogger
    {
        string Say(string input);
        string Success(string input);
        string Warn(string input);
        string FatalErr(string input);
    }

    /* 
     * This class is where the interface is
     * inherited from. Basically this will
     * contain the color coordinating of the
     * information displayed from the interface.
     */

    public class ConsoleReport : ILogger
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
             * the required information. 
             */

            ILogger info = new ConsoleReport();
            string randFileName = Path.GetRandomFileName();

            info.Say("Starting file extraction..");

            /* 
             * The day variable is to add a day of archiving to 
             * the zip filename this will help if you have a lot 
             * of zip files in that directory. The folder variable
             * is to allow the user the ability to choose which
             * folder they want to archive from.
             */

            string folder;
            Console.Write("Enter folder to extract from: ");
            folder = Console.ReadLine();

            var profileDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string day = DateTime.Now.ToString("MM-dd-yy ");
            string startDir = $@"{profileDir}\{folder}";
            string zipDir = $@"{profileDir}\archive\{day}{randFileName}.zip";
            string dirName = $@"{profileDir}\archive";

            //Check if the archive directory exists
            info.Say("Attempting to create archive directory..");

            if (Directory.Exists(dirName))
            {
                info.Success("Directory already exists, resuming extraction process");
            }

            else
            {
                //Create archive directory if it doesn't exist
                info.Say($"Creating archive directory here: {dirName}");
                Directory.CreateDirectory(dirName);
                info.Say("Directory created, resuming process..");
            }

            try
            {
                //Attempt to extract folder to zip file
                info.Say($"Attempting to extract files into: {zipDir}");
                Zip(startDir, zipDir);
                info.Success($"Extracted file successfully to: {zipDir}");
            }

            catch (Exception e)
            {
                /* 
                 * Catch any error that occurs during the 
                 * archiving stage and log the error to a 
                 * text file for further analysis.
                 */

                var programPath = AppDomain.CurrentDomain.BaseDirectory;
                info.FatalErr($"Something went wrong and the program cannot continue, exiting process with error code {e}..");
                info.FatalErr("Writing error to file for further analysis.");
                File.WriteAllText($"{programPath}/log/errorlog.txt", e.ToString());
                Environment.Exit(2);
            }

            info.Say("Press enter to exit..");
            Console.ReadLine();
        }
    }
}