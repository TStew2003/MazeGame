using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    private Vector2 moveAmount;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAmount = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        getMovement();
    }

    private void FixedUpdate()
    {
        move();
    }

    private void getMovement()
    {
        moveAmount.x = Input.GetAxisRaw("Horizontal");
        moveAmount.y = Input.GetAxisRaw("Vertical");
        moveAmount = moveAmount.normalized * speed;
    }

    private void move()
    {
        rb.MovePosition(rb.position + moveAmount * Time.deltaTime);
    }
}
