using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MYUDP
{
	public class MyUDP
	{
		
		private String localHost = "127.0.0.1";
		private int localPort = 60000;
		private String remoteHost = "127.0.0.1";
		private int remotePort = 40000;
		private IPEndPoint localEP;
		private IPEndPoint remoteEP;

		private Socket sendUdp;

		public MyUDP ()
		{
			localEP = new IPEndPoint (IPAddress.Parse (localHost), localPort);
			remoteEP = new IPEndPoint (IPAddress.Parse (remoteHost), remotePort);
			sendUdp = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			//Thread thread = new Thread (sendMSG);	
		}


		public  void send (String sessoin, short cmd, byte[] sendbytes)
		{
			using (MemoryStream ms = new MemoryStream ()) {//相当于try cath
				BinaryWriter bs = new BinaryWriter (ms);
				short c = System.Net.IPAddress.HostToNetworkOrder (cmd);
				byte[] bb = System.Text.Encoding.UTF8.GetBytes (sessoin);
				short len = System.Net.IPAddress.HostToNetworkOrder ((short)bb.Length);
				bs.Write (len);
				bs.Write (bb);
				bs.Write (c);
				bs.Write (sendbytes);


				byte[] data = ms.ToArray ();
			
				sendUdp.SendTo (data, remoteEP);
				//sendUdp.Close ();
			}
		}



	}
}

