using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathrattle : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int spawnCount = 1;

    void Start()
    {
        
    }

    public void OnDeath()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Spawn(prefabToSpawn, GetComponent<Unit>().owner, GetComponent<Unit>().currentLane);
        }
    }


    public void Spawn(GameObject prefab, Player owner, Lane currentLane)
    {
        if (prefab.gameObject.GetComponent<Unit>())
        {
            Unit tempUnit = Instantiate(prefab, transform.position, transform.rotation).GetComponent<Unit>();
            tempUnit.GetComponent<MoveForward>().startingRotation = GetComponent<MoveForward>().startingRotation;
            tempUnit.owner = owner;
            tempUnit.currentLane = currentLane;
            tempUnit.gameObject.GetComponent<Health>().owner = owner;
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

    void Update()
    {
        
    }
}
