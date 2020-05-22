using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using ComponentAce.Compression.Libs.zlib;

namespace EmuTarkov.Common.Utils.HTTP
{
	public class Request
	{
		public static string Session;
		public string RemoteEndPoint;

		public Request(string session, string remoteEndPoint)
		{
			Session = session;
			RemoteEndPoint = remoteEndPoint;
		}

		private Stream Send(string url, string data = null, bool compress = true)
		{
			// disable SSL encryption
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

			// set session headers
			WebRequest request = WebRequest.Create(new Uri(RemoteEndPoint + url));

			if (!string.IsNullOrEmpty(Session))
			{
				request.Headers.Add("Cookie", $"PHPSESSID={Session}");
				request.Headers.Add("SessionId", Session);
			}

			// set request type and body
			if (!string.IsNullOrEmpty(data))
			{
				byte[] bytes = (compress) ? SimpleZlib.CompressToBytes(data, zlibConst.Z_BEST_COMPRESSION) : Encoding.UTF8.GetBytes(data);

				request.Method = "POST";
				request.ContentType = "application/json";
				request.ContentLength = bytes.Length;

				using (Stream stream = request.GetRequestStream())
				{
					stream.Write(bytes, 0, bytes.Length);
				}
			}
			else
			{
				request.Method = "GET";
			}

			// get response stream
			WebResponse response = request.GetResponse();
			return response.GetResponseStream();
		}

		public string GetJson(string url, string data = null, bool compress = true)
		{
			using (Stream stream = Send(url, data, compress))
			{
				using (MemoryStream ms = new MemoryStream())
				{
					stream.CopyTo(ms);
					return SimpleZlib.Decompress(ms.ToArray(), null);
				}
			}
		}

		public Texture2D GetImage(string url, string data = null, bool compress = true)
		{
			using (Stream stream = Send(url, data, compress))
			{
				using (MemoryStream ms = new MemoryStream())
				{
					Texture2D texture = new Texture2D(8, 8);

					stream.CopyTo(ms);
					texture.LoadImage(ms.ToArray());
					return texture;
				}
			}
		}
	}
}
