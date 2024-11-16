using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.FilePathAttribute;

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

    private float[] distanceLeftArray;
    private float[] distanceRightArray;
    private float[] distanceTopArray;

    void Update()
    {
        faceMouse();
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(cloneBullet());
        }
    }

    private void Start()
    {
        distanceLeftArray = new float[BoundsLeft.GetComponent<PolygonCollider2D>().points.Length];
        distanceRightArray = new float[BoundsRight.GetComponent<PolygonCollider2D>().points.Length];
        distanceTopArray = new float[BoundsTop.GetComponent<PolygonCollider2D>().points.Length];
    }

    void faceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2[] Left = BoundsLeft.GetComponent<PolygonCollider2D>().points;
        Vector2[] Right = BoundsRight.GetComponent<PolygonCollider2D>().points;
        Vector2[] Top = BoundsTop.GetComponent<PolygonCollider2D>().points;

        for (int i = 0; i < Left.Length; i++)
        {
            distanceLeftArray[i] = Vector2.Distance(mousePosition, BoundsLeft.transform.TransformPoint(Left[i]));
        }

        for (int i = 0; i < Right.Length; i++)
        {
            distanceRightArray[i] = Vector2.Distance(mousePosition, BoundsRight.transform.TransformPoint(Right[i]));
        }

        for (int i = 0; i < Top.Length; i++)
        {
            distanceTopArray[i] = Vector2.Distance(mousePosition, BoundsTop.transform.TransformPoint(Top[i]));
        }

        Vector2 closestPointLeft = Left[Array.IndexOf(distanceLeftArray, distanceLeftArray.Min())];
        Vector2 closestPointRight = Right[Array.IndexOf(distanceRightArray, distanceRightArray.Min())];
        Vector2 closestPointTop = Top[Array.IndexOf(distanceTopArray, distanceTopArray.Min())];

        float minDistance = Mathf.Min(distanceLeftArray.Min(), distanceRightArray.Min(), distanceTopArray.Min());

        if (minDistance == distanceLeftArray.Min())
        {
            //transform.position = BoundsLeft.transform.TransformPoint(closestPointLeft);
            transform.position = Vector2.Lerp(transform.position, BoundsLeft.transform.TransformPoint(closestPointLeft), 0.3f);
        }
        else if (minDistance == distanceRightArray.Min())
        {
            //transform.position = BoundsRight.transform.TransformPoint(closestPointRight);
            transform.position = Vector2.Lerp(transform.position, BoundsRight.transform.TransformPoint(closestPointRight), 0.3f);
        }
        else if (minDistance == distanceTopArray.Min())
        {
            //transform.position = BoundsTop.transform.TransformPoint(closestPointTop);
            transform.position = Vector2.Lerp(transform.position, BoundsTop.transform.TransformPoint(closestPointTop), 0.3f);
        }
        
       
        Vector2 direction = (mousePosition - transform.position).normalized;
        
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

    private void OnDrawGizmos()
    {
        Vector3 objectPosition = BoundsLeft.transform.position;
        Quaternion objectRotation = BoundsLeft.transform.rotation;
        Vector3 objectScale = BoundsLeft.transform.lossyScale;

        foreach (Vector2 point in BoundsLeft.GetComponent<PolygonCollider2D>().points)
        {
            // Convert from local space to world space considering rotation and scale
            Vector3 worldPoint = objectPosition + objectRotation * Vector3.Scale(point, objectScale);
            Gizmos.DrawSphere(worldPoint, 0.01f);
        }

        Vector3 objectPosition2 = BoundsRight.transform.position;
        Quaternion objectRotation2 = BoundsRight.transform.rotation;
        Vector3 objectScale2 = BoundsRight.transform.lossyScale;

        foreach (Vector2 point in BoundsRight.GetComponent<PolygonCollider2D>().points)
        {
            // Convert from local space to world space considering rotation and scale
            Vector3 worldPoint = objectPosition2 + objectRotation2 * Vector3.Scale(point, objectScale2);
            Gizmos.DrawSphere(worldPoint, 0.01f);
        }

        Vector3 objectPosition3 = BoundsTop.transform.position;
        Quaternion objectRotation3 = BoundsTop.transform.rotation;
        Vector3 objectScale3 = BoundsTop.transform.lossyScale;

        foreach (Vector2 point in BoundsTop.GetComponent<PolygonCollider2D>().points)
        {
            // Convert from local space to world space considering rotation and scale
            Vector3 worldPoint = objectPosition3 + objectRotation3 * Vector3.Scale(point, objectScale3);
            Gizmos.DrawSphere(worldPoint, 0.01f);
        }
    }

}
