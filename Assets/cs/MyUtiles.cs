using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
// using UnityEngine;
public class MyUtiles
{
    public static Random random = new Random();
    public static void Main()
    {
        test3();

    }

    public static void test1()
    {
        Console.WriteLine("Hello World!");
        string test_data = "1,2;-15,15;-11,11;1,1;15,-15;-1,1;1,-1;-15,-15;";
        string[] test_data_list = test_data.Split(';');
        foreach (string s in test_data_list)
        {
            // if not null and not empty
            if (!string.IsNullOrEmpty(s))
            {
                Console.WriteLine(s);
                int x = int.Parse(s.Split(',')[0]);
                int z = int.Parse(s.Split(',')[1]);
                // Console.WriteLine("x: " + x + " z: " + z);
                Console.WriteLine(place2point(x, 0, z));
            }
        }
    }

    public static void test2()
    {
        // 0-9 and A-F
        string test_data = "0123456789ABCDEF";
        foreach (char c in test_data)
        {
            Console.WriteLine(c);
            float[] place = point2place(c.ToString(), center: false);
            Console.WriteLine("x: " + place[0] + " z: " + place[2]);
        }
    }

    public static void test3() {

        string json = "{\"finalbox\":\"0\",\"walls\":\"0111110010010110110110111110011000110011\",\"player_id\":\"0\",\"coins\":\"328\",\"isChanged\":\"false\",\"tnts\":\"66\"}";
        Console.WriteLine(json);
        string player_id = get_value_from_json(json, "walls");
        Console.WriteLine(player_id);

    }

    // @param place: a transform
    // @return: a single char string, from "0" to "9", or "A" to "F", 16 possible values in total
    public static string place2point(float x, float y, float z)
    {
        // look x, z; ignore y
        // for (x, z):
        if (x > 20) {
            x = (float) 19.999;
        }
        if (x < -20) {
            x = (float) -19.999;
        }
        if (z > 20) {
            z = (float) 19.999;
        }
        if (z < -20) {
            z = (float) -19.999;
        }
        
        int value = (4 - (((int)z + 20) / 10) - 1) * 4 + (((int)x) + 20) / 10;
        if (value < 0 || value > 15)
        {
            return "U";
        }
        return value.ToString("X");
    }

    public static float[] point2place(string point, bool center = true)
    {
        // trim
        point = point.Trim();
        // assert length
        if (point.Length != 1)
        {
            Console.WriteLine("point2place: invalid point: " + point + ", return (0, 0, 0)");
            return new float[] { 0, 0, 0 };
        }
        // parse

        int value = int.Parse(point, System.Globalization.NumberStyles.HexNumber);
        float x = (value % 4) * 10 - 20 + 5;
        float z = (4 - (value / 4) - 1) * 10 - 20 + 5;
        if (!center)
        {
            // randomly bias x, z
            // range: -5 ~ 5
            
            float bias1 = (float)random.NextDouble() * 10 - 4;
            float bias2 = (float)random.NextDouble() * 10 - 4;
            x += bias1;
            z += bias2;
        }
        return new float[] { x, 0, z };
    }

    public static string get_value_from_json(string json, string key)
    {
        /*
                    {
            "finalbox": "0",
            "walls": "0111110010010110110110111110011000110011",
            "player_id": "0",
            "coins": "328",
            "isChanged": "false",
            "tnts": "66"
            }
        */
        // trim
        json = json.Trim();
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
        Console.WriteLine("get_value_from_json: invalid json"  + ", return null");
        return null;
    }
}