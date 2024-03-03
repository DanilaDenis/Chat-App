using System.Windows;
using System.Windows.Input;
using ChatClient.MVVM.ViewModel;

namespace ChatClient.MVVM.View;

public partial class ConnectWindow : Window
{
    public ConnectWindow()
    {
        InitializeComponent();
    }
    private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }
    private void ButtonMinimize(object sender, RoutedEventArgs e)
    {
        SystemCommands.MinimizeWindow(this);
    }

    private void ButtonMaximize(object sender, RoutedEventArgs e)
    {
        switch (WindowState.ToString())
        {
            case "Maximized":
                SystemCommands.RestoreWindow(this);
                break;
            case "Normal":
                SystemCommands.MaximizeWindow(this);
                break;
        }
    }

    private void ButtonClose(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void CommandBinding_CanExecute_1(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
}