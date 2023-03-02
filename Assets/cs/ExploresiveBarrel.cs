using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploresiveBarrel : MonoBehaviour
{
    public GameObject Explosion;
    public static int score = -20;

    public bool isBooming = false;
    // set Barrel to active 
    void Start()
    {
        // Barrel.SetActive(true);
        Explosion.SetActive(false);
    }

    void Update()
    {
        // if player is 3 meters away from tnt, then boom.
        if (Vector3.Distance(movement.get_player_position(), transform.position) < 3)
        {
            Debug.Log("booming!!!!!!!!!!!!!!!!!!!!");
            isBooming = true;
            // destroy tnt after 1 second.
            Explosion.SetActive(true);
            UIManager.AddScore(score);
            Destroy(gameObject, 1);
        }
    }
}