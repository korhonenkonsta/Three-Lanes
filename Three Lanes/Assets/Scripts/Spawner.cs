using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public bool doSpawn = true;
    public float spawnInterval = 1;
    public Player owner;
    public Lane currentLane;

    void Start()
    {
        StartCoroutine(ContinuousSpawn());
    }

    IEnumerator ContinuousSpawn()
    {
        while (doSpawn)
        {
            Spawn(prefabToSpawn);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void Spawn(GameObject prefab)
    {
        if (prefab.gameObject.GetComponent<Unit>())
        {
            Unit tempUnit = Instantiate(prefab, transform.position + transform.forward * 0.2f, transform.rotation).GetComponent<Unit>();

            if (!owner && GetComponent<Unit>())
            {
                owner = GetComponent<Unit>().owner;
                currentLane = GetComponent<Unit>().currentLane;
            }

            tempUnit.owner = owner;
            tempUnit.currentLane = currentLane;
            tempUnit.GetComponent<Health>().owner = owner;

            if (tempUnit.GetComponent<Shooter>())
            {
                tempUnit.GetComponent<Shooter>().u = tempUnit;
            }

            owner.opponent.enemyUnitsAll.Add(tempUnit.transform);

            if (currentLane.laneNumber == 1)
            {
                owner.opponent.enemyUnits1.Add(tempUnit.transform);
            }
            else if (currentLane.laneNumber == 2)
            {
                owner.opponent.enemyUnits2.Add(tempUnit.transform);
            }
            else
            {
                owner.opponent.enemyUnits3.Add(tempUnit.transform);
            }

        }
        else
        {
            print("other than unit spawned");
            Instantiate(prefab, transform.position, transform.rotation);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
