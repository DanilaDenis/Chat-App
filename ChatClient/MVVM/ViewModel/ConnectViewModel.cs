using System.Windows;
using ChatClient.Core;
using ChatClient.Net;

namespace ChatClient.MVVM.ViewModel;

public class ConnectViewModel
{
    public string Username { get; set; }
    public string Text { get; set; }
    public RelayCommand Command { get;}

    private Server _server;

    public ConnectViewModel()
    {
        Command = new RelayCommand(o => HandleConnect());
    }

    private void HandleConnect()
    {
        if (Username is null or "Everyone" or "") {  return; }
        _server = new Server();
        var mainWindow = new MainWindow
        {
            DataContext = new MainViewModel(_server)
        };
        
        ((MainViewModel)mainWindow.DataContext).Username = Username;
        _server.ConnectToServer(Username);
        mainWindow.Show();
        if (Application.Current.MainWindow != null) Application.Current.MainWindow.Close();
        
    }
}