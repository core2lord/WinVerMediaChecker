using System.Collections.Generic;
using System.IO;
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

    // public ActiveDrives ActiveDrives { get; set; } = new();

    #region Public Properties

    public List<string> ActiveDrives { get; set; } = new();
    public Dictionary<int, DirectoryInfo> ActiveRootDirectories { get; set; } = new();

    #endregion

    #region Private Methods

    private void ActiveDrivesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (ActiveDrivesList.SelectedIndex >= 1)
        {
            DriveSelection.Content = ActiveRootDirectories[ActiveDrivesList.SelectedIndex];
            ConfirmCheckMarkLabel.Visibility = Visibility.Visible;
        }
        else
        {
            DriveSelection.Content = null;
            ConfirmCheckMarkLabel.Visibility = Visibility.Collapsed;
        }
    }

    private void DriveRefresh()
    {
        int _activeDriveCount = 1;
        var _driveInfo = new MediaSearch(this).FindActiveDrives();
        ResetDropDownMenu(this, new RoutedEventArgs());

        foreach (var activeDrive in _driveInfo)
        {
            var rootDirectoryString = activeDrive.RootDirectory.ToString();
            ActiveRootDirectories.Add(_activeDriveCount, activeDrive.RootDirectory);
            if (activeDrive.Name != rootDirectoryString)
            {
                ActiveDrives.Add($"#{_activeDriveCount++,2}). | {rootDirectoryString.ToUpper(),2}, {activeDrive.Name,2}");
                ActiveDrivesList.Items.Refresh();
                break;
            }
            ActiveDrives.Add($"#{_activeDriveCount++,2}). | {rootDirectoryString.ToUpper(),2}");
            ActiveDrivesList.Items.Refresh();
        }
        ActiveDrivesList.IsDropDownOpen = true;
    }

    private void RefreshDrivesButton_Click(object sender, RoutedEventArgs e)
    {
        DriveRefresh();
    }

    private void ResetDropDownMenu(object sender, RoutedEventArgs e)
    {
        ActiveDrives.Clear();
        ActiveRootDirectories.Clear();
        ActiveDrives.Add("Select Drive Containing Media");
        ActiveDrivesList.Items.Refresh();
        ActiveDrivesList.SelectedIndex = 0;
        ConfirmCheckMarkLabel.Visibility = Visibility.Collapsed;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ActiveDrivesList.ItemsSource = ActiveDrives;
        ResetDropDownMenu(this, new RoutedEventArgs());
    }

    #endregion
}