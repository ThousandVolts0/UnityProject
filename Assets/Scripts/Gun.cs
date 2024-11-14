using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject Player;
    public GameObject bulletPrefab;
    public GameObject bulletMuzzleFlash;

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

        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        if (transform.eulerAngles.z < 360 && transform.eulerAngles.z > 180)
        {
            gunOffset = 0.25f;
        }
        else
        {
            gunOffset = -0.25f;
        }

        transform.up = direction;
        transform.position = new Vector3(Player.transform.position.x + gunOffset, Player.transform.position.y, Player.transform.position.z);

            
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
