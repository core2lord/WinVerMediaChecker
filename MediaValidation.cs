using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;

#nullable disable

namespace WinVerMediaChecker
{
    internal class MediaValidation
    {
        #region Public Fields

        public string[] ImageNames = { "install.esd", "install.wim", "boot.wim" };

        #endregion Public Fields

        #region Private Fields

        private readonly SolidColorBrush _greenBrush = new(Colors.Green);
        private readonly MainWindow _mainWindow;
        private readonly SolidColorBrush _redBrush = new(Colors.Red);

        public MediaValidation(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        #region Public Properties

        public char[] ReadyDriveArray { get; set; }

        #endregion Public Properties

        #region Public Methods

        private bool VerifyImageName(DirectoryInfo rootDirectory, out string path)
        {
            string[] testPaths = { $"{rootDirectory}\\sources\\", $"{rootDirectory}\\x86\\sources\\", $"{rootDirectory}\\x64\\sources\\" };

            foreach (var imageFileName in ImageNames)
            {
                foreach (var testPath in testPaths)
                {
                    var _pathUnderTest = testPath + imageFileName;

                    if (Path.Exists(_pathUnderTest))
                    {
                        path = _pathUnderTest;
                        return true;
                    }
                }
            }
            path = null;
            return false;
        }

        public bool IsWindowsMedia(DirectoryInfo rootPathUnderTest, out string testedPath)
        {
            if (Path.Exists(rootPathUnderTest.Name))
            {
                DriveInfo _driveInfoUnderTest = new(rootPathUnderTest.Root.ToString());
                if (VerifyImageName(rootPathUnderTest, out string path))
                {
                    switch (_driveInfoUnderTest.DriveType)
                    {
                        case DriveType.Removable:
                            {
                                // Removable Storage
                                _mainWindow.StorageVerifyLabel.Content = "\uECF3";
                                break;
                            }
                        case DriveType.Fixed:
                            {
                                // Checkmark
                                _mainWindow.StorageVerifyLabel.Content = "\uE001";
                                break;
                            }
                        case DriveType.Unknown:
                            {
                                // Checkmark
                                _mainWindow.StorageVerifyLabel.Content = "\uE001";
                                break;
                            }
                    }
                    testedPath = path;
                    return true;
                 }
               }
            testedPath = null;
            return false;
        }
        #endregion
    }
}
#endregion
