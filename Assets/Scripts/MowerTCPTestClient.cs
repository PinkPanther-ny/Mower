// This work is licensed under the Creative Commons Attribution-ShareAlike 4.0 International License. 
// To view a copy of this license, visit http://creativecommons.org/licenses/by-sa/4.0/ 
// or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using NetMQ;
using NetMQ.Sockets;

public class MowerTCPTestClient : MonoBehaviour
{
	public string ip = "10.42.0.1";
	public int port = 10000;
	public Text valueText;


	RequestSocket socket;
	private void Awake()
	{
		AsyncIO.ForceDotNet.Force();
		socket = new RequestSocket();
		socket.Connect($"tcp://10.42.0.1:7000");
	}


	#region private members 	
	private TcpClient socketConnection;
	private Thread clientReceiveThread;
	#endregion
	// Use this for initialization 	
	void Start()
	{
		//ConnectToTcpServer();
	}
	// Update is called once per frame
	void Update()
	{
		SendAction(valueText.text);
	}
	/// <summary> 	
	/// Setup socket connection. 	
	/// </summary> 	
	private void ConnectToTcpServer()
	{
		try
		{
			clientReceiveThread = new Thread(new ThreadStart(ListenForData));
			clientReceiveThread.IsBackground = true;
			clientReceiveThread.Start();
		}
		catch (Exception e)
		{
			Debug.Log("On client connect exception " + e);
		}
	}
	/// <summary> 	
	/// Runs in background clientReceiveThread; Listens for incomming data. 	
	/// </summary>     
	private void ListenForData()
	{
		try
		{
			socketConnection = new TcpClient(ip, port);
			socketConnection.NoDelay = true;

			Byte[] bytes = new Byte[64];
			while (true)
			{
				// Get a stream object for reading 				
				using (NetworkStream stream = socketConnection.GetStream())
				{
					int length;
					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
					{
						var incommingData = new byte[length];
						Array.Copy(bytes, 0, incommingData, 0, length);
						// Convert byte array to string message. 						
						string serverMessage = Encoding.ASCII.GetString(incommingData);
						Debug.Log("server message received as: " + serverMessage);
					}
				}
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}
	/// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	
	public void _SendAction(string action)
	{
		if (socketConnection == null)
		{
			return;
		}
		try
		{
			if (action.StartsWith("X"))
            {
				return;
            }
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream();
			if (stream.CanWrite)
			{
				// Convert string message to byte array.                 
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(action + "|" + action + "|" + action + "|");
				// Write byte array to socketConnection stream.                 
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
				stream.Flush();
				// Debug.Log("Client sent his message - should be received by server");
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
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
		NetMQConfig.Cleanup();
	}

}