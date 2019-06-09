﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject explosionEffect;
    public float radius = 1f;
    public int damage = 1;

    void Start()
    {
        
    }

    public void Explode()
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

                //Currently friendly fire is on even for bases!
                if (nearbyObject.GetComponent<Health>())
                {
                    nearbyObject.GetComponent<Health>().ChangeHealth(-damage);
                }
            }
            
        }

    }

    void Update()
    {
        
    }
}
