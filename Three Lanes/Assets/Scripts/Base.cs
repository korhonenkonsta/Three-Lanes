using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public Player owner;
    public GameObject mainBasePrefab;

    void Start()
    {
        
    }

    public virtual void OnDeath()
    {
        SpawnMainBase(mainBasePrefab);
        owner.baseCount--;

        if (owner.baseCount < 2)
        {
            owner.LoseRound();
        }
    }

    public void SpawnMainBase(GameObject prefab)
    {
        GameObject baseTemp = Instantiate(prefab, transform.position, transform.rotation);
        baseTemp.GetComponent<BaseMain>().owner = owner;
        baseTemp.layer = 2;
    }

    void Update()
    {

    }
}
