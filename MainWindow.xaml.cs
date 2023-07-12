using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    #region Private Methods

    private void DriveRefresh()
    {
        availableDrivesDropDownMenu.Items.Clear();
       _ = new GetDriveList(this).Refresh();





    }

    private void refreshDrivesButton_Click(object sender, RoutedEventArgs e)
    {
        DriveRefresh();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        DriveRefresh();

    }

    #endregion

    private void availableDrivesDropDownMenu_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        driveSelectionConfirmation.Content = availableDrivesDropDownMenu.SelectedItem.ToString()!.Remove(0, 8);
    }
}