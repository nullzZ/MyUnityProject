using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Action;

namespace MY_NET
{
	public class MySocket
	{

		private Socket clientSocket;
		//private Encoder encoder;
		List<byte[]> worldpackage;


		//		public MySocket (AsyncCallback asyncCallback)
		//		{
		//			this.asyncCallback = asyncCallback;
		//		}

		public bool onConnet (String  host, int port)
		{
			clientSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			clientSocket.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
			clientSocket.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);

			IPAddress ipAddress = IPAddress.Parse (host);
		  
			IPEndPoint ipEndpoint = new IPEndPoint (ipAddress, port);
		    
			IAsyncResult result = clientSocket.BeginConnect (ipEndpoint, new AsyncCallback (connectCallback), clientSocket);

			bool success = result.AsyncWaitHandle.WaitOne (5000, true);
			if (!success) {
				Closed ();
				Debug.Log ("connect Time Out");
			} else {
				worldpackage = new List<byte[]> ();
				Thread thread = new Thread (new ThreadStart (ReceiveSorket));
				thread.IsBackground = true;
				thread.Start ();
			}
			return success;
	
		}

		private void connectCallback (IAsyncResult asyncConnect)
		{
			Debug.Log ("connectSuccess");
		}

		private void ReceiveSorket ()
		{
			while (true) {	 
				if (!clientSocket.Connected) {
					Debug.Log ("Failed to clientSocket server.");
					clientSocket.Close ();
					break;
				}
				try {
					//接受数据保存至bytes当中
					byte[] bytes = new byte[4096];
					int i = clientSocket.Receive (bytes);
					if (i <= 0) {
						clientSocket.Close ();
						break;
					}  

					if (bytes.Length > 2) {
						Debug.Log ("接收到数据");
						SplitPackage (bytes, 0);
					} else {
						Debug.Log ("length is not  >  2");
					}
					 
				} catch (Exception e) {
					Debug.LogError ("Failed to clientSocket error." + e);
					clientSocket.Close ();
					break;
				}
			}
		}

		private void SplitPackage (byte[] bytes, int index)
		{
			while (true) {
				byte[] head = new byte[2];
				Array.Copy (bytes, index, head, 0, 2);
				short len = System.Net.IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (head, 0));

				Debug.Log ("[消息]len:" + len);

				if (len > 0) {
					byte[] cmdB = new byte[2];
					Array.Copy (bytes, 2, cmdB, 0, 2);
					short cmd = System.Net.IPAddress.NetworkToHostOrder (BitConverter.ToInt16 (cmdB, 0));

					Debug.Log ("cmd===" + cmd);

					int headLengthIndex = index + 4;
					int protoLen = len - 2;
					byte[] data = new byte[protoLen];
					Array.Copy (bytes, headLengthIndex, data, 0, protoLen);	

					//worldpackage.Add (data);
						 
					if (ActionManager.Instance.isHasCmd (cmd)) {
						ActionManager.Instance.getAction (cmd).excute (data);
					} else {
						Debug.LogError ("cmd不存|" + cmd);
					}


					index = headLengthIndex + len;			
				} else {
					break;
				}
			}
		}

		public void send (short cmd, byte[] data)
		{
			using (MemoryStream ms = new MemoryStream ()) {//相当于try cath
				BinaryWriter bs = new BinaryWriter (ms);
				short l = (short)(2 + data.Length);
				short len = System.Net.IPAddress.HostToNetworkOrder (l);
				short c = System.Net.IPAddress.HostToNetworkOrder (cmd);
				bs.Write (len);
				bs.Write (c);
				bs.Write (data);
				SendMessage (ms.ToArray ());
			}
		}

		//向服务端发送数据包，也就是一个结构体对象
		public void SendMessage (byte[] data)
		{
			
			if (!clientSocket.Connected) {
				clientSocket.Close ();
				return;
			}
			try {
				//计算出新的字节数组的长度
				//int length = Marshal.SizeOf(size) + Marshal.SizeOf(obj);
				Debug.Log ("data len " + data.Length);
				if (data.Length == 0) {
					return;
				}

				//向服务端异步发送这个字节数组
				IAsyncResult asyncSend = clientSocket.BeginSend (data, 0, data.Length, SocketFlags.None, new AsyncCallback (sendCallback), clientSocket);
				//监测超时
				bool success = asyncSend.AsyncWaitHandle.WaitOne (5000, true);
				if (!success) {
					clientSocket.Close ();
					Debug.Log ("Time Out !");
				}

			} catch (Exception e) {
				Debug.Log ("send message error: " + e);
			}
		}

		private void sendCallback (IAsyncResult asyncSend)
		{
			Debug.Log ("发送成功------");
		}

		public void Closed ()
		{
			if (clientSocket != null && clientSocket.Connected) {
				clientSocket.Shutdown (SocketShutdown.Both);
				clientSocket.Close ();
			}
			clientSocket = null;
		}

		public bool isConnect ()
		{
			if (!clientSocket.Connected) {
				return false;
			} else {
				return true;
			}
		}
	}
}