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
        Instantiate(prefab, transform.position, transform.rotation).GetComponent<BaseMain>().owner = owner;
    }

    void Update()
    {

    }
}
