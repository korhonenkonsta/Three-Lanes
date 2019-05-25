using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    public int damage;
    public Player owner;

    AudioSource audioSource;
    public GameObject explosionEffect;

    void Start()
    {
        
    }

    public void ChangeHealth(int amount)
    {
        health += amount;

        if (health <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        if (GetComponent<Base>())
        {
            GetComponent<Base>().OnDeath();
        }
        else
        {
            if (GetComponent<Deathrattle>())
            {
                GetComponent<Deathrattle>().OnDeath();
            }
            owner.opponent.RemoveUnitFromTargetLists(transform);
        }
        GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
        ParticleSystem PS = effect.GetComponent<ParticleSystem>();
        Destroy(effect, PS.main.duration);

        Destroy(gameObject);
    }

    //void OnCollisionEnter(Collision col)
    //{
    //    if (col.gameObject.GetComponent<Health>())
    //    {
    //        //Units
    //        if (GetComponent<Unit>() && col.gameObject.GetComponent<Unit>())
    //        {
    //            if (GetComponent<Unit>().owner != col.gameObject.GetComponent<Unit>().owner)
    //            {
    //                col.gameObject.GetComponent<Health>().ChangeHealth(-damage);
    //                print("units from dif owners collided");
    //            }
    //        }
    //        else
    //        {
    //            print("non-unit health changed");
    //            col.gameObject.GetComponent<Health>().ChangeHealth(-damage);
    //        }
    //    }

    //    //audioSource.Play();
    //}

    void Update()
    {
        
    }
}
