using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsExplosion : MonoBehaviour
{
    public float minForce = 50f;
    public float maxForce = 300f;
    public float radius = 0.1f;
    public float destroyDelay = 5f;

    public void Explode()
    {
        foreach (Transform t in transform)
        {
            Rigidbody rb = t.GetComponent<Rigidbody>();

            if (rb)
            {
                rb.AddExplosionForce(Random.Range(minForce, maxForce), transform.position - Vector3.forward * 0.01f, radius);
            }

            Destroy(t.gameObject, destroyDelay);
        }
    }

    void Start()
    {
        Explode();
    }

    void Update()
    {
        
    }
}
