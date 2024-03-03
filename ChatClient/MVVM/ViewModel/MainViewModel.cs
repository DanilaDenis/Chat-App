using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ChatClient.Core;
using ChatClient.MVVM.Model;
using ChatClient.Net;

namespace ChatClient.MVVM.ViewModel;

public class MainViewModel : ObservableObject
{
    public  RelayCommand SendMessageCommand { get; set; }
    private UserModel _selectedUser;
    public string Text { get; set; }

    public UserModel SelectedUser
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            OnPropertyChanged();
            ChangeMessages();
        }
    }

    private Server _server { get; set; }
    public ObservableCollection<UserModel> Users { get; set; }
    public ObservableCollection<MessageModel> Messages { get; set; }
    public ObservableCollection<MessageModel> ShownMessages { get; set; }
    public string Username { get; set; }
    private string _message;

    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        } }

    public MainViewModel(Server server)
    {
        Text = "@Message";
        Users = new ObservableCollection<UserModel>();
        Messages = new ObservableCollection<MessageModel>();
        ShownMessages = new ObservableCollection<MessageModel>();
        Users.Add(new UserModel{Username = "Everyone"});
        SelectedUser = Users.First();
        _server = server;
        _server.connectedEvent += UserConnected;
        _server.userDisconnectEvent += RemoveUser;
        _server.msgReceivedEvent += MsgReceived;
        
        SendMessageCommand = new RelayCommand(o => _server.SendMessage(Message,Username,SelectedUser.Username), O =>!string.IsNullOrEmpty(Message) && SelectedUser.Username != "");
    }

    private void ChangeMessages()
    {
        if (Messages.Count == 0) return;
        ShownMessages.Clear();
        if (SelectedUser.Username == "Everyone")
        {
            foreach (var msg in Messages.Where(o=>o.receiver == "Everyone"))
            {
                Application.Current.Dispatcher.Invoke(() => ShownMessages.Add(msg));
            }
        }
        else
        {
            foreach (var msg in Messages.Where(o=> CheckParticipants(o) && o.receiver != "Everyone"))
            {
                Application.Current.Dispatcher.Invoke(() => ShownMessages.Add(msg));
            }
        }

        
    }

    private bool CheckParticipants(MessageModel model)
    {
        return (model.sender == SelectedUser.Username && model.receiver == Username) ||
               (model.sender == Username && model.receiver == SelectedUser.Username);
    }
    private void UserConnected()
    {
        var user = new UserModel
        {
            Username = _server.PacketReader.ReadMessage(),
            UID = _server.PacketReader.ReadMessage()
        };
        if (Users.All(x => x.UID != user.UID))
        {
            Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            
        }
    }
    private void RemoveUser()
    {
        var uid = _server.PacketReader.ReadMessage();
        var user = Users.FirstOrDefault(x => x.UID == uid);
        if (SelectedUser.Username == user.Username)
        {
            Application.Current.Dispatcher.Invoke(() =>  SelectedUser = Users.First(o => o.Username == "Everyone"));
        }
        
        Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
 }
    private void MsgReceived()
    {
        var msg = _server.PacketReader.ReadMessage();
        var sendr = _server.PacketReader.ReadMessage();
        var rcvr = _server.PacketReader.ReadMessage();
            var message = new MessageModel
        {
            Message = msg,
            sender = sendr,
            receiver = rcvr
        };
        Application.Current.Dispatcher.Invoke(() => Messages.Add(message));
        if (CheckParticipants(message) || (_selectedUser.Username=="Everyone" && message.receiver == "Everyone"))
        {
            Application.Current.Dispatcher.Invoke(() => ShownMessages.Add(message));
        }
        
        Message = "";

    }
}