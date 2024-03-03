using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ChatClient.Net.IO;

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
        var length = ReadInt32();
        msgBuffer = new byte[length];
        _networkStream.Read(msgBuffer, 0, length);
        var msg = Encoding.ASCII.GetString(msgBuffer);
        return msg;
    }
}