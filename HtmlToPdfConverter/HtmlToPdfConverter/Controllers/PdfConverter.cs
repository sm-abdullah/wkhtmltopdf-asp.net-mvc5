using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace HtmlToPdfConverter.Controllers
{
    public class PDFConverter 
    {
         /// <summary>
         /// it will Convert HTML Code into PDF
         /// </summary>
         /// <param name="htmlCode">HTML Code</param>
         /// <param name="wkhtmlToPdfExePath">wkhtmlToPdf Exe file Path</param>
         /// <returns></returns>
        public byte[] Convert(string htmlCode, string wkhtmlToPdfExePath)
        {
            Process process;
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = wkhtmlToPdfExePath;
            processStartInfo.WorkingDirectory = @"D:\";

            // run the conversion utility
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            // note: that we tell wkhtmltopdf to be quiet and not run scripts
            string args = "-q -n ";
            args += "--disable-smart-shrinking ";
            args += "";
            args += "--outline-depth 0 ";
            args += "--page-size A4 ";
            args += " - -";

            processStartInfo.Arguments = args;

            process = Process.Start(processStartInfo);

                using (StreamWriter stramWriter = process.StandardInput)
                {
                    stramWriter.AutoFlush = true;
                    stramWriter.Write(htmlCode);
                }

                //read output
                byte[] buffer = new byte[32768];
                byte[] file;
                using (var memoryStream = new MemoryStream())
                {
                    while (true)
                    {
                        int read = process.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length);
                        if (read <= 0)
                            break;
                        memoryStream.Write(buffer, 0, read);
                    }
                    file = memoryStream.ToArray();
                }

                process.StandardOutput.Close();
                // wait or exit
                process.WaitForExit(60000);

                // read the exit code, close process
                int returnCode = process.ExitCode;
                process.Close();
               
                process.Dispose();
                if (returnCode == 0 || returnCode == 1)
                {
                    return file;
                }
                else
                {
                    throw new Exception(string.Format("Could not create PDF, returnCode:{0}", returnCode));
                }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="wkhtmlToPdfExePath"></param>
        /// <returns></returns>
        public byte[] Convert(Uri url, string wkhtmlToPdfExePath)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = wkhtmlToPdfExePath,
                WorkingDirectory = @"D:\",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            var args = new StringBuilder();
            args.Append("-q  ");
            args.Append("--footer-left"); args.Append("  \"Converted by Wkhtml To Pdf Written By Sm.Abdullah".PadRight(90) + DateTime.Now.ToString("dd-MMM-yy") + "\"  ");
            args.Append("--footer-font-size"); args.Append(" 9   ");
            args.Append("--footer-right"); args.Append(" [page]/[toPage]  ");
            args.Append("--footer-line  ");
            args.Append("--outline-depth"); args.Append(" 0  ");
            args.Append("--enable-javascript  ");
            args.Append("--no-stop-slow-scripts  ");
            args.Append("--javascript-delay"); args.Append(" 3500  ");
            args.Append("--page-size"); args.Append(" A4  ");
            args.Append("\"" + url + "\"" + " -");

            processStartInfo.Arguments = args.ToString();



            Process process = Process.Start(processStartInfo);

                byte[] buffer = new byte[32768];
                byte[] file;
                using (var memoryStream = new MemoryStream())
                {
                    while (true)
                    {
                        int read = process.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length);
                        if (read <= 0)
                            break;
                        memoryStream.Write(buffer, 0, read);
                    }
                    file = memoryStream.ToArray();
                }

                process.StandardOutput.Close();
                // wait or exit
                process.WaitForExit(60000);

                // read the exit code, close process
                int returnCode = process.ExitCode;
                process.Close();
                process.Dispose();
                if (returnCode == 0 || returnCode == 1)
                    return file;
                else
                {
                    throw new Exception(string.Format("Could not create PDF, returnCode:{0}", returnCode));
                }
        }
    }
}