using System;
using System.IO;
using System.Text;

namespace ChatClient.Net.IO;

public class PacketBuilder
{
    private MemoryStream _memoryStream;

    public PacketBuilder()
    {
        _memoryStream = new MemoryStream();
    }

    public void WriteOpCode(byte opcode)
    {
        _memoryStream.WriteByte(opcode);
    }

    public void WriteMessage(string msg)
    {
        var msgLenght = msg.Length;
        _memoryStream.Write(BitConverter.GetBytes(msgLenght));
        _memoryStream.Write(Encoding.ASCII.GetBytes(msg));
    }

    public byte[] GetPacketBytes()
    {
        return _memoryStream.ToArray();
    }
}