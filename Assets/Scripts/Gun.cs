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

    public GameObject boundsLeft;
    public GameObject boundsRight;

    [SerializeField] public float bulletSpeed = 10f;
    [SerializeField] public float gunCooldown = 0.5f;

    private Vector2 mousePos;
    private Vector2 targetPos;
    private Vector3 direction;

    bool gunHasCooldown = false;

    private Vector3 vel = Vector3.zero;

    private void FixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);

        if (Vector3.Distance(targetPos, boundsLeft.transform.position) < (Vector3.Distance(targetPos,boundsRight.transform.position)))
        {
            gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, boundsLeft.transform.position, ref vel, 0.075f);
        }
        else
        {
            gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, boundsRight.transform.position, ref vel, 0.075f);
        }

        direction = (targetPos - transform.position).normalized;

        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !gunHasCooldown)
        {
            Debug.Log("T");
            StartCoroutine(fireBullet());
        }
    }
    private IEnumerator fireBullet()
    {
        gunHasCooldown = true;

        GameObject activeBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Rigidbody2D bulletRb = activeBullet.GetComponent<Rigidbody2D>();

        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        activeBullet.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        if (bulletRb != null)
        {
            bulletRb.velocity = direction.normalized * bulletSpeed;
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

}
