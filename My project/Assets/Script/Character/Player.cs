using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 5f;
    private float jumpP = 300f;

    Rigidbody2D rbody;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rbody.velocity.y) < 0.01f)
        {
            rbody.AddForce(transform.up * jumpP);
        }
    }

    void FixedUpdate()
    {
        float move = 0f;

        if (Input.GetKey(KeyCode.D))
        {
            move = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            move = -1f;
        }

        rbody.velocity = new Vector2(move * speed, rbody.velocity.y);
    }
}
