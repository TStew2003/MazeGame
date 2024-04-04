using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gridSize;
    public Animator animator;
    private Vector2 targetPosition;
    private bool isHiding;

    public Canvas gameOverCanvas;


    void Start()
    {
        gameOverCanvas.gameObject.SetActive(false);
        targetPosition = transform.position;
        isHiding = false;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("isHiding", true);
            isHiding = true;
        }

        else if (transform.position == new Vector3(targetPosition.x, targetPosition.y, transform.position.z))
        {
            animator.SetBool("isHiding", false);
            isHiding = false;

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            if (horizontal != 0 || vertical != 0)
            {
                if (horizontal > 0)
                {
                    transform.eulerAngles = new Vector3(0f, 180f, 0f);
                }
                else if (horizontal < 0)
                {
                    transform.eulerAngles = new Vector3(0f, 0f, 0f);
                }

                Vector2 nextPosition = transform.position + new Vector3(horizontal * gridSize, vertical * gridSize, 0);
                if (ValidNextCell(nextPosition))
                {
                    targetPosition = nextPosition;
                }
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private bool ValidNextCell(Vector2 position)
    {
        Collider2D collider = Physics2D.OverlapCircle(position, 0.1f);
        return collider == null;
    }

    public bool IsHiding()
    {
        return isHiding;
    }

    public void Reset()
    {
        Vector3 pos = new Vector3(0.5f, 8f, 0f);
        this.transform.position = pos;
        this.gameObject.SetActive(true);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            this.gameObject.SetActive(false);

            gameOverCanvas.gameObject.SetActive(true);

            //play explosion and/or sound
        }
    }
}
