using System;
using UnityEngine;
using UnityEngine.UI;
using NetMQ;
using NetMQ.Sockets;

public class MowerZMQClient : MonoBehaviour
{
	public Text valueText;

	RequestSocket socket;
	private void Awake()
	{
		AsyncIO.ForceDotNet.Force();
		socket = new RequestSocket();
		socket.Connect($"tcp://10.42.0.1:7000");
	}

	// Update is called once per frame
	void Update()
	{
		SendAction(valueText.text);
	}

	public void SendAction(String action)
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
		Debug.Log("OnApplicationQuit");
		NetMQConfig.Cleanup();
	}

}