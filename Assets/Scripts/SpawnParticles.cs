using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.Universal;

public class SpawnParticles : MonoBehaviour
{
    // Start is called before the first frame update
    public void spawnParticles(Transform pos, GameObject emitterObj, Transform otherCollider)
    {
        GameObject emitterClone = Instantiate(emitterObj, pos.position, Quaternion.identity);
        emitterClone.transform.SetParent(otherCollider);
        emitterClone.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        ParticleSystem particleSystem = emitterClone.GetComponent<ParticleSystem>();
        if (particleSystem )
        {
            particleSystem.Play();
        }



    }

}
