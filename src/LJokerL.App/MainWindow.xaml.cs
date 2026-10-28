using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using LJokerL.App.ViewModels;

namespace LJokerL.App;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private static readonly Regex NumericRegex = new("^[0-9]*$");

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
        Closed += (_, _) => (DataContext as MainViewModel)?.StopGameWatcher();
    }

    private void OnNumericTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !IsNumeric(e.Text);
    }

    private void OnNumericPasting(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(DataFormats.Text))
        {
            var text = e.DataObject.GetData(DataFormats.Text) as string ?? string.Empty;
            if (!IsNumeric(text))
            {
                e.CancelCommand();
            }
        }
        else
        {
            e.CancelCommand();
        }
    }

    private static bool IsNumeric(string text)
    {
        return NumericRegex.IsMatch(text);
    }
}
