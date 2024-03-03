using System.Net.Sockets;
using System.Text;

namespace ChatServer.Net.IO;

public class PacketReader : BinaryReader
{
    private NetworkStream _networkStream;
    public PacketReader(NetworkStream ns) : base(ns)
    {
        _networkStream = ns;
    }

    public string ReadMessage()
    {
        byte[] msgBuffer;
        var lenght = ReadInt32();
        msgBuffer = new byte[lenght];
        _networkStream.Read(msgBuffer, 0, lenght);
        var msg = Encoding.ASCII.GetString(msgBuffer);
        return msg;
    }
    
}