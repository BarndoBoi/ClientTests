using System;
using UnityEngine;
using WebSocketSharp;

public class Client : MonoBehaviour
{
    WebSocket socket;

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
            Debug.Log(e.Data);

    }

    private void Connect(object sender, EventArgs e)
    {
        Debug.Log("Connected to server");
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
}
