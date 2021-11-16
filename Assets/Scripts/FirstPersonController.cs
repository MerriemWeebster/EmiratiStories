using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool up = Input.GetKey(KeyCode.E);
        bool down = Input.GetKey(KeyCode.Q);

        Vector3 movement = transform.right * x + transform.forward * z;

        if(up)
        {
            movement.y = 0.5f;
        }
        else if(down)
        {
            movement.y = -0.5f;
        }

        transform.position += movement * speed * Time.deltaTime;
    }
}
