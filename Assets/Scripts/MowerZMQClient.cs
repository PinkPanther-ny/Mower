using System;
using UnityEngine;
using UnityEngine.UI;
using NetMQ;
using NetMQ.Sockets;

public class MowerZMQClient : MonoBehaviour
{
	public Text valueText;
	
	RequestSocket socket;
	private void Start()
	{
		AsyncIO.ForceDotNet.Force();
		socket = new RequestSocket();
		socket.Connect($"tcp://10.42.0.1:7000");
		Debug.Log(socket);
	}

	// Update is called once per frame
	void Update()
	{
		SendAction(valueText.text);
	}

	public void SendAction(String action)
	{
		var message = "";
		var timeout = new TimeSpan(0, 0, 2);
        try
        {
			socket.TrySendFrame(action);
        }
        catch (FiniteStateMachineException)
        {
			socket.TryReceiveFrameString(timeout, out message);
			Debug.Log("TrySendFrame");
		}


		try
		{
			socket.TryReceiveFrameString(timeout, out message);
		}
		catch (FiniteStateMachineException)
		{
			socket.TrySendFrame(action);
			Debug.Log("TryReceiveFrameString");
		}
	}

	private void OnApplicationQuit()
	{
		Debug.Log("OnApplicationQuit");
		NetMQConfig.Cleanup();
	}

}