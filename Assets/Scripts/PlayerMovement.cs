using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    [Range(0, 1000)] public float defaultWalkSpeed = 7f;
    [Range(0, 1000)] public float runSpeed = 9f;
    [Range(0, 1000)] public float jumpForce = 7.5f;

    public float maxStamina = 100f;
    public float stamina = 100f;
  
    private Rigidbody2D Rigidbody;
    private float activeWalkSpeed = 7f;
    private bool isGrounded = false;
    private bool isRunning = false;
    private bool isCrouching = false;

    public LayerMask groundLayer;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        stamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (stamina > 0 && !isCrouching)
            {
                isRunning = true;
                activeWalkSpeed = runSpeed;
                Debug.Log("Running");
            }
            
        }
        else
        {
            isRunning = false;
            activeWalkSpeed = defaultWalkSpeed;
        }

        if (Input.GetKey(KeyCode.C))
        {
            if (isGrounded)
            {
                isCrouching = true;
                Debug.Log("Crouching");
            }
            
        }
        else
        {
            isCrouching = false;
        }

        float moveInput = Input.GetAxis("Horizontal");
        Rigidbody.velocity = new Vector2(moveInput * activeWalkSpeed, Rigidbody.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded || Input.GetKeyDown(KeyCode.W) && isGrounded)
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
