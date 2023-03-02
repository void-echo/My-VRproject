using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class movement : MonoBehaviour
{

    public Vector3 head_p, right_hand_p;
    public bool hand_over_head;
    public Rigidbody rigidbody;
    public static string pnt_char;
    public static movement myself;
    public static Vector3 vw; // player position
    public static Vector3 get_player_position()
    {
        return vw;
    }


    void Start()
    {
        myself = this;
        rigidbody = GetComponent<Rigidbody>();  // 获取刚体组件
        // freezeRotation on Y axis
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionY;
    }

    // Update is called once per frame
    void Update()
    {
        head_p = TCPClient.head_position;
        // Debug.Log(head_p);
        vw = new Vector3();
        vw[0] = head_p[0] * (float)10;
        // vw[1] = head_p[1] * (float)10;
        vw[1] = (float) -0.15;

        vw[2] = (2 - head_p[2]) * (float)10;//前后不一致 改一改
        rigidbody.MovePosition(vw);
        right_hand_p = TCPClient.right_hand_position;
        hand_over_head = right_hand_p[1] > head_p[1];
        // if hand over head, print.
        if (hand_over_head)
        {
            Debug.Log("hand over head");
        }
    }
}
