using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
public class Test
{
    public static string _ip = "192.168.200.234";
    public static int http_port = 8443;
    public static int kinnect_port = 9999;
    public static IPEndPoint iPEndPoint;
    public static Socket socket;
    public static Thread receiveThread;

    public static byte[] buffer;                   //缓冲区，存储接收到的数据
    public static bool quit = false; //系统连接标识
    public static string message;    //客户端收到的信息
    public static void Main(string[] args)
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            iPEndPoint = new IPEndPoint(IPAddress.Parse(_ip), kinnect_port);
            socket.Connect(iPEndPoint);
        }
        catch (SocketException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static string http_get(string url, string port, string argument)
    {
        WebRequest request = WebRequest.Create("http://" + url + ":" + port + "/" + argument);
        // If required by the server, set the credentials.
        string content = "";
        request.Credentials = CredentialCache.DefaultCredentials;
        // Get the response.
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        // Display the status.
        Console.WriteLine(response.StatusDescription);
        // Get the stream containing content returned by the server.
        System.IO.Stream dataStream = response.GetResponseStream();
        // Open the stream using a StreamReader for easy access.
        System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
        // Read the content.
        content = reader.ReadToEnd();
        return content.Trim();
    }
}

