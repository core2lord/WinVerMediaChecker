using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

    #endregion Public Constructors

    #region Private Fields

    private readonly SolidColorBrush greenBrush = new(Colors.Green);
    private readonly SolidColorBrush redBrush = new(Colors.Red);
    private bool _firstRun = true;
    private bool noSelection = true;
    private TimeSpan timeSinceLastRefreshDuration;
    private TimeSpan timeSinceLastRefreshStart;
    private string validatedImageFileName = string.Empty;
    private List<int> selectionCache = new();
    private MediaValidation mediaValidation;

    #endregion Private Fields

    #region Private Properties

    private List<string> TemporaryMenuTitle { get; set; } = new List<string>();

    #endregion Private Properties
        
    #region Public Properties
    public Dictionary<int, DirectoryInfo> ActiveRootDirectories { get; set; } = new();
    public List<string> ReadyStorage { get; set; } = new();
    public Dictionary<int, DriveType> ReadyStorageType { get; set; } = new();

    #endregion Public Properties

    #region Private Methods

    private void DriveRefresh()
    {
        int _activeDriveCount = 1;
        var _driveInfo = new GetMedia().FindActiveDrives;
        ResetAll(this, new RoutedEventArgs());
        ReadyDrivesComboBox.ItemsSource = ReadyStorage;

        foreach (var activeDrive in _driveInfo!.Invoke())
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
        if (GroupBoxTop.Visibility == Visibility.Visible)
        {
            ReadyDrivesComboBox.IsDropDownOpen = true;
        }
        timeSinceLastRefreshStart = new();
    }

    private void ExecuteCommandButton_Click(object sender, RoutedEventArgs e)
    {
        VersionCheckerCommand versionChecker = new(this);
        versionChecker.ExecuteCommandSync(validatedImageFileName);
        ExecuteCommandButton.IsEnabled = false;
    }

    private void ReadyDrivesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_firstRun)
        {
            var selectedIndex = ReadyDrivesComboBox.SelectedIndex;
            if (selectedIndex >= 1)
            {
                var selectedRoot = ActiveRootDirectories[selectedIndex];
                var selectedRootString = selectedRoot.ToString();
                var driveType = ReadyStorageType[selectedIndex];
                StorageVerifyLabel.Visibility = Visibility.Visible;

                // Reconfirm storage drive is actually ready.
                if (!Path.Exists(selectedRootString))
                {
                    CurrentDriveSelection.Content = selectedRootString;
                    StorageVerifyLabel.Content = "\uEA39";
                    StorageVerifyLabel.Foreground = redBrush;
                    ExecuteCommandButton.IsEnabled = false;
                    return;
                }
                mediaValidation = mediaValidation ?? new(this);
                if (!mediaValidation.IsWindowsMedia(selectedRoot, out string _))
                {
                    StorageVerifyLabel.Content = "\uEA39";
                    StorageVerifyLabel.Foreground = redBrush;
                    ExecuteCommandButton.IsEnabled = false;
                    if (!selectionCache.Contains(selectedIndex))
                    {
                        AppendResultsLogText($"Unable to verify [{CurrentDriveSelection.Content.ToString()!.ToUpper()}] storage as valid Microsoft Windows Operating System installation source");
                        selectionCache.Add(selectedIndex);
                    }
                    return;
                }
                ExecuteCommandButton.IsEnabled = true;
                StorageVerifyLabel.Foreground = greenBrush;
            }
            else
            {
                CurrentDriveSelection.Content = "";
                StorageVerifyLabel.Visibility = Visibility.Collapsed;
                ExecuteCommandButton.IsEnabled = false;
                ExecuteCommandButton.Content = "\uF8A5";
                if (noSelection)
                {
                    AppendResultsLogText("No storage drive selected.");
                    noSelection = false;
                }
            }
        }
    }
    private void RefreshDrivesButton_Click(object sender, RoutedEventArgs e)
    {
#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
        if (sender != typeof(FirstRunOnce)) 
        {
            _firstRun = false;  
        }
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
        DriveRefresh();
    }
    private void ResetAll(object sender, RoutedEventArgs e)
    {
        ReadyStorage.Clear();
        ReadyStorageType.Clear();
        ActiveRootDirectories.Clear();
        ReadyDrivesComboBox.ItemsSource = TemporaryMenuTitle;
        ReadyStorage.Add($"Found Local Storage List");
        ReadyDrivesComboBox.Items.Refresh();
        ReadyDrivesComboBox.SelectedIndex = 0;
        StorageVerifyLabel.Visibility = Visibility.Collapsed;
        ExecuteCommandButton.IsEnabled = false;

#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
        if (sender == typeof(FirstRunOnce))
        {
            GroupBoxTop.Visibility = Visibility.Visible;
            GroupBoxMiddle.Visibility = Visibility.Visible;
        }
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ReadyDrivesComboBox.ItemsSource = TemporaryMenuTitle;
        TemporaryMenuTitle.Add("Click the refresh button to populate local storage list.");
        FirstRunOnce.RunOnce();
    }

    #endregion Private Methods

    #region Public Methods

    public void InvokeRefreshDrivesButton_Click(object sender) => RefreshDrivesButton_Click(sender, new RoutedEventArgs());

    public void InvokeResetAll(object sender) => ResetAll(sender, new RoutedEventArgs());

    public void AppendResultsLogText(string text)
    {
        using var sw = new StringWriter();
        sw.WriteLine($"<{DateTime.Now}> {text}");
        ResultsTextBlock.Text += sw.ToString();
    }

    #endregion Public Methods

    public class ExecuteCommandButtonLogic : ICommand
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (!mainWindow.mediaValidation.IsWindowsMedia(mainWindow.ReadyDrivesComboBox.selectedRoot, out string _))
            {
                this.StorageVerifyLabel.Content = "\uEA39";
                StorageVerifyLabel.Foreground = redBrush;
                ExecuteCommandButton.IsEnabled = false;
                if (!selectionCache.Contains(selectedIndex))
                {
                    AppendResultsLogText($"Unable to verify [{CurrentDriveSelection.Content.ToString()!.ToUpper()}] storage as valid Microsoft Windows Operating System installation source");
                    selectionCache.Add(selectedIndex);
                }
                return true;
            }
        }

        public void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}