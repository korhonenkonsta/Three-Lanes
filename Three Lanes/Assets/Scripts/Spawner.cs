﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public bool doSpawn = true;
    public float spawnInterval = 1;

    // Start is called before the first frame update
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
        Instantiate(prefab, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
