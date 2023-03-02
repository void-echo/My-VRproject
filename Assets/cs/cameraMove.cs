using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if press wasd, move camera



        // wasd keys to move camera until collision
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(Vector3.forward * Time.deltaTime * 10);
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(Vector3.back * Time.deltaTime * 10);
            
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector3.left * Time.deltaTime * 10);
            
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector3.right * Time.deltaTime * 10);
            
        }

        // if press space, fly camera.
        if (Input.GetKey(KeyCode.Space)) {
            transform.Translate(Vector3.up * Time.deltaTime * 10);
        }

        // if press L, fly down camera.
        if (Input.GetKey(KeyCode.L)) {
            transform.Translate(Vector3.down * Time.deltaTime * 10);
        }

        // if mouse move, rotate camera
        if (Input.GetAxis("Mouse X") != 0) {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * 3);
        }
        // if (Input.GetAxis("Mouse Y") != 0) {
        //     transform.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * 10);
        // }
        // if (Input.GetAxis("Mouse ScrollWheel") != 0) {
        //     transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * 10);
        // }

        // if it is on Mobile, use touch to move camera
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved) {
                transform.Rotate(Vector3.up * touch.deltaPosition.x * 0.1f);
                // transform.Rotate(Vector3.left * touch.deltaPosition.y * 0.1f);
            }
        }
    }
}
