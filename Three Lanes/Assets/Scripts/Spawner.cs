using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public bool doSpawn = true;
    public float spawnInterval = 1;
    public Player owner;
    public int laneNumber;

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
        if (prefab.gameObject.GetComponent<Unit>())
        {
            Unit tempUnit = Instantiate(prefab, transform.position, transform.rotation).GetComponent<Unit>();
            tempUnit.owner = owner;
            tempUnit.gameObject.GetComponent<Health>().owner = owner;
            owner.opponent.enemyUnits.Add(tempUnit.transform);
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
