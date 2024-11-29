using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovementWallBounce : MonoBehaviour
{
    public float speed = 2f;               // Fiendens hastighet
    public float detectionDistance = 0.15f;   // Avst�nd f�r v�ggdetektion
    public LayerMask wallLayer;           // Endast v�ggar ska p�verka fiendens r�relse
    private bool movingRight = true;      // Om fienden r�r sig �t h�ger
    Vector3 rayOrigin;
    Vector3 rayDirection;

    void Update()
    {
        // Flytta fienden lokalt
        transform.Translate(Vector2.right * speed * Time.deltaTime, Space.Self);

        // L�gg till marginal f�r Raycast startpunkt
        rayOrigin = transform.position + (movingRight ? Vector3.right : Vector3.left) * 0.1f; // F�rskjut Raycast fram�t
        rayDirection = movingRight ? Vector2.right : Vector2.left;

        // Skicka Raycast f�r att uppt�cka v�ggar
        RaycastHit2D wallInfo = Physics2D.Raycast(rayOrigin, rayDirection, detectionDistance, wallLayer);

        // Om en v�gg uppt�cks, v�nd fiendens riktning
        if (wallInfo.collider)
        {
            movingRight = !movingRight;
        }

        // V�nd fiendens riktning
        if (movingRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // Riktning �t h�ger
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0); // Riktning �t v�nster
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + rayDirection * detectionDistance);
    }
}