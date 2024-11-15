using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject Player;
    public GameObject bulletPrefab;
    public GameObject bulletMuzzleFlash;
    public GameObject gunLimitedSpace;

    [Range(0f, 1000f)] public float bulletSpeed = 10f;
    [Range(0f, 10f)] public float gunCooldown = 0.5f;

    private bool hasCooldown = false;
    private float gunOffset = 0;

    void Update()
    {
        faceMouse();
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(cloneBullet());
        }
    }

    void faceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Get the PolygonCollider2D on gunLimitedSpace
        PolygonCollider2D polygonCollider = gunLimitedSpace.GetComponent<PolygonCollider2D>();

        if (polygonCollider != null)
        {
            // Find the closest point on the polygon's edge to the mouse position
            Vector2 closestPoint = polygonCollider.ClosestPoint(mousePosition);

            // Set the gun's position to this closest point on the edge
            transform.position = closestPoint;

            // Rotate the gun to face the mouse direction
            Vector2 direction = (mousePosition - transform.position).normalized;
            transform.up = direction;
        }
    }




    IEnumerator cloneBullet()
    {
        if (!hasCooldown)
        {
            //gameObject.GetComponent<AudioSource>().Play();
            hasCooldown = true;
            GameObject bulletClone = Instantiate(bulletPrefab, transform.position, transform.rotation);
            //StartCoroutine(toggleMuzzleFlash());

            Rigidbody2D rb = bulletClone.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.freezeRotation = false;
                rb.constraints = RigidbodyConstraints2D.None;
                rb.velocity = transform.up * bulletSpeed;
                yield return new WaitForSeconds(gunCooldown);
                hasCooldown = false;
                Destroy(bulletClone, 5f);
            }
        }
    }
    IEnumerator toggleMuzzleFlash()
    {
        
        SpriteRenderer muzzleFlashRenderer = bulletMuzzleFlash.GetComponent<SpriteRenderer>();
        SpriteRenderer muzzleFlashChildRenderer = bulletMuzzleFlash.transform.Find("OuterCircle").GetComponent<SpriteRenderer>();
        muzzleFlashChildRenderer.enabled = true;
        muzzleFlashRenderer.enabled = true;
        yield return new WaitForSeconds(0.2f);
        muzzleFlashChildRenderer.enabled = false;
        muzzleFlashRenderer.enabled = false;
    }
}
