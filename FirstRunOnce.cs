using System;
using System.Windows;
using Microsoft.VisualBasic;

#nullable disable

namespace WinVerMediaChecker;

public static class FirstRunOnce
{
    #region Private Properties

    private static MainWindow _mainWindow { get; set; } = (MainWindow)Application.Current.MainWindow;

    #endregion Private Properties

    #region Public Properties

    public static bool IsFirstRun { get; set; } = true;

    #endregion Public Properties

    #region Public Methods

    public static void RunOnce()
    {
        if (IsFirstRun)
        {
            MediaValidation mediaValidation = new(_mainWindow);
            VersionCheckerCommand versionCheckerCommand = new(_mainWindow);
            _mainWindow!.GroupBoxTop.Visibility = Visibility.Collapsed;
            _mainWindow.GroupBoxMiddle.Visibility = Visibility.Collapsed;
            _mainWindow?.InvokeRefreshDrivesButton_Click(typeof(FirstRunOnce));
            for (int i = 1; i < _mainWindow.ActiveRootDirectories.Count; i++)
            {
                var DriveUnderTest = _mainWindow.ActiveRootDirectories[i];

                if (mediaValidation.IsWindowsMedia(DriveUnderTest, out string path))
                {
                    if (path is not null)
                    {
                        versionCheckerCommand.ExecuteCommandSync(path);
                        return;
                    }
                }
                _mainWindow.AppendResultsLogText("failed to find valid windows installation media");
                _mainWindow.InvokeResetAll(typeof(FirstRunOnce));
            }
            IsFirstRun = false;
        }
    }

    #endregion Public Methods
}