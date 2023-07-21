using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;

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

    public Dictionary<int, DirectoryInfo> ActiveRootDirectories { get; set; } = new();
    public List<string> ReadyStorage { get; set; } = new();
    public Dictionary<int, DriveType> ReadyStorageType { get; set; } = new();

    #endregion

    #region Private Methods

    private void DriveRefresh()
    {
        int _activeDriveCount = 1;
        var _driveInfo = new MediaSearch(this).FindActiveDrives();
        ResetAll(this, new RoutedEventArgs());

        foreach (var activeDrive in _driveInfo)
        {
            var storageRootString = activeDrive.RootDirectory.Root.ToString();
            var storageRootStringUpper = activeDrive.RootDirectory.Root.ToString().ToUpper();
            var storageVolumeLabel = activeDrive.VolumeLabel;
            ActiveRootDirectories.Add(_activeDriveCount, activeDrive.RootDirectory.Root);
            ReadyStorageType.Add(_activeDriveCount, activeDrive.DriveType);

            if (activeDrive.Name != storageRootString)
            {
                ReadyStorage.Add($"#{_activeDriveCount++,2}). | {$"{{{activeDrive.DriveType}}}",2}, {$"({storageRootStringUpper})",4}), {$"[{storageVolumeLabel}]",2}");
                ReadyDrivesComboBox.Items.Refresh();
                break;
            }
            ReadyStorage.Add($"#{_activeDriveCount++}) | {$"{{{activeDrive.DriveType}}}",2}, {$"({storageRootStringUpper})",4}");
            ReadyDrivesComboBox.Items.Refresh();
        }
        ReadyDrivesComboBox.IsDropDownOpen = true;
    }

    private void ExecuteCommandButton_Click(object sender, RoutedEventArgs e)
    {
        VersionCheckerCommand versionChecker = new(this);
        versionChecker.ExecuteCommandSync();
    }

    private void ReadyDrivesComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        var selectedIndex = ReadyDrivesComboBox.SelectedIndex;
        SolidColorBrush redBrush = new(Colors.Red);
        SolidColorBrush greenBrush = new(Colors.Green);
        StorageVerify.Visibility = Visibility.Visible;
        if (selectedIndex >= 1)
        {
            var selectedRoot = ActiveRootDirectories[selectedIndex];
            var driveType = ReadyStorageType[selectedIndex];
            CurrentDriveSelection.Content = selectedRoot.ToString();
            switch (driveType)
            {
                case DriveType.Removable:
                    {
                        // Removable Storage
                        StorageVerify.Content = "\uECF3";
                        break;
                    }
                case DriveType.Fixed:
                    {
                        // Checkmark
                        StorageVerify.Content = "\uE001";
                        break;
                    }
                case DriveType.Unknown:
                    {
                        // Checkmark
                        StorageVerify.Content = "\uE001";
                        break;
                    }
            }
            if (!Path.Exists(selectedRoot.ToString()))
            {
                StorageVerify.Content = "\uEA39";
                StorageVerify.Foreground = redBrush;
                return;
            }
            if (!WindowsMediaVerify.VerifySelectedMedia(selectedRoot, out var validImageFileName))
            {
                StorageVerify.Content = "\uEA39";
                StorageVerify.Foreground = redBrush;
                ResultsTextBlock.Text = $"Unable to verify [{CurrentDriveSelection.Content.ToString()!.ToUpper()}] storage as valid Microsoft Windows Operating System installation source";
                return;
            }
            ExecuteCommandButton.IsEnabled = true;
            StorageVerify.Foreground = greenBrush;
        }
        else
        {
            GBox1.Visibility = Visibility.Visible;
            CurrentDriveSelection.Content = null;
            StorageVerify.Content = "\uEA39";
            StorageVerify.Foreground = redBrush;
        }
    }

    private void RefreshDrivesButton_Click(object sender, RoutedEventArgs e)
    {
        DriveRefresh();
    }

    private void ResetAll(object sender, RoutedEventArgs e)
    {
        ReadyStorage.Clear();
        ActiveRootDirectories.Clear();
        ReadyStorage.Add("Select Drive Containing Media");
        ReadyDrivesComboBox.Items.Refresh();
        ReadyDrivesComboBox.SelectedIndex = 0;
        StorageVerify.Visibility = Visibility.Collapsed;
        ExecuteCommandButton.IsEnabled = false;
        ResultsTextBlock.Text = "Results Log...";
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ReadyDrivesComboBox.ItemsSource = ReadyStorage;
        ResetAll(this, new RoutedEventArgs());
    }

    #endregion
}