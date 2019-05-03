using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{

    Rigidbody rb;
    public float factor; //0.2
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

    void FollowTargetWithRotation(Transform target, float speed)
    {
        if (target)
        {
            transform.LookAt(target);
            //rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Force);
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * factor);
        }
        else
        {
            transform.rotation = startingRotation;
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * factor);
        }
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        FollowTargetWithRotation(GetClosestEnemy(GetComponent<Unit>().owner.enemyUnits, transform), 10f);
    }
}
