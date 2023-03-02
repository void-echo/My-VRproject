using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyer : MonoBehaviour
{
    public float lifeTime = 10f;

    // Update is called once per frame
    void Update()
    {
        if (lifeTime > 0) {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0) {
                Destroy(this.gameObject);
            }
        } 

        if (this.transform.position.y <= -20) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "destroyer") {
            Destroy(this.gameObject);
        }
    }
}
