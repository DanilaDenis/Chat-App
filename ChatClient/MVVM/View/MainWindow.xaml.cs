using System;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChatClient.MVVM.Model;
using ChatClient.MVVM.ViewModel;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            (sender as TextBox).Text = string.Empty;
        }
    }
}