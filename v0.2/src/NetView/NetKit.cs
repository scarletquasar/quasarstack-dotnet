using System;
using System.Net.NetworkInformation;
using System.Net;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace quasarStack.NetView
{

	public static class NetKit
	{

		public static string[] ListTCP(string method = "none")
		{
			List<string> tcpList = new List<string>();
			if (method == "show-comment-table") { tcpList.Add("Local End Point | Remote End Point"); }
			IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
			TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
			foreach (TcpConnectionInformation c in connections)
			{
				tcpList.Add(c.LocalEndPoint.ToString() + "|" + c.RemoteEndPoint.ToString());
			}

			return tcpList.ToArray();

		}

		public static float FastPing(string target)
		{
			/*
			 * ==================================================================
 			 *  PING THE TARGET ADDRESS AND RETURN THE ROUNDSTRIP TIME
 			 * ==================================================================
 			 */
			try
			{
				Ping pingSender = new Ping();
				PingReply reply = pingSender.Send(target);
				return reply.RoundtripTime;
			}
			catch
			{
				General.ReportError("Fatal Error: Can't acess the function Ping().Send(target)");
			}
			return 0;

		}

		public static float[] MultiPing(string target, int requestLength)
		{
			/*
			 * ==================================================================
 			 *  PING THE TARGET ADDRESS MULTIPLE TIMES AND RETURN THE ROUNDSTRIP TIME
 			 * ==================================================================
 			 */
			List<float> requestResults = new List<float>();
			try
			{
				for (int maxRequests = requestLength; maxRequests > 0; maxRequests -= 1)
				{
					Ping pingSender = new Ping();
					PingReply reply = pingSender.Send(target);
					requestResults.Add(reply.RoundtripTime);
				}
				return requestResults.ToArray();
			}
			catch
			{
				General.ReportError("Fatal Error: Can't acess the function Ping().Send(target)");
			}
			return null;
		}

		public static string[] CurrentDNS()
		{
			/*
			 * ==================================================================
 			 *  RETURN THE CURRENT DNS SERVERS CONFIGURED IN THE SYSTEM
 			 * ==================================================================
 			 */
			List<string> dnsServers = new List<string>();
			try
			{
				NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

				foreach (NetworkInterface networkInterface in networkInterfaces)
				{
					if (networkInterface.OperationalStatus == OperationalStatus.Up)
					{
						IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();
						IPAddressCollection dnsAddresses = ipProperties.DnsAddresses;

						foreach (IPAddress dnsAdress in dnsAddresses)
						{
							dnsServers.Add(dnsAdress.ToString());
						}

						return dnsServers.ToArray();
					}
				}
			}
			catch
			{
				return null;
			}
			return null;
		}

	}
}