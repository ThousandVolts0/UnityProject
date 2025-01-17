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
    public GameObject player;
    public GameObject bulletPrefab;
    
    public GameObject muzzleFlash;
    public GameObject muzzleFlashAttachment;
    public CameraShake CameraShake;

    public GameObject boundsLeft;
    public GameObject boundsRight;

    public float recoilStrength = 3.5f;

    public float bulletSpeed = 10f;
    [SerializeField] public float gunCooldown = 0.5f;
    [SerializeField] public float muzzleFlashLifetime = 0.5f;

    private Vector3 direction;

    Vector3 mousePos;

    bool gunHasCooldown = false;

    private Vector3 vel = Vector3.zero;

    private void FixedUpdate()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        if (Vector3.Distance(mousePos, boundsLeft.transform.position) < (Vector3.Distance(mousePos, boundsRight.transform.position)))
        {
            gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, boundsLeft.transform.position, ref vel, 0.075f);
        }
        else
        {
            gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, boundsRight.transform.position, ref vel, 0.075f);
        }

        Vector3 rotation = transform.position - mousePos;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !gunHasCooldown)
        {
            Debug.Log("T");
            StartCoroutine(fireBullet());
            Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
            if (playerRigidbody)
            {
                if (Mathf.Abs(playerRigidbody.velocity.x) > 0)
                {
                    StartCoroutine(CameraShake.Shake(Camera.main.transform, 0.04f, 0.04f));
                }
                else
                {
                    StartCoroutine(CameraShake.Shake(Camera.main.transform, 0.1f, 0.07f));
                }
            }
            else
            {
                StartCoroutine(CameraShake.Shake(Camera.main.transform, 0.1f, 0.07f));
            }
      

        }
    }
    private IEnumerator fireBullet()
    {
        gunHasCooldown = true;

        StartCoroutine(doRecoil());

        GameObject activeBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        GameObject activeMuzzleFlash = Instantiate(muzzleFlash, muzzleFlashAttachment.transform.position, activeBullet.transform.rotation, gameObject.transform);
        activeMuzzleFlash.transform.localScale = new Vector2(2.5f, 1.25f);

        Destroy(activeMuzzleFlash, muzzleFlashLifetime);

        Rigidbody2D bulletRb = activeBullet.GetComponent<Rigidbody2D>();

        Vector3 bulletdirection = mousePos - activeBullet.transform.position;
        Vector3 rotation = activeBullet.transform.position - mousePos;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        activeBullet.transform.rotation = Quaternion.Euler(0, 0, rot + 90);

        Debug.Log("BulletDirection: " + bulletdirection.ToString());

        if (bulletRb != null)
        {
            bulletRb.velocity = new Vector2(bulletdirection.x, bulletdirection.y).normalized * bulletSpeed;
        }
        else
        {   
            Debug.Log("Bullet rigidbody not found.");
            Destroy(activeBullet);
            yield return null;
        }

        yield return new WaitForSeconds(gunCooldown);

        gunHasCooldown = false;
    }

    private IEnumerator doRecoil()
    {
        transform.Translate(-Vector2.up * Time.deltaTime * recoilStrength);
        yield return null;
    }

}
