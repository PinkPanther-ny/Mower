using UnityEngine;
using System;
using NetMQ;
using NetMQ.Sockets;
using Debug = UnityEngine.Debug;

public class zmqimg : MonoBehaviour
{
    Texture2D t;
    SubscriberSocket subSocket;
    private void Awake()
    {
        AsyncIO.ForceDotNet.Force();
        subSocket = new SubscriberSocket();
        subSocket.Options.ReceiveHighWatermark = 2;
        subSocket.Connect("tcp://10.42.0.1:8000");
        subSocket.Subscribe("");
    }

    void Update()
    {

        string messageReceived = subSocket.ReceiveFrameString();
        Debug.Log(messageReceived.Length);
        byte[] bytebuffer = Convert.FromBase64String(messageReceived);

        Texture2D tex = new Texture2D(1280, 480);
        tex.LoadImage(bytebuffer);
        tex.Apply();
        GetComponent<Renderer>().material.mainTexture = tex;

    }
}