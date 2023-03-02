using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tnt : MonoBehaviour
{
    public static int score = -20;

    public bool isBooming = false;
    void Update()
    {
        // if player is 2 meters away from tnt, then boom.
        if (Vector3.Distance(movement.get_player_position(), transform.position) < 3)
        {
            Debug.Log("booming!!!!!!!!!!!!!!!!!!!!");
            isBooming = true;
            // destroy tnt after 1 second.
            Destroy(gameObject, 1);
            UIManager.AddScore(score);
        }
    }

    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // if coin is collected, then destroy coin.
        if (other.gameObject.tag == "player")
        {
            this.isBooming = true;
            // destroy tnt after 1 second.
            Destroy(gameObject, 1);
        }
    }
}
