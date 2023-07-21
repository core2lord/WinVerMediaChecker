using System.Collections.Generic;
using System.IO;

#nullable disable

namespace WinVerMediaChecker;

internal static class WindowsMediaVerify
{
    #region Public Properties

    public static Dictionary<int, WindowsImageFileNames> WindowsImageFiles { get; } = new Dictionary<int, WindowsImageFileNames>();

    #endregion

    #region Public Methods

    public static bool VerifySelectedMedia(DirectoryInfo rootDirectory, out WindowsImageFileNames validImageFileName)
    {
        foreach (var file in WindowsImageFiles)
        {
            var imageFileName = WindowsImageFiles[file.Key];
            if (Path.Exists($"{rootDirectory}\\sources\\{imageFileName}"))
            {
                validImageFileName = imageFileName;
                return true;
            }
            if (Path.Exists($"{rootDirectory}\\x86\\sources\\{imageFileName}"))
            {
                validImageFileName = imageFileName;
                return true;
            }
            if (Path.Exists($"{rootDirectory}\\x64\\sources\\{imageFileName}"))
            {
                validImageFileName = imageFileName;
                return true;
            }
        }
        return false;
    }

    #endregion
}

public struct WindowsImageFileNames
{
    #region Public Fields

    public static string[] ImageNames = { "install.wim", "install.esd", "boot.wim" };

    #endregion
}