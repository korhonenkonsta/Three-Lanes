using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public Player owner;
    protected bool isQuitting = false;
    public GameObject mainBasePrefab;

    void Start()
    {
        
    }

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    public void OnDeath()
    {
        SpawnMainBase(mainBasePrefab);
        owner.baseCount--;
    }

    //void OnDestroy()
    //{
    //    if (!isQuitting)
    //    {
            
            
    //    }
    //}

    public void SpawnMainBase(GameObject prefab)
    {
        Instantiate(prefab, transform.position, transform.rotation).GetComponent<BaseMain>().owner = owner;
    }

    void Update()
    {

    }
}
