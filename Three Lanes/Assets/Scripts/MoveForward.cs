using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{

    Rigidbody rb;
    public float speedFactor; //0.2
    Quaternion startingRotation;

    //1. Set lane
    //2. Search for closest enemy (other owner) in lane
    //3. Move towards it
    //4. Otherwise, move forward

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startingRotation = transform.rotation;
    }

    Transform GetClosestEnemy(List<Transform> enemies, Transform fromThis)
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
            transform.rotation = startingRotation;
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * speedFactor);
        }
    }

    void Update()
    {
        //print(startingRotation.eulerAngles);
    }

    void FixedUpdate()
    {
        Unit u = GetComponent<Unit>();
        if (u.multiLaneTargetSearch)
        {
            FollowTargetWithRotation(GetClosestEnemy(u.owner.enemyUnitsAll, transform));
        }
        else
        {
            if (u.currentLane.laneNumber == 1)
            {
                FollowTargetWithRotation(GetClosestEnemy(u.owner.enemyUnits1, transform));
            }
            else if (u.currentLane.laneNumber == 2)
            {
                FollowTargetWithRotation(GetClosestEnemy(u.owner.enemyUnits2, transform));
            }
            else
            {
                FollowTargetWithRotation(GetClosestEnemy(u.owner.enemyUnits3, transform));
            }
        }
    }
}
