using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMove : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    private bool isFrozen = false;

    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + 5);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            isFrozen = !isFrozen;

            // Toggle gravity scale based on isFrozen
            rb.gravityScale = isFrozen ? 0 : 1;

            // Optionally, reset velocity if you want the object to stay perfectly still
            if (isFrozen)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
}
