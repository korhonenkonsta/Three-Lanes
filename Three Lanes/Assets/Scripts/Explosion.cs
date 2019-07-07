using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject explosionEffect;
    public float radius = 1f;
    public int damage = 1;

    public float fuseTime = 1f;

    void Start()
    {
        
    }

    public IEnumerator LightFuse(Player owner)
    {
        yield return new WaitForSeconds(fuseTime);
        Explode(owner);
        Destroy(gameObject);
    }

    public void Explode(Player owner)
    {
        GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
        ParticleSystem PS = effect.GetComponent<ParticleSystem>();
        Destroy(effect, PS.main.duration);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.gameObject != this)
            {
                //Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

                if (nearbyObject.GetComponent<Unit>())
                {
                    if (nearbyObject.GetComponent<Unit>().owner != owner)
                    {
                        print(nearbyObject.GetComponent<Health>().hp);
                        nearbyObject.GetComponent<Health>().ChangeHealth(-damage);
                        print(damage);
                    }
                }
            }
            
        }

    }

    void Update()
    {
        
    }
}
