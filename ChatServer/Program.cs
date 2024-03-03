// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;
using ChatServer;
using ChatServer.Net.IO;

class Program
{
    private static List<Client> _clients;
    private static TcpListener _tcpListener;

    private static void Main(string[] args)
    {
        _clients = new List<Client>();
        _tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
        _tcpListener.Start();
        while (true)
        {
            var client = new Client(_tcpListener.AcceptTcpClient());
            Console.WriteLine("Client Connected");
            _clients.Add(client);
            BroadcastConnection();
        }

    }

    private static void BroadcastConnection()
    {
        foreach (var user in _clients)
        {
            foreach (var usr in _clients)
            {
                if (usr != user)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1);
                    broadcastPacket.WriteMessage(usr.Username);
                    broadcastPacket.WriteMessage(usr.UID.ToString());
                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }

            }
        }
        
    }

    public static void BroadCastMessage(string message,string sender)
    {
        foreach (var user in _clients)
        {
            var msgPacket = new PacketBuilder();
            msgPacket.WriteOpCode(5);
            msgPacket.WriteMessage(message);
            msgPacket.WriteMessage(sender);
            msgPacket.WriteMessage("Everyone");
            user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
        }
    }
    public static void BroadCastToFriend(string message,string sender,string receiver)
    {
        foreach (var user in _clients)
        {
            if (user.Username == receiver || user.Username == sender)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(6);
                msgPacket.WriteMessage(message);
                msgPacket.WriteMessage(sender);
                msgPacket.WriteMessage(receiver);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }

        }
    }
    
    public static void BroadCastDisconnect(string uid)
    {
        var disconnectedUser = _clients.FirstOrDefault(x => x.UID.ToString() == uid);
        _clients.Remove(disconnectedUser);
        foreach (var user in _clients)
        {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10);
                broadcastPacket.WriteMessage(uid);
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
        }

        BroadCastMessage($"[{disconnectedUser.Username}] Disconnected",disconnectedUser.Username);
        
    }
}