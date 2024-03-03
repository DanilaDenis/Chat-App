using System;

namespace ChatClient.MVVM.Model;

public class MessageModel
{
    public string sender { get; set; }
    public string senderColor { get; set; }
    public string receiver { get; set; }
    public string ImageSource { get; set; }

    public string Message { get; set; }
    public bool? FirstMessage { get; set; }
    public override string ToString()
    {
        return Message;
    }
}