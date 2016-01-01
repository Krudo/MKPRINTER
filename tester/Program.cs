using PrinterPlusPlusSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace tester
{
    class Program
    {
        static void Main(string[] args)
        {
            //var retVal = string.Empty;
            //var errorMessage = string.Empty;
            //var command = "C:\\PS2PDF\\ps2pdf.exe ";
            //var opt = string.Format("-nolayout \"{0}\" \"{1}\""
            //    , @"C:\PrinterPlusPlus\Temp\file.ps"
            //    , @"C:\PrinterPlusPlus\Temp\output.pdf");

            //try
            //{
            //    retVal = Shell.ExecuteShellCommand(command, opt, ref errorMessage);
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            //var x = 4;

            //ExecuteCommand("echo testing");

            //string s = @"C:\\PrinterPlusPlus\\Temp\\EmailWeatherInCelcius_KRUDO_Meir_20150725_232841_21.ps";

            //s = s.Replace("\","\\\"");

            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential("printer", "printer");
                client.UploadFile("ftp://cp.orinoa.com/EmailWeatherInCelcius_KRUDO_Meir_20150725_232841_21.ps", @"C:\\PrinterPlusPlus\\Temp\\EmailWeatherInCelcius_KRUDO_Meir_20150725_232841_21.ps");
            }
            
        }

        

        static void ExecuteCommand(string command)
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            //processInfo = new ProcessStartInfo(@"C:\\PS2PDF\\sample\\test2.bat", "/c " + command);
            processInfo = new ProcessStartInfo(@"C:\\PS2PDF\\sample\\PPP.bat", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();

            // *** Read the streams ***
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            process.Close();
        }
    }
}
