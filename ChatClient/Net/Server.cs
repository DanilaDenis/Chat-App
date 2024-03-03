using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ChatClient.Net.IO;

namespace ChatClient.Net;

public class Server
{
    private TcpClient _client;
    private PacketBuilder _packetBuilder;
    public PacketReader PacketReader;

    public event Action connectedEvent; 
    public event Action msgReceivedEvent; 
    public event Action userDisconnectEvent; 
    public Server()
    {
        _client = new TcpClient();
    }

    public void ConnectToServer(string username)
    {
        if(!_client.Connected)
        {
            _client.Connect("127.0.0.1", 8080);
            PacketReader = new PacketReader(_client.GetStream());
            if (!string.IsNullOrEmpty(username))
            {
                Console.WriteLine($"[{DateTime.Now}]: {username} has connected");
                var connectPacket = new PacketBuilder();
                connectPacket.WriteOpCode(0);
                connectPacket.WriteMessage(username);
                _client.Client.Send(connectPacket.GetPacketBytes());
            }
            ReadPackets();
        }
        
    }
    private void ReadPackets()
    {
        Task.Run(() =>
        {
            while (true)
            {
                var opcode = PacketReader.ReadByte();
                switch (opcode)
                {
                    case 1:
                        connectedEvent?.Invoke();
                        break;
                    case 5:
                        msgReceivedEvent?.Invoke();
                        break;
                    case 6:
                        msgReceivedEvent?.Invoke();
                        break;
                    case 10:
                        userDisconnectEvent?.Invoke();
                        break;
                        
                    default:
                        Console.WriteLine("yes..");
                        break;
                }
            }
        });
    }
    public void SendMessage(string msg,string sender,string receiver)
    {
        var msgPacket = new PacketBuilder();
        if (receiver == "Everyone")
        {
            msgPacket.WriteOpCode(5);
            msgPacket.WriteMessage(msg);
        }
        else
        {
            msgPacket.WriteOpCode(6);
            msgPacket.WriteMessage(msg);
            msgPacket.WriteMessage(sender);
            msgPacket.WriteMessage(receiver);
        }

        _client.Client.Send(msgPacket.GetPacketBytes());
    }
    public void UserDisconnect(string msg)
    {
        var msgPacket = new PacketBuilder();
        msgPacket.WriteOpCode(10);
        msgPacket.WriteMessage(msg);
        _client.Client.Send(msgPacket.GetPacketBytes());
    }
}