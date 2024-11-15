using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject Player;
    public GameObject bulletPrefab;
    public GameObject bulletMuzzleFlash;
    public GameObject BoundsLeft;
    public GameObject BoundsRight;
    public GameObject BoundsTop;

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

        Vector2[] Left = BoundsLeft.GetComponent<PolygonCollider2D>().points;
        Vector2[] Right = BoundsRight.GetComponent<PolygonCollider2D>().points;
        Vector2[] Top = BoundsTop.GetComponent<PolygonCollider2D>().points;



        Vector2 closestPointLeft = Left.ClosestPoint(mousePosition);


        Vector2 closestPointRight = Right.ClosestPoint(mousePosition);
        Vector2 closestPointTop = Top.ClosestPoint(mousePosition);

        float distancePointLeft  = Vector2.Distance(mousePosition, closestPointLeft);
        float distancePointRight = Vector2.Distance(mousePosition, closestPointRight);
        float distancePointTop = Vector2.Distance(mousePosition, closestPointTop);

        float minDistance = Mathf.Min(distancePointLeft, distancePointRight, distancePointTop);
        Vector2 direction = (mousePosition - transform.position).normalized;

        if (minDistance == distancePointLeft)
        {
            mousePosition = closestPointLeft;
        }
        else if (minDistance == distancePointRight)
        {
            mousePosition = closestPointRight;
        }
        else if (minDistance == distancePointTop)
        {
            mousePosition = closestPointTop;
        }

        transform.position = mousePosition;
        transform.up = direction;
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
