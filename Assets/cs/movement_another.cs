using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class movement_another : MonoBehaviour
{

    public Vector3 head_p;
    public Rigidbody rigidbody;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();  // 获取刚体组件
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }

    // Update is called once per frame
    void Update()
    {
        head_p = TCPClient.another_head_position;
        // Debug.Log(head_p);
        Vector3 vw = new Vector3();
        vw[0] = head_p[0] * (float)10;
        // vw[1] = head_p[1] * (float)10;
        vw[1] = (float) -0.15;

        vw[2] = (2 - head_p[2]) * (float)10;//前后不一致 改一改
        rigidbody.MovePosition(vw);
        // Debug.Log("move to " + vw);
    }
}
