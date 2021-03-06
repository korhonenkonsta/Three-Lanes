﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Spawner : MonoBehaviour
{
    public bool isGenerator;
    public int resourceAmount = 1;
    public GameObject prefabToSpawn;
    public bool doSpawn = true;
    public float spawnInterval = 5f;
    public float spawnIntervalStep = 1f;
    public float spawnIntervalMin = 1f;
    public bool hasWindup;
    public Player owner;
    public Lane currentLane;

    public GameObject resourcePopupPrefab;
    public float height = 1f;

    public float nextSpawnTime = 0;
    public Image coolDownBarForeground;
    public float fill;

    public Image remainingBarForeground;

    public bool isTemporary;
    public int spawnCount;
    public int spawnCountBeforeDestroy = 10;

    public bool isGlobal;

    private IEnumerator coroutine;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        if (GetComponent<Building>() || GetComponent<Unit>() || GetComponent<Item>())
        {
            if (owner)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }

                coroutine = ContinuousSpawn();
                StartCoroutine(coroutine);

                nextSpawnTime = Time.time + spawnInterval;
                fill = 0;
            }
        }
    }

    public void CreatePopup()
    {
        GameObject popup = Instantiate(resourcePopupPrefab, transform.position + new Vector3(0, height, 0), Quaternion.identity);
        popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + resourceAmount.ToString();
        Destroy(popup, 3f);
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

            if (isGenerator)
            {
                if (isGlobal)
                {
                    owner.resources += resourceAmount;
                }
                else
                {
                    owner.roundExtraResources += resourceAmount;
                }

                CreatePopup();
            }
            else
            {
                if (GetComponent<Item>())
                {
                    if (owner.allBuildAreas.Count > 0)
                    {
                        BuildArea b = owner.allBuildAreas[0];
                        if (b)
                        {
                            Transform t = b.transform;
                            Spawn(prefabToSpawn, t.position + t.forward * 0.4f + t.up * 0.5f, t.rotation, owner, b.currentLane, 0);
                        }
                    }
                }
                else if (prefabToSpawn.GetComponent<Unit>())
                {
                    print(prefabToSpawn.name);
                    if (prefabToSpawn.name == "Mine")
                    {
                        Spawn(prefabToSpawn, transform.position - transform.forward * 0.4f, transform.rotation, owner, currentLane, 0);
                    }
                    else
                    {
                        Spawn(prefabToSpawn, transform.position + transform.forward * 0.4f, transform.rotation, owner, currentLane, 0);
                    }
                }
                else
                {
                    Spawn(prefabToSpawn, transform.position + transform.forward * 0.4f, transform.rotation, owner, currentLane, 0);
                }
            }
            
            spawnCount++;

            if (isTemporary)
            {
                remainingBarForeground.fillAmount = 1 - spawnCount / (float)spawnCountBeforeDestroy;

                if (spawnCount >= spawnCountBeforeDestroy)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot, Player ownerPlayer, Lane lane, int cost)
    {
        owner = ownerPlayer;

        if (GetComponent<Unit>())
        {
            cost = 0;
        }

        if (owner.resources + owner.roundExtraResources >= cost)
        {
            if (owner.roundExtraResources >= cost)
            {
                owner.roundExtraResources -= cost;
            }
            else
            {
                int remainder = cost - owner.roundExtraResources;
                owner.resources -= remainder;
                owner.roundExtraResources = 0;
            }

            if (prefab.gameObject.GetComponent<Unit>())
            {
                Unit tempUnit = Instantiate(prefab, pos, rot).GetComponent<Unit>();

                currentLane = lane;

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

                foreach (GameObject item in owner.inventory.items)
                {
                    if (item.name == "Item Creeps Explode(Clone)")
                    {
                        Explosion explosion = tempUnit.gameObject.AddComponent<Explosion>();
                        explosion.explosionEffect = Resources.Load("EnergyExplosion") as GameObject;
                    }
                    else if (item.name == "Item +1 Regen All Units(Clone)")
                    {
                        tempUnit.gameObject.GetComponent<Health>().hpRegen++;
                    }
                    else if (item.name == "Item +1 Hp All Units(Clone)")
                    {
                        tempUnit.gameObject.GetComponent<Health>().ChangeMaxHp(1);
                    }
                }

                //Add to list
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
        else
        {
            return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnInterval;
            fill = 0;
        }

        fill += Time.deltaTime;

        if (coolDownBarForeground)
        {
            coolDownBarForeground.fillAmount = fill / spawnInterval;
        }

    }
}
