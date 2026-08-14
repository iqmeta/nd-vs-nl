using data;
using NiceLabel.SDK;
using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace nice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("de-DE");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            try
            {
                PrintEngineFactory.PrintEngine.Initialize();
            }
            catch
            {
                Console.WriteLine("Could not initialize NiceLabel Print Engine. Is NiceLabel installed on this machine?");
                return;
            }            
            var label = PrintEngineFactory.PrintEngine.OpenLabel("Label.nlbl");
            label.PrintSettings.OutputFileName = $"nicelabel.prn";


            //REAL RENDERING STARTS HERE / REST STAYS AVAILABLE IN MEMORY AFTER APP IS RUNNING
            
            DateTime start = DateTime.Now;
            var session = label.StartSessionPrint();
            foreach (var labelData in DataHelper.GetLabelData())
            {
                label.Variables["Hello"].SetValue(labelData.Name);
                label.SessionPrint(labelData.Qty, session);
            }
            label.EndSessionPrint(session);
            // PREVENT MICROSOFT PRINT QUEUE FILE SYSTEM I/O PROBLEMS - RACE CONDITIONS
            byte[] outBytes = null;
            int retryCount = 0;
            const int maxRetries = 10;  // Adjust as needed
            const int retryDelayMs = 10;  // Delay between retries
            while (retryCount < maxRetries)
            {
                try
                {
                    outBytes = File.ReadAllBytes(label.PrintSettings.OutputFileName);

                    //System.IO.File.Delete(label.PrintSettings.OutputFileName);
                    break;  // Success, exit loop
                }
                catch (IOException ex) when (ex.Message.Contains("being used by another process"))  // Check for lock-specific error
                {
                    retryCount++;
                    System.Threading.Thread.Sleep(retryDelayMs);  // Wait briefly before retrying
                }
            }
            try
            {
                //File.Delete(label.PrintSettings.OutputFileName);
            }
            catch (Exception) { /* Ignore cleanup errors */ }
            if (outBytes == null)
            {
                throw new IOException("Failed to read the generated after retries " + maxRetries + " due to file lock.");
            }
            DateTime end = DateTime.Now;
            var total = end - start;
            Console.Write($"After {total.TotalMilliseconds} ms ZPL Printstream ready in memory with {outBytes.Length / 1024.0:F2} kb");

            //// CLEAN CLEAN CLEAN
            label.Dispose();
            label = null;
            session.Dispose();
            session = null;
        }
    }
}
