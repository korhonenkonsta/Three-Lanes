using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Unit u;
    public float range = 1f;
    public bool isReloading;
    public float reloadTime = 0.5f;
    Transform target;
    public GameObject prefabToSpawn;

    void Start()
    {
        
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }

    public float GetDistanceToTransform(Transform other)
    {
        float dist = -1f;
        if (other)
        {
            dist = Vector3.Distance(other.position, transform.position);
        }
        
        return dist;
    }

    public void Shoot()
    {
        Unit tempUnit = Instantiate(prefabToSpawn, transform.position + transform.forward * 0.2f, transform.rotation).GetComponent<Unit>();
        tempUnit.owner = u.owner;
        tempUnit.currentLane = u.currentLane;
        tempUnit.GetComponent<Health>().owner = u.owner;

        StartCoroutine("Reload");
    }

    void Update()
    {
        if (u.multiLaneTargetSearch)
        {
            target = u.GetClosestEnemy(u.owner.enemyUnitsAll, transform);
        }
        else
        {
            if (u.currentLane.laneNumber == 1)
            {
                target = u.GetClosestEnemy(u.owner.enemyUnits1, transform);
            }
            else if (u.currentLane.laneNumber == 2)
            {
                target = u.GetClosestEnemy(u.owner.enemyUnits2, transform);
            }
            else
            {
                target = u.GetClosestEnemy(u.owner.enemyUnits3, transform);
            }
        }

        if (GetDistanceToTransform(target) >= 0 && GetDistanceToTransform(target) <= range && !isReloading)
        {
            Shoot();
        }
    }
}
