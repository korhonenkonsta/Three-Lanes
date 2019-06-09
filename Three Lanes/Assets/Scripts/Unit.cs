using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Player owner;
    public Health health;
    public int damage;
    public Lane currentLane;

    public bool multiLaneTargetSearch;
    public bool isBullet;

    void Start()
    {
        
    }

    public Transform GetClosestEnemy(List<Transform> enemies, Transform fromThis)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = fromThis.position;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
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
                    if (GetComponent<Explosion>())
                    {
                        GetComponent<Health>().OnDeath();
                    }
                    else
                    {
                        col.gameObject.GetComponent<Health>().ChangeHealth(-damage);
                    }
                    //print("units from dif owners collided");
                }
            }
            else
            {
                if (col.gameObject.GetComponent<Base>())
                {
                    if (owner && owner != col.gameObject.GetComponent<Base>().owner)
                    {
                        
                        col.gameObject.GetComponent<Health>().ChangeHealth(-damage);
                        //print("units from dif owners collided");

                        //Issue with exploding creeps, do dmg twice to base
                        GetComponent<Health>().OnDeath();
                    }
                    else
                    {
                        //print("no owner, or same owner");
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
            //print("no health");
        }

        //audioSource.Play();
    }

    void Update()
    {
        if (!GetComponent<Shooter>())
        {
            damage = health.hp;
        }
        
    }
}
