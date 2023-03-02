using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class NowStatus
{

    public static string walls, coins, tnts, finalbox;
    public const string IP = "192.168.200.234", port_socket = "9999", port_http = "8443";
    public static int score = 0;

    public static void foo()
    {
        Console.WriteLine("foo");
    }

    public static void run_func_every_n_seconds(int n, Action func)
    {
        while (true)
        {
            func();
            Thread.Sleep(n * 1000);
        }
    }

    public static void Main()
    {
        from_now_on_update_status_every_n_seconds(1);
        while (true)
        {
            Console.WriteLine("walls: " + walls);
            Console.WriteLine("coins: " + coins);
            Console.WriteLine("tnts: " + tnts);
            Console.WriteLine("finalbox: " + finalbox);
            Thread.Sleep(1000);
        }
    }

    public static void update_now_status()
    {
        string result = http_get(IP, port_http, "api/now-status");
        walls = get_value_from_json(result, "walls");
        coins = get_value_from_json(result, "coins");
        tnts = get_value_from_json(result, "tnts");
        finalbox = get_value_from_json(result, "finalbox");
    }

    public static void from_now_on_update_status_every_n_seconds(int second)
    {
        Thread thread = new Thread(() => run_func_every_n_seconds(second, update_now_status));
        thread.Start();
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

    private static string get_value_from_json(string json, string key)
    {

        json = json.Trim();
        // print
        // Console.WriteLine("get_value_from_json: " + json);
        // assert length
        if (json.Length < 2)
        {
            Console.WriteLine("get_value_from_json: invalid json: " + json + ", return null");
            return null;
        }
        // assert start with {
        if (json[0] != '{')
        {
            Console.WriteLine("get_value_from_json: invalid json: " + json + ", return null");
            return null;
        }
        // assert end with }
        if (json[json.Length - 1] != '}')
        {
            Console.WriteLine("get_value_from_json: invalid json: " + json + ", return null");
            return null;
        }
        // rmv {}
        json = json.Substring(1, json.Length - 2);
        // split
        string[] json_list = json.Split(',');
        foreach (string s in json_list)
        {
            // if not null and not empty
            if (!string.IsNullOrEmpty(s))
            {
                // Console.WriteLine(s);
                string k = s.Split(':')[0].Trim();
                // rmv ""
                k = k.Substring(1, k.Length - 2);
                string v = s.Split(':')[1].Trim();
                // rmv ""
                v = v.Substring(1, v.Length - 2);
                if (k == key)
                {
                    return v;
                }
            }
        }
        Console.WriteLine("get_value_from_json: invalid json" + ", return null");
        return null;
    }
}
