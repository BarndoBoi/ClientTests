using System;
using UnityEngine;

public class BaseMessageJSON
{
    public string type;
    public string message;
    public string user;
    [NonSerialized]
    public string json;

    public static BaseMessageJSON ConnectMessage(string user, string json = null)
    {
        var connect = new BaseMessageJSON();
        connect.type = "join";
        connect.user = user;
        //connect.Message = ""; //See if this works with a null value
        return connect;
    }

    public static BaseMessageJSON ChatMessage(string user, string message, string json = null)
    {
        var chat = new BaseMessageJSON();
        chat.type = "chat";
        chat.user = user;
        chat.message = message;
        return chat;
    }
}
