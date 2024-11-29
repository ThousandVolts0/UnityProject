using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovementWallBounce : MonoBehaviour
{
    public float speed = 2f;               // Fiendens hastighet
    public float detectionDistance = 0.15f;   // Avstånd för väggdetektion
    public LayerMask wallLayer;           // Endast väggar ska påverka fiendens rörelse
    private bool movingRight = true;      // Om fienden rör sig åt höger
    Vector3 rayOrigin;
    Vector3 rayDirection;

    void Update()
    {
        // Flytta fienden lokalt
        transform.Translate(Vector2.right * speed * Time.deltaTime, Space.Self);

        // Lägg till marginal för Raycast startpunkt
        rayOrigin = transform.position + (movingRight ? Vector3.right : Vector3.left) * 0.1f; // Förskjut Raycast framåt
        rayDirection = movingRight ? Vector2.right : Vector2.left;

        // Skicka Raycast för att upptäcka väggar
        RaycastHit2D wallInfo = Physics2D.Raycast(rayOrigin, rayDirection, detectionDistance, wallLayer);

        // Om en vägg upptäcks, vänd fiendens riktning
        if (wallInfo.collider)
        {
            movingRight = !movingRight;
        }

        // Vänd fiendens riktning
        if (movingRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // Riktning åt höger
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0); // Riktning åt vänster
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + rayDirection * detectionDistance);
    }
}