using System.Collections.Generic;
using System.Windows;

namespace WinVerMediaChecker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    #region Public Constructors

    public MainWindow()
    {
        InitializeComponent();
    }

    #endregion

    #region Public Properties

    public Dictionary<int, string> ActiveDrives { get; set; } = new();

    #endregion

    #region Private Methods

    private void ActiveDrivesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        DriveSelection.Content = ActiveDrives[ActiveDrivesList.SelectedIndex];
    }

    private void DriveRefresh()
    {
        int _activeDriveCount = 1;
        ActiveDrivesList.Items.Clear();
        var _driveInfo = new MediaSearch(this).FindActiveDrives();
        foreach (var activeDrive in _driveInfo)
        {
            var rootDirectoryString = activeDrive.RootDirectory.ToString();
            ActiveDrives.Add(_activeDriveCount++, rootDirectoryString);
            if (activeDrive.Name != rootDirectoryString)
            {
                ActiveDrivesList.Items.Add($"#{_activeDriveCount++,10}). | {rootDirectoryString.ToUpper(),5}   {activeDrive.Name}");
                break;
            }
            ActiveDrivesList.Items.Add($"#{_activeDriveCount++,10}).    {rootDirectoryString.ToUpper(),5}");
        }
    }

    private void RefreshDrivesButton_Click(object sender, RoutedEventArgs e)
    {
        DriveRefresh();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        DriveRefresh();
    }

    #endregion
}