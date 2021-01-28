﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 inputs;
    private readonly float speed = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        inputs.x = Input.GetAxisRaw("Horizontal");
        inputs.y = Input.GetAxisRaw("Vertical");
        inputs.Normalize();

        /*if (inputs == Vector2.zero && speed > startSpeed)
            speed -= deceleration * Time.deltaTime;
        else if (speed < maxSpeed)
            speed += acceleration * Time.deltaTime;*/

        rb.velocity = inputs * speed;
    }
}
