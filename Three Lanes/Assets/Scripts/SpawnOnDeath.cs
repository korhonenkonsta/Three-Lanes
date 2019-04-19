using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDeath : MonoBehaviour
{
    public GameObject prefabToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnDestroy()
    {
        Spawn(prefabToSpawn);
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
