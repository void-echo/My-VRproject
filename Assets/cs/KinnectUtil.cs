using System;
using System.Collections;
using System.Collections.Generic;

// import system regex for string split
using System.Text.RegularExpressions;

public class KinnectUtil
{
    public static void Main()
    {
        String msg1 = "K#0#(-0.22, 1.97, 1.81)#(-0.15, 1.95, 1.85)#(-0.15, 1.89, 1.86)#(-0.11, 1.95, 1.83)#(-0.06, 1.84, 1.95)#(-0.22, 1.75, 1.78)#(-0.21, 1.75, 1.78)#(-0.23, 1.52, 1.73)#(-0.22, 1.52, 1.73)#(-0.23, 1.46, 1.74)#(-0.22, 1.47, 1.75)#(-0.22, 1.46, 1.74)#(-0.19, 1.45, 1.75)#(-0.23, 1.42, 1.70)#(-0.21, 1.41, 1.72)#(-0.14, 1.68, 1.89)#(-0.14, 1.39, 1.87)#(-0.11, 1.37, 1.92)#(-0.20, 1.03, 1.78)#(-0.07, 1.05, 2.05)#(-0.22, 0.61, 1.76)#(-0.06, 0.62, 2.18)#(-0.15, 0.61, 1.71)#(-0.02, 0.61, 2.14)#(-0.13, 1.39, 1.92)#+";

        String msg2 = "K#0#(0.03, 2.04, 2.07)#(-0.03, 1.94, 2.07)#(-0.03, 1.88, 2.08)#(-0.15, 1.83, 2.01)#(0.09, 1.84, 2.19)#(-0.23, 1.61, 1.98)#(0.14, 1.66, 2.32)#(-0.17, 1.40, 1.95)#(0.20, 1.50, 2.25)#(-0.16, 1.37, 1.96)#(0.17, 1.47, 2.22)#(-0.19, 1.37, 1.95)#(0.16, 1.44, 2.21)#(-0.14, 1.31, 1.95)#(0.16, 1.40, 2.19)#(-0.02, 1.67, 2.09)#(-0.06, 1.39, 2.05)#(0.03, 1.38, 2.12)#(-0.05, 1.02, 2.02)#(0.02, 1.02, 2.12)#(-0.05, 0.62, 2.05)#(-0.01, 0.62, 2.18)#(-0.01, 0.61, 1.97)#(0.05, 0.61, 2.13)#(-0.01, 1.39, 2.11)#+K#1#(0.40, 1.62, 1.50)#(0.39, 1.50, 1.57)#(0.39, 1.44, 1.59)#(0.23, 1.43, 1.61)#(0.56, 1.37, 1.57)#(0.15, 1.19, 1.66)#(0.68, 1.20, 1.55)#(0.19, 1.00, 1.50)#(0.61, 1.01, 1.47)#(0.21, 0.93, 1.44)#(0.58, 0.99, 1.47)#(0.25, 0.95, 1.42)#(0.51, 1.00, 1.45)#(0.22, 0.87, 1.41)#(0.52, 0.89, 1.39)#(0.40, 1.27, 1.67)#(0.32, 1.05, 1.74)#(0.47, 1.01, 1.72)#(0.21, 0.95, 1.46)#(0.48, 0.97, 1.40)#(0.31, 0.64, 1.69)#(0.47, 0.62, 1.68)#(0.25, 0.60, 1.67)#(0.53, 0.60, 1.65)#(0.40, 1.03, 1.75)#+";
        
        float[] pos1 = parseOneComponentPosition(msg1, 0, "头");
        float[] pos2_0 = parseOneComponentPosition(msg2, 0, "头");
        float[] pos2_1 = parseOneComponentPosition(msg2, 1, "头");

        Console.WriteLine("pos1: " + pos1[0] + ", " + pos1[1] + ", " + pos1[2]);
        Console.WriteLine("pos2_0: " + pos2_0[0] + ", " + pos2_0[1] + ", " + pos2_0[2]);
        Console.WriteLine("pos2_1: " + pos2_1[0] + ", " + pos2_1[1] + ", " + pos2_1[2]);
    }
    private static Dictionary<String, int> pos2index = new Dictionary<String, int>();

    private static bool set_ = false;
    
    public static float[]  parseOneComponentPosition(string cookedMsg, int uid, string posStr)
    {
        if (!set_) {
            prepare_dict();
        }
        try {
            // if end with "#+", then remove it
            if (cookedMsg.EndsWith("#+")) {
                cookedMsg = cookedMsg.Substring(0, cookedMsg.Length - 2);
            }
            // string[] parts = cookedMsg.Split("#+");
            
            // split with string "#+"
            string[] parts = Regex.Split(cookedMsg, "#\\+");
            // if (parts.Length != 1 && parts.Length != 2) {
            //     Console.WriteLine("Error: parts.Length != 1 && parts.Length != 2");
            //     return null;
            // }
            int index = getIndexFromComponent(posStr);
            if (index == -1) {
                Console.WriteLine("Error: index == -1");
                return null;
            }
            string[] posParts = parts[uid].Split('#');
            return parseOnePosition(posParts[index + 2]);
        } catch (Exception e) {
            Console.WriteLine(e);
            return null;
        }
    }

    private static int getIndexFromComponent(string component) {
        if (pos2index.ContainsKey(component)) {
            return pos2index[component];
        } else {
            return -1;
        }
    }

    private static void prepare_dict() {
        set_ = true;
        pos2index.Add("脊椎底", 0);
        pos2index.Add("脊椎中", 1);
        pos2index.Add("颈", 2);
        pos2index.Add("头", 3);
        pos2index.Add("左肩", 4);
        pos2index.Add("左肘", 5);
        pos2index.Add("左腕", 6);
        pos2index.Add("左手", 7);
        pos2index.Add("右肩", 8);
        pos2index.Add("右肘", 9);
        pos2index.Add("右腕", 10);
        pos2index.Add("右手", 11);
        pos2index.Add("左臀", 12);
        pos2index.Add("左膝", 13);
        pos2index.Add("左脚踝", 14);
        pos2index.Add("左脚趾", 15);
        pos2index.Add("右臀", 16);
        pos2index.Add("右膝", 17);
        pos2index.Add("右脚踝", 18);
        pos2index.Add("右脚趾", 19);
        pos2index.Add("肩膀中", 20);
        pos2index.Add("左手指尖", 21);
        pos2index.Add("左手大拇指", 22);
        pos2index.Add("右手指尖", 23);
        pos2index.Add("右手大拇指", 24);
    }

    private static float[] parseOnePosition(string posStr) {
        // example: (0.03, 2.04, 2.07)
        try {
            string[] parts = posStr.Split(',');
            if (parts.Length != 3) {
                Console.WriteLine("Error: parts.Length != 3");
                return null;
            }
            float[] pos = new float[3];
            pos[0] = float.Parse(parts[0].Substring(1));
            pos[1] = float.Parse(parts[1]);
            pos[2] = float.Parse(parts[2].Substring(0, parts[2].Length - 1));
            return pos;
        } catch (Exception ignored) {
            return null;
        }
    }
}