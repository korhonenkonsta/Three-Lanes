using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Player owner;
    public Health health;
    public int damage;

    void Start()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Health>())
        {
            //Units
            if (col.gameObject.GetComponent<Unit>())
            {
                if (owner != col.gameObject.GetComponent<Unit>().owner)
                {
                    col.gameObject.GetComponent<Health>().ChangeHealth(-damage);
                    print("units from dif owners collided");
                }
            }
            else
            {
                if (col.gameObject.GetComponent<Base>())
                {
                    if (owner && owner != col.gameObject.GetComponent<Base>().owner)
                    {
                        print("owner:" + owner);
                        print("owner other:" + col.gameObject.GetComponent<Base>().owner);
                        col.gameObject.GetComponent<Health>().ChangeHealth(-damage);
                        //print("units from dif owners collided");
                        Destroy(gameObject);
                    }
                    else
                    {
                        print("no owner, or same owner");
                    }
                }
                else
                {
                    print("non-unit or base health changed");
                    col.gameObject.GetComponent<Health>().ChangeHealth(-damage);
                }
            }
        }
        else
        {
            print("no health");
        }

        //audioSource.Play();
    }

    void Update()
    {
        
    }
}
