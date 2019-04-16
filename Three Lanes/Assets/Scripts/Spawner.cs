using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public bool doSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Example());
    }

    IEnumerator Example()
    {
        while (doSpawn)
        {
            Spawn(prefabToSpawn);
            yield return new WaitForSeconds(5);
        }
    }

    public void Spawn(GameObject prefab)
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
