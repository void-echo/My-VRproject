using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    public static int score = 10;
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(1, 1, 1));
        if (movement.myself.hand_over_head) {
            if (Vector3.Distance(movement.get_player_position(), transform.position) < 5) {
                UIManager.AddScore(score);
                Destroy(gameObject);
                UIManager.myself.GameOver();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
