using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public SpawnParticles spawnParticles;
    public GameObject sparkEmitter;

    private void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        spawnParticles.spawnParticles(collision.otherCollider.transform, sparkEmitter, collision.gameObject.transform);
        Destroy(collision.otherCollider.gameObject);
    }

}
