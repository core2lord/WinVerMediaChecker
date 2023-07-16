using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WinVerMediaChecker
{
    internal class VersionCheckerCommand
    {
        #region Private Fields

        private MainWindow _mainWindow;

        #endregion

        #region Public Methods

        public void ExecuteCommandSync(object command)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo =
                new System.Diagnostics.ProcessStartInfo("cmd", "/c " + $"DISM / get - wiminfo / wimfile:\"{_mainWindow.DriveSelection.Content}sources\\install.wim\"")
                {
                    // The following commands are needed to redirect the standard output.
                    // This means that it will be redirected to the Process.StandardOutput StreamReader.
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    // Do not create the black window.
                    CreateNoWindow = true
                };
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                _mainWindow.ResultsTextBlock.Text = result;
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.InnerException}\n{e.Message}");
            }
        }

        #endregion
    }
}