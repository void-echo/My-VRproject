//=============================== WXY. ==================================
//
// Purpose: 此脚本用于向接收服务端发送来的数据
//          
//=======================================================================
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;



public class TCPClient : MonoBehaviour
{

    IPEndPoint iPEndPoint;
    Socket socket;
    Thread receiveThread;

    byte[] buffer;                   //缓冲区，存储接收到的数据
    public static bool quit = false; //系统连接标识
    public static string message;    //客户端收到的信息
    public string _ip = NowStatus.IP;
    public static int _port = 9999;
    public static string http_server_port = NowStatus.port_http;
    public const int print_per_times = 10;
    public int __count = 0;
    public static Vector3 head_position = new Vector3(0, 0, 0);
    public static Vector3 another_head_position = new Vector3(0, 0, 0);
    public static Vector3 right_hand_position = new Vector3(0, 0, 0);
    public int player_id = 0;

    // Use this for initialization
    void Start()
    {
        try
        {
            string msg_json = http_get(_ip, "" + http_server_port, "api/init");
            string player_id_str = MyUtiles.get_value_from_json(msg_json, "player_id");
            KabeControl.myself.Start_();
            string kabe_seq = MyUtiles.get_value_from_json(msg_json, "walls");
            player_id = int.Parse(player_id_str);
            this.Init();
            receiveThread = new Thread(new ThreadStart(this.Receive));
            receiveThread.Start();
            KabeControl.updateKabeBySeq(kabe_seq);
            NowStatus.walls = kabe_seq;
            NowStatus.coins = MyUtiles.get_value_from_json(msg_json, "coins");
            NowStatus.tnts = MyUtiles.get_value_from_json(msg_json, "tnts");
            NowStatus.finalbox = MyUtiles.get_value_from_json(msg_json, "finalbox");
            // print player_id, walls, tnts, finalbox
            Debug.Log("---------------- PLAYER_ID: " + player_id + " ----------------");
            NowStatus.from_now_on_update_status_every_n_seconds(1);
        }
        catch (SocketException e)
        {
            // Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// 与服务端建立连接
    /// </summary>
    public void Init()
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("socket created");
            // print ip, port
            Console.WriteLine("ip: " + _ip);
            Console.WriteLine("port: " + _port);

            // print again using Debug
            Debug.Log("socket created");
            Debug.Log("ip: " + _ip);
            Debug.Log("port: " + _port);

            iPEndPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);
            socket.Connect(iPEndPoint);
        }
        catch (SocketException e)
        {
            Debug.Log(e.Message);
        }

    }
    /// <summary>
    /// 接受服务端消息
    /// </summary>
    /// <param name="message">客户端接收到的字节数据</param>
    /// <returns></returns>
    public void Receive()
    {
        while (!quit)
        {
            if (quit) break;
            buffer = new byte[1024 * 1024];
            //获取信息长度
            int n = socket.Receive(buffer);
            //byte转String
            string message = Encoding.UTF8.GetString(buffer, 0, n);
            //数据分割函数，用户自行编写
            Split_data(message);
        }

    }


    void OnApplicationQuit()
    {
        quit = true;
    }

    /// <summary>
    /// 数据分割处理函数，用户自行编写
    /// </summary>
    /// <param name="message">客户端接收到的字节数据</param>
    /// <returns></returns>
    public void Split_data(string message)
    {
        /*
		 * 数据分割说明：
			一层分割——“@”字符分割为单条数据；
			二层分割——“+”字符进行不同设备数据之间的分割，得到单个设备数据，以设备标识字符（G、K...）进行区分；
			三层分割——“#”字符进行单个数据的分割。
		*/
        __count++;
        if (__count < 10)
        {
            return;
        }
        __count = 0;
        try
        {
            // we just keep the message string between the last @ and the second last @
            if (message.Length <= 5) return;
            // ignore '@@' and '@' or other short message. This kind of message is probably broken.
            int last_at = message.LastIndexOf('@');
            int second_last_at = message.LastIndexOf('@', last_at - 1);

            message = message.Substring(second_last_at + 1, last_at - second_last_at - 1);
            // if not contains +, return
            if (message.IndexOf('+') == -1) return;
            // if contains more than one +, we just keep the message before the first +.
            int first_plus = message.IndexOf('+');
            if (first_plus != -1)
            {
                int count = 0;
                for (int i = 0; i < message.Length; i++)
                {
                    if (message[i] == '+') count++;
                }
                // if we have more than one "+", then we have 2 or more devices.
                // now using the 2nd device
                if (player_id == 0)
                {
                    float[] hdpst = KinnectUtil.parseOneComponentPosition(message, 0, "头");
                    float[] rthdpst = KinnectUtil.parseOneComponentPosition(message, 0, "右手");
                    // Debug.Log("updating player 0(myself) head position: " + hdpst[0] + ", " + hdpst[1] + ", " + hdpst[2]);
                    head_position.x = hdpst[0];
                    head_position.y = hdpst[1];
                    head_position.z = hdpst[2];
                    right_hand_position.x = rthdpst[0];
                    right_hand_position.y = rthdpst[1];
                    right_hand_position.z = rthdpst[2];
                    if (count > 1)
                    {
                        float[] hdpst1 = KinnectUtil.parseOneComponentPosition(message, 1, "头");
                        // Debug.Log("updating player 1(another) head position: " + hdpst1[0] + ", " + hdpst1[1] + ", " + hdpst1[2]);
                        another_head_position.x = hdpst1[0];
                        another_head_position.y = hdpst1[1];
                        another_head_position.z = hdpst1[2];
                    }
                }
                else
                {
                    if (count > 1)
                    {
                        float[] hdpst = KinnectUtil.parseOneComponentPosition(message, 1, "头");
                        float[] rthdpst = KinnectUtil.parseOneComponentPosition(message, 1, "右手");
                        // Debug.Log("updating player 1(myself) head position: " + hdpst[0] + ", " + hdpst[1] + ", " + hdpst[2]);
                        head_position.x = hdpst[0];
                        head_position.y = hdpst[1];
                        head_position.z = hdpst[2];
                        right_hand_position.x = rthdpst[0];
                        right_hand_position.y = rthdpst[1];
                        right_hand_position.z = rthdpst[2];
                    }
                    float[] hdpst1 = KinnectUtil.parseOneComponentPosition(message, 0, "头");
                    // Debug.Log("updating player 0(another) head position: " + hdpst1[0] + ", " + hdpst1[1] + ", " + hdpst1[2]);
                    another_head_position.x = hdpst1[0];
                    another_head_position.y = hdpst1[1];
                    another_head_position.z = hdpst1[2];
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

    }

    public void parseHeadPositionAndUpdate(String message)
    {
        try
        {
            String head_position_str = message.Split('#')[5];
            // head_position_str is like this: '(x, y, z)'
            float x = float.Parse(head_position_str.Split(',')[0].Substring(1));
            float y = float.Parse(head_position_str.Split(',')[1]);
            float z = float.Parse(head_position_str.Split(',')[2].Substring(0, head_position_str.Split(',')[2].Length - 1));


            // update the element
            head_position.x = x;
            head_position.y = y;
            head_position.z = z;
            // Debug.Log("head position: " + head_position);
        } // catch index out of range exception
        catch (Exception e)
        {
            return;
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
        // Console.WriteLine(response.StatusDescription);
        // Get the stream containing content returned by the server.
        System.IO.Stream dataStream = response.GetResponseStream();
        // Open the stream using a StreamReader for easy access.
        System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
        // Read the content.
        content = reader.ReadToEnd();
        return content.Trim();
    }

}
