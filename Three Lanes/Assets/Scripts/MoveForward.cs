using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{

    Rigidbody rb;
    public float speedFactor; //0.2
    public Quaternion startingRotation;
    Unit u;

    //1. Set lane
    //2. Search for closest enemy (other owner) in lane
    //3. Move towards it
    //4. Otherwise, move forward

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //startingRotation = transform.rotation;
    }

    

    void FollowTargetWithRotation(Transform target)
    {
        if (target)
        {
            transform.LookAt(target);
            //rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Force);
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * speedFactor);
        }
        else
        {
            //transform.rotation = startingRotation;
            rb.rotation = startingRotation.normalized;
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * speedFactor);
        }
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (!u)
        {
            u = GetComponent<Unit>();
        }
        
        if (u.multiLaneTargetSearch)
        {
            FollowTargetWithRotation(u.GetClosestEnemy(u.owner.enemyUnitsAll, transform));
        }
        else
        {
            if (u.currentLane.laneNumber == 1)
            {
                FollowTargetWithRotation(u.GetClosestEnemy(u.owner.enemyUnits1, transform));
            }
            else if (u.currentLane.laneNumber == 2)
            {
                FollowTargetWithRotation(u.GetClosestEnemy(u.owner.enemyUnits2, transform));
            }
            else
            {
                FollowTargetWithRotation(u.GetClosestEnemy(u.owner.enemyUnits3, transform));
            }
        }

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
