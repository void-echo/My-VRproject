using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class final_box : MonoBehaviour
{
    public static int score = 100;
    public static bool game_over = false;

    void Update()
    {
        // if player is 3 meters away from the box, then game over.
        if (Vector3.Distance(movement.get_player_position(), transform.position) < 3)
        {
            game_over = true;
            UIManager.AddScore(score);
            UIManager.myself.GameOver();
        }
    }

    void Start()
    {

    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     // if coin is collected, then destroy coin.
    //     if (other.gameObject.tag == "player")
    //     {
    //         game_over = true;
    //         UIManager.myself.GameOver();
    //         // destroy tnt after 1 second.
    //         Destroy(gameObject);
    //     }
    // }
}
