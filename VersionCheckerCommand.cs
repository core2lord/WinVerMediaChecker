using System;
using System.Diagnostics;

namespace WinVerMediaChecker
{
    public class VersionCheckerCommand
    {
        #region Public Constructors

        public VersionCheckerCommand(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        #endregion Public Constructors

        #region Private Fields

        public MainWindow _mainWindow;

        #endregion Private Fields

        #region Public Methods

        public void ExecuteCommandSync(string imagePath)
        {
            try
            {
                // create the ProcessStartInfo
                // Incidentally, /c tells WindowsConsole that we want it to execute the command that follows,
                // and then exit.
                ProcessStartInfo proccessStartInfo =
                new("cmd", "/c " + $"DISM /Get-WimInfo /wimfile:\"{_mainWindow.CurrentDriveSelection.Content}sources\\{imagePath}\"")
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
                _mainWindow.AppendResultsLogText(result);
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                _mainWindow.AppendResultsLogText(e.ToString());
                Console.WriteLine($"{e.InnerException}\n{e.Message}");
            }
        }

        #endregion Public Methods
    }
}