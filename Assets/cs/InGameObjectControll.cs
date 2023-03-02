using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameObjectControll : MonoBehaviour
{
    public GameObject tntPrefab, coinPrefab, finalBoxPrefab;
    public static string old_tnts = null, old_coins = null, old_finalBox = null, old_walls = null;
    public static InGameObjectControll myself = null;
    public static int __counter = 0;

    void Start()
    {
        myself = this;
        render_one_from_status("tnts");
        render_one_from_status("coins");
        render_one_from_status("finalbox");
        // render_one_from_status("walls");

        
    }

    void Update()
    {
        __counter += 1;
        if (__counter > 500)
        {
            updateAllFromStatus();
            __counter = 0;
        }
    }



    public static void updateAllFromStatus()
    {
        /*
        "finalbox": "0",
        "walls": "1111111111111111111111111111111111111111",
        "coins": "328",
        "tnts": "66"
        */
        if (old_tnts == null)
        {
            old_tnts = NowStatus.tnts;
            Debug.Log("initing old_tnts: " + old_tnts);
            render_one_from_status("tnts");
        }
        if (old_coins == null)
        {
            old_coins = NowStatus.coins;
            Debug.Log("initing old_coins: " + old_coins);
            render_one_from_status("coins");
        }
        if (old_finalBox == null)
        {
            old_finalBox = NowStatus.finalbox;
            Debug.Log("initing old_finalBox: " + old_finalBox);
            render_one_from_status("finalbox");
        }
        if (old_walls == null)
        {
            old_walls = NowStatus.walls;
            Debug.Log("initing old_walls: " + old_walls);
            render_one_from_status("walls");
        }

        foreach (string type_ in new string[] { "tnts", "coins", "finalbox", "walls" })
        {
            render_one_from_status(type_);
        }
    }

    public static void render_one_from_status(string type_) {
        // check type_ in ["tnts", "coins", "finalbox", "walls"]
        // check value_ is a hex single char string
        if (type_ == "tnts") {
            // check if changed
            if (old_tnts != NowStatus.tnts) {
                // update old_tnts
                old_tnts = NowStatus.tnts;
                // render
                render_tnts();
            }
        } else if (type_ == "coins") {
            // check if changed
            if (old_coins != NowStatus.coins) {
                // update old_coins
                old_coins = NowStatus.coins;
                // render
                render_coins();
            }
        } else if (type_ == "finalbox") {
            // check if changed
            if (old_finalBox != NowStatus.finalbox) {
                // update old_finalBox
                old_finalBox = NowStatus.finalbox;
                // render
                render_finalBox();
            }
        } else if (type_ == "walls") {
            // check if changed
            if (old_walls != NowStatus.walls) {
                // update old_walls
                old_walls = NowStatus.walls;
                // render
                render_walls();
            }
        } else {
            Debug.Log("error: type_ is not in [tnts, coins, finalbox, walls]. Got: " + type_);
        }
    }

    public static void render_tnts() {
        Debug.Log("rendering tnts");
        // firstly delete all objects tagged "tnt"
        GameObject[] tnts = GameObject.FindGameObjectsWithTag("tnt");
        foreach (GameObject tnt in tnts) {
            // inactivate tnt
            tnt.SetActive(false);
        }
    
        
        foreach (char c in NowStatus.tnts) {
            float[] pos = MyUtiles.point2place(c.ToString(), center: true);
            
            GameObject tnt = Instantiate(myself.tntPrefab, new Vector3(pos[0], pos[1], pos[2]), Quaternion.identity);
        }
    }

    public static void render_coins() {
        Debug.Log("rendering coins");
        // firstly delete all objects tagged "coin"
        GameObject[] coins = GameObject.FindGameObjectsWithTag("coin");
        foreach (GameObject coin in coins) {
            // inactivate coin
            coin.SetActive(false);
        }
    
        
        foreach (char c in NowStatus.coins) {
            float[] pos = MyUtiles.point2place(c.ToString(), center: true);
            // Rotation x 270, y 0, z 0 
            GameObject coin = Instantiate(myself.coinPrefab, new Vector3(pos[0], pos[1], pos[2]), Quaternion.Euler(270, 0, 0));
        }
    }

    public static void render_finalBox() {
        Debug.Log("rendering finalBox");
        // firstly delete all objects tagged "final_box"
        GameObject[] finalBoxes = GameObject.FindGameObjectsWithTag("final_box");
        foreach (GameObject finalBox in finalBoxes) {
            // inactivate finalBox
            finalBox.SetActive(false);
        }
    
        
        foreach (char c in NowStatus.finalbox) {
            float[] pos = MyUtiles.point2place(c.ToString(), center: true);
            // Rotation x 270, y 90, z 0
            GameObject finalBox = Instantiate(myself.finalBoxPrefab, new Vector3(pos[0], pos[1], pos[2]), Quaternion.Euler(270, 90, 0));
        }
    }

    public static void render_walls() {
        // firstly delete all objects tagged "wall"
        KabeControl.updateKabeBySeq(NowStatus.walls);
    }
}