using Microsoft.VisualBasic;
using PrinterPlusPlusSDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace VPrint
{
    class Processor : PrinterPlusPlusSDK.IProcessor
    {
        public PrinterPlusPlusSDK.ProcessResult Process(string key, string psFilename)
        {

            Microsoft.VisualBasic.Interaction.MsgBox(psFilename);
            //Microsoft.VisualBasic.Interaction.MsgBox(key);
            var recipients = Microsoft.VisualBasic.Interaction.InputBox("Enter patinet ID:");
            //Microsoft.VisualBasic.Interaction.MsgBox(recipients);

            var note = Microsoft.VisualBasic.Interaction.InputBox("Add Note:");
            //Microsoft.VisualBasic.Interaction.MsgBox(note);
            //Convert PS to Text
            //Microsoft.VisualBasic.Interaction.MsgBox(psFilename);

            //var txtFilename = System.IO.Path.GetTempFileName();
            //Microsoft.VisualBasic.Interaction.MsgBox(txtFilename);

            //psFilename.Replace("
            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential("pri", "pri");
                //client.UploadFile("ftp://cp.orin.com/EmailWeatherInCelcius_KRUDO_Meir_20150725_232841_21.ps", @"C:\\PrinterPlusPlus\\Temp\\EmailWeatherInCelcius_KRUDO_Meir_20150725_232841_21.ps");
                client.UploadFile("ftp://cp.orinoa.com/VP_" + recipients + "232841_21.ps", psFilename);

            }
            
            //try
            //{
            //    var y = ConvertPsToPdf(psFilename, txtFilename);
            //    Microsoft.VisualBasic.Interaction.MsgBox("Y" + y);
            //}
            //catch (Exception ex)
            //{
            //    Microsoft.VisualBasic.Interaction.MsgBox(ex.Message);
            //}
            
            //var x = ConvertPsToTxt(psFilename, txtFilename);

            //SendFTP(psFilename);
            //Microsoft.VisualBasic.Interaction.MsgBox(y);
            //Microsoft.VisualBasic.Interaction.MsgBox(txtFilename);

            ////Process the converted Text File
            //var extractedValue = ProcessTextFile(txtFilename);

            //Ask user for recipeint's email
            //var recipients = Microsoft.VisualBasic.Interaction.InputBox("Enter email address of recipient.");
            
            ////Send email if user entered an email address
            //if (!string.IsNullOrWhiteSpace(recipients))
            //{
            //    SendEmail(extractedValue, recipients);
            //}
            //SendFTP(txtFilename);

            return new ProcessResult();
        }
        public static string ConvertPsToTxt(string psFilename, string txtFilename)
        {
            var retVal = string.Empty;
            var errorMessage = string.Empty;
            var command = "C:\\ps2txt\\ps2txt.exe";
            var args = string.Format("-nolayout \"{0}\" \"{1}\"", psFilename, txtFilename);
            retVal = Shell.ExecuteShellCommand(command, args, ref errorMessage);

            return retVal;

        }

        public static string ConvertPsToPdf(string psFilename, string txtFilename)
        {
            var retVal = string.Empty;
            var errorMessage = string.Empty;
            var command = "C:\\PS2PDF\\ps2pdf.exe";
            var args = string.Format("-nolayout \"{0}\" \"{1}\"", psFilename, @"C:\PrinterPlusPlus\Temp\output.pdf");
            retVal = Shell.ExecuteShellCommand(command, args, ref errorMessage);

            return retVal;

        }
         

        public ExtractedValue ProcessTextFile(string txtFilename)
        {
            var values = new ExtractedValue();      //Create the extracted values placeholders
            var reachedMarker = false;
            //Read the text file
            using (System.IO.StreamReader sr = System.IO.File.OpenText(txtFilename))
            {
                while (sr.Peek() > -1)
                {
                    var currentLine = sr.ReadLine().Trim();

                    //Skip whitespaces
                    if (string.IsNullOrWhiteSpace(currentLine))
                        continue;

                    //Checked if we've reached the marker to begin extraction
                    if (reachedMarker == true)
                    {
                        //Get Title value
                        if (string.IsNullOrWhiteSpace(values.Title))
                        {
                            values.Title = currentLine;
                            continue;
                        }
                        //Skip Right Now value
                        if (currentLine.ToLower() == "right now")
                            continue;
                        //Get UpdatedDateTime value
                        if (string.IsNullOrWhiteSpace(values.UpdatedDateTime))
                        {
                            values.UpdatedDateTime = currentLine;
                            continue;
                        }
                        //Get TemperatureFahrenheit and convert value to Celcius
                        if (values.TemperatureFahrenheit == 0)
                        {
                            values.TemperatureFahrenheit = Convert.ToDouble(currentLine.Substring(0, currentLine.Length - 2));
                            values.TemperatureCelcius = ((values.TemperatureFahrenheit - 32) / 1.8);
                            continue;
                        }
                        //Get Forecast value
                        if (string.IsNullOrWhiteSpace(values.Forecast))
                        {
                            values.Forecast = currentLine;
                            break;
                        }
                    }
                    //Mark farming so we can begin extracting data
                    if (currentLine.ToLower() == "farming")
                        reachedMarker = true;
                }
            }
            return values;      //Return the extracted values
        }
        public class ExtractedValue
        {
            public string Title { get; set; }
            public string UpdatedDateTime { get; set; }
            public double TemperatureFahrenheit { get; set; }
            public double TemperatureCelcius { get; set; }
            public string Forecast { get; set; }
        }

        public void SendFTP(string fileName)
        {
            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential("printer", "printer");
                client.UploadFile("cp.orinoa.com", "STOR", fileName);
            }
        }


        public void SendEmail(ExtractedValue values, string recipient)
        {

          

            try
            {
                string body = "";
                
                body = values.Title;
                body += Environment.NewLine;
                body += values.UpdatedDateTime;
                body += Environment.NewLine;
                body += values.TemperatureCelcius;
                body += Environment.NewLine;
                body += values.Forecast;
                File.WriteAllText(@"c:\temp\a.txt", body);


                //var fromAddress = new MailAddress("meir.krudo@gmail.com", "Meir Krudo");
                //var toAddress = new MailAddress(recipient, "Meiry");
                //const string fromPassword = "";
                //const string subject = "Printer Driver";
                //string body = "";

                //body = values.Title;
                //body += Environment.NewLine;
                //body += values.UpdatedDateTime;
                //body += Environment.NewLine;
                //body += values.TemperatureCelcius;
                //body += Environment.NewLine;
                //body += values.Forecast;
                ////BodyEncoding = System.Text.Encoding.UTF8;

                //var smtp = new SmtpClient
                //{
                //    Host = "smtp.gmail.com",
                //    Port = 587,
                //    EnableSsl = true,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    UseDefaultCredentials = false,
                //    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                //};
                //using (var message = new MailMessage(fromAddress, toAddress)
                //{
                //    Subject = subject,
                //    Body = body
                //})
                //{
                //    smtp.Send(message);
                //}


                //var smtpHost = "smtp.mail.com";     //Change this to correct SMTP server
                //var sender = "info@printerplusplus.com";    //Change this to the email address of the sender
                //SmtpClient client = new SmtpClient(smtpHost);

                //client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential("username", "password");  //Change this to the username and password for your SMTP server

                //MailAddress from = new MailAddress(sender);
                //MailAddress to = new MailAddress(recipient);
                //MailMessage message = new MailMessage(from, to);
                //message.Body = values.Title;
                //message.Body += Environment.NewLine;
                //message.Body += values.UpdatedDateTime;
                //message.Body += Environment.NewLine;
                //message.Body += values.TemperatureCelcius;
                //message.Body += Environment.NewLine;
                //message.Body += values.Forecast;
                //message.BodyEncoding = System.Text.Encoding.UTF8;
                //message.Subject = "Printer++ EmailWeatherInCelcius";
                //message.SubjectEncoding = System.Text.Encoding.UTF8;
                //client.Send(message);
                //message.Dispose();
            }
            catch (Exception ex)
            {
                //Error occured while sending email. Add code to handle error.
            }
        }
    }
}
