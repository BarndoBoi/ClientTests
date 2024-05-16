using System;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using TMPro;

public class Client : MonoBehaviour
{
    WebSocket socket;
    [SerializeField]
    TextMeshProUGUI chatBox;
    [SerializeField]
    string User;

    // Start is called before the first frame update
    void Start()
    {
        socket = new WebSocket("ws://localhost:42069");

        socket.OnOpen += Connect;
        socket.OnMessage += Receive;
        socket.Connect();

    }

    private void Receive(object sender, MessageEventArgs e)
    {
        if (e.IsText)
        { //We're expecting a JSON object to come through here
            Debug.Log(e.Data);
            BaseMessageJSON message = JsonConvert.DeserializeObject<BaseMessageJSON>(e.Data);
            message.json = e.Data;
            RouteMessage(message);
        }
        else
        {
            Debug.LogWarning("Received unhandled data object instead of text!");
        }

    }

    private void Connect(object sender, EventArgs e)
    {
        Debug.Log("Connected to server");
        var connectMessage = BaseMessageJSON.ConnectMessage(User);
        var json = JsonConvert.SerializeObject(connectMessage);
        socket.Send(json);
        connectMessage.json = json;
    }

    private void OnApplicationQuit()
    {
        socket.Close();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            socket.Send("Hello World!");
        }
    }

    public void SendChat(string message)
    {
        Debug.Log("Sending chat message...");
        var chatMessage = BaseMessageJSON.ChatMessage(User, message);
        var json = JsonConvert.SerializeObject(chatMessage);
        socket.Send(json);
        chatMessage.json = json;
    }

    private void HandleChat(BaseMessageJSON json)
    {
        //Do chat UI stuff here
        Debug.Log("Received chat message");
        Debug.Log("Contents: " + json.json);
        string username = User == json.user ? "You" : json.user;
        string chatMessage = username + ": " + json.message;
        chatBox.text += chatMessage + "<br>";
        chatBox.ForceMeshUpdate(true);
    }

    private void HandleJoin(BaseMessageJSON json)
    {
        //Add other clients in here
        Debug.Log("Received join message");
        Debug.Log("Contents: " + json.json);
    }

    private void RouteMessage(BaseMessageJSON message)
    {
        switch (message.type)
        {
            case "chat":
                HandleChat(message);
                break;
            case "join":
                HandleJoin(message);
                break;
        }
    }
}
