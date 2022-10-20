using UnityEngine;
using NetMQ;
using NetMQ.Sockets;
using System;

public class ZMQMower : MonoBehaviour
{
    RequestSocket socket;
    private void Awake()
    {
        AsyncIO.ForceDotNet.Force();
        socket = new RequestSocket();
        socket.Connect($"tcp://10.42.0.1:7000");
    }

    private void Update()
    {
        RequestMessage("A20_20");
    }

    public void RequestMessage(String action)
    {
        var messageReceived = false;
        var message = "";
        var timeout = new TimeSpan(0, 0, 2);

        if (socket.TrySendFrame(action))
        {
            messageReceived = socket.TryReceiveFrameString(timeout, out message);
        }
    }

    private void OnApplicationQuit()
    {
        NetMQConfig.Cleanup();
    }


}
