using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    public Transform respawnPoint;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Respawn();
        }
    }

    void Respawn()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            Debug.Log("Player respawned at: " + respawnPoint.position);
        }
        else
        {
            Debug.LogWarning("Respawn point not set!");
        }
    }
}
