using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    [Range (0, 1000)] public float speed = 10f;
    [Range (0, 1000)] public float jumpForce = 7.5f;
    private Rigidbody2D Rigidbody;
    private bool isGrounded;

    public LayerMask groundLayer;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Rigidbody.velocity = new Vector2(moveInput * speed, Rigidbody.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded || Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.35f, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 0.35f);
    }

}
