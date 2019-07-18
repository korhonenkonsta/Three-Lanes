﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public bool doSpawn = true;
    public float spawnInterval = 5f;
    public float spawnIntervalStep = 1f;
    public float spawnIntervalMin = 1f;
    public bool hasWindup;
    public Player owner;
    public Lane currentLane;

    void Start()
    {
        if (GetComponent<Building>() || GetComponent<Unit>())
        {
            StartCoroutine(ContinuousSpawn());
        }
    }

    IEnumerator ContinuousSpawn()
    {
        while (doSpawn)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (hasWindup && spawnInterval > spawnIntervalMin)
            {
                spawnInterval -= spawnIntervalStep;
            }

            Spawn(prefabToSpawn, transform.position + transform.forward * 0.4f, transform.rotation, owner, currentLane);
        }
    }

    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot, Player ownerPlayer, Lane lane)
    {
        if (prefab.gameObject.GetComponent<Unit>())
        {
            Unit tempUnit = Instantiate(prefab, pos, rot).GetComponent<Unit>();

            //if (!owner && GetComponent<Unit>())
            //{
            //    owner = GetComponent<Unit>().owner;
            //    currentLane = GetComponent<Unit>().currentLane;
            //}
            //else
            //{
                owner = ownerPlayer;
                currentLane = lane;
            //}
            
            tempUnit.owner = ownerPlayer;
            tempUnit.currentLane = lane;

            if (tempUnit.GetComponent<Spawner>())
            {
                tempUnit.GetComponent<Spawner>().owner = owner;
                tempUnit.GetComponent<Spawner>().currentLane = currentLane;
            }

            tempUnit.GetComponent<Health>().owner = owner;
            tempUnit.GetComponent<Health>().armor = owner.armorLevel;

            if (tempUnit.GetComponent<Shooter>())
            {
                tempUnit.GetComponent<Shooter>().u = tempUnit;
            }

            if (tempUnit.GetComponent<MoveForward>())
            {
                if (GetComponent<Unit>() && GetComponent<MoveForward>())
                {
                    tempUnit.GetComponent<MoveForward>().startingRotation = GetComponent<MoveForward>().startingRotation;
                }
                else
                {
                    tempUnit.GetComponent<MoveForward>().startingRotation = transform.rotation;
                }
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
            return tempUnit.gameObject;
        }
        else
        {
            print("other than unit spawned");
            GameObject tempObject = Instantiate(prefab, pos, rot);
            return tempObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
