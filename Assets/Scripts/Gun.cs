using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Transactions;
using TMPro;
using Unity.Mathematics;
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

    [Range(0f, 1000f)] public float bulletSpeed = 10f;
    [Range(0f, 10f)] public float gunCooldown = 0.5f;

    private bool hasCooldown = false;
    private float gunOffset = 0;

    private float[] distanceLeftArray;
    private float[] distanceRightArray;
    private float[] distanceTopArray;
    private float lerpFactor;
    private float minDistance;
    private float minDistanceLeft;
    private float minDistanceRight;

    private Rigidbody2D rigidbody;
    private Vector3 mousePosition;

    private Vector2[] Right;
    private Vector2[] Left;
    private Vector2 closestPointLeft;
    private Vector2 closestPointRight;

    private Vector2 direction = new Vector2(0,0);
    private Vector3 targetPos = new Vector3(0,0,0);

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

        rigidbody = Player.GetComponent<Rigidbody2D>();
    }

    void faceMouse()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Left = BoundsLeft.GetComponent<PolygonCollider2D>().points;
        Right = BoundsRight.GetComponent<PolygonCollider2D>().points;

        for (int i = 0; i < Left.Length; i++)
        {
            distanceLeftArray[i] = Vector2.Distance(mousePosition, BoundsLeft.transform.TransformPoint(Left[i]));
        }

        for (int i = 0; i < Right.Length; i++)
        {
            distanceRightArray[i] = Vector2.Distance(mousePosition, BoundsRight.transform.TransformPoint(Right[i]));
        }

        minDistanceLeft = distanceLeftArray.Min();
        minDistanceRight = distanceRightArray.Min();

        closestPointLeft = Left[Array.IndexOf(distanceLeftArray, minDistanceLeft)];
        closestPointRight = Right[Array.IndexOf(distanceRightArray, minDistanceRight)];

        minDistance = Mathf.Min(minDistanceLeft, minDistanceRight);

        if (minDistance == minDistanceLeft)
        {
            //transform.position = BoundsLeft.transform.TransformPoint(closestPointLeft);
            targetPos = BoundsLeft.transform.TransformPoint(closestPointLeft);
        }
        else if (minDistance == minDistanceRight)
        {
            //transform.position = BoundsRight.transform.TransformPoint(closestPointRight);
            
            targetPos = BoundsRight.transform.TransformPoint(closestPointRight);
        }

        float velocity = Mathf.Round(rigidbody.velocity.magnitude);

        transform.position = Vector2.Lerp(transform.position, targetPos, 0.25f);
        direction = (mousePosition - transform.position).normalized;
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

    //private void OnDrawGizmos()
    //{
    //    Vector3 objectPosition = BoundsLeft.transform.position;
    //    Quaternion objectRotation = BoundsLeft.transform.rotation;
    //    Vector3 objectScale = BoundsLeft.transform.lossyScale;

    //    foreach (Vector2 point in BoundsLeft.GetComponent<PolygonCollider2D>().points)
    //    {
    //        // Convert from local space to world space considering rotation and scale
    //        Vector3 worldPoint = objectPosition + objectRotation * Vector3.Scale(point, objectScale);
    //        Gizmos.DrawSphere(worldPoint, 0.01f);
    //    }

    //    Vector3 objectPosition2 = BoundsRight.transform.position;
    //    Quaternion objectRotation2 = BoundsRight.transform.rotation;
    //    Vector3 objectScale2 = BoundsRight.transform.lossyScale;

    //    foreach (Vector2 point in BoundsRight.GetComponent<PolygonCollider2D>().points)
    //    {
    //        // Convert from local space to world space considering rotation and scale
    //        Vector3 worldPoint = objectPosition2 + objectRotation2 * Vector3.Scale(point, objectScale2);
    //        Gizmos.DrawSphere(worldPoint, 0.01f);
    //    }

    //}

}
