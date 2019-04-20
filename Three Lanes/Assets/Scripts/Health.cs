using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    public int damage;

    AudioSource audioSource;

    void Start()
    {
        
    }

    public void ChangeHealth(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        if (GetComponent<Base>()!= null)
        {
            GetComponent<Base>().OnDeath();
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Health>())
        {
            col.gameObject.GetComponent<Health>().ChangeHealth(damage);
        }

        //audioSource.Play();
    }

    void Update()
    {
        
    }
}
