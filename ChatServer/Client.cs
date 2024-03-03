using System.Net.NetworkInformation;
using System.Net.Sockets;
using ChatServer.Net.IO;

namespace ChatServer;

public class Client
{
    public string Username { get; set; }
    public  Guid UID { get; set; }
    public TcpClient ClientSocket { get; set; }

    private PacketReader _packetReader;
    public Client(TcpClient client)
    {
        ClientSocket = client;
        UID = Guid.NewGuid();
        _packetReader = new PacketReader(ClientSocket.GetStream());

        var opcode = _packetReader.ReadByte();
        //validate opcode; if not drop connection;
        Username = _packetReader.ReadMessage();
        Console.WriteLine($"[{DateTime.Now}]: {Username} has connected");
        Task.Run(Process);
    }

    private void Process()
    {
        while (true)
        {
            try
            {
                var opcode = _packetReader.ReadByte();
                switch (opcode)
                {
                    case 5:
                        var msg = _packetReader.ReadMessage();
                        Console.WriteLine($"[{DateTime.Now}]:Message received!{msg}");
                        Program.BroadCastMessage($"[{Username}]:{msg}",Username);
                        break;
                    case 6:
                        var msgToFriend = _packetReader.ReadMessage();
                        var sender = _packetReader.ReadMessage();
                        var friend = _packetReader.ReadMessage();
                        Console.WriteLine($"[{DateTime.Now}]:Message received!from {sender} to {friend}:{msgToFriend}");
                        Program.BroadCastToFriend($"[{sender}]:{msgToFriend}",sender,friend);
                        break;
                    case 10:
                        var user = _packetReader.ReadMessage();
                        Console.WriteLine($"[{Username}] has disconnected");
                        Program.BroadCastDisconnect($"[{Username}] has disconnected");
                        break;
                }

            }
            catch (Exception)
            {
                Console.WriteLine($"[{UID.ToString()}]:Disconnected");
                Program.BroadCastDisconnect(UID.ToString());
                ClientSocket.Close();
                break;
            }
            
        }
    }
}