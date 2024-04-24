using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f; // You can adjust the speed to your liking
    private Rigidbody2D rb;
    private void PlayerMove() {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(moveX * speed, moveY * speed);
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        PlayerMove();
    }
}
