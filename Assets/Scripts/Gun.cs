using System.Collections;
using UnityEngine;

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
    private Vector3 mousePosition;
    private Vector2 direction = new Vector2(0, 0);
    private Vector3 targetPos = new Vector3(0, 0, 0);
    private Rigidbody2D rigidbody;

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
        rigidbody = Player.GetComponent<Rigidbody2D>();
    }

    void faceMouse()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        float distanceLeft = Vector2.Distance(mousePosition, BoundsLeft.transform.position);
        float distanceRight = Vector2.Distance(mousePosition, BoundsRight.transform.position);

        if (distanceLeft < distanceRight)
        {
            targetPos = BoundsLeft.transform.position;
        }
        else
        {
            targetPos = BoundsRight.transform.position;
        }

        transform.position = Vector2.Lerp(transform.position, targetPos, 0.25f);
        direction = (mousePosition - transform.position).normalized;
        transform.up = Vector2.Lerp(transform.up, direction, 0.6f);
    }

    IEnumerator cloneBullet()
    {
        if (!hasCooldown)
        {
            hasCooldown = true;
            Vector3 spawnpos = transform.position;
            GameObject bulletClone = Instantiate(bulletPrefab, spawnpos, transform.rotation);
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
            else
            {
                hasCooldown = false;
                yield return null;
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
