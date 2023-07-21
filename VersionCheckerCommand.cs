using System;
using System.Diagnostics;
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
        #region Public Constructors

        public VersionCheckerCommand(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        #endregion

        #region Private Fields

        private MainWindow _mainWindow;

        #endregion

        #region Public Methods

        public void ExecuteCommandSync()
        {
            try
            {
                // create the ProcessStartInfo
                // Incidentally, /c tells WindowsConsole that we want it to execute the command that follows,
                // and then exit.
                ProcessStartInfo proccessStartInfo =
                new("cmd", "/c " + $"DISM / get - wiminfo / wimfile:\"{_mainWindow.CurrentDriveSelection.Content}sources\\install.wim\"")
                {
                    // The following commands are needed to redirect the standard output to Process.StandardOutput StreamReader.
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    ErrorDialog = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    // Do not create a new Console Window.
                    CreateNoWindow = true,
                };
                // Now we create a process, assign its ProcessStartInfo and start it
                using Process activeProcess = new()
                {
                    StartInfo = proccessStartInfo
                };
                activeProcess.Start();
                // Get the output into a string
                string result = activeProcess.StandardOutput.ReadToEnd();
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