using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    public float resourceInterval = 5;
    public bool doSpawn = true;
    public Player owner;
    public Building b;

    void Start()
    {
        
    }

    public void Init(Player p, Building bu)
    {
        owner = p;
        b = bu;
        StartCoroutine(ContinuousGenerate());
    }

    IEnumerator ContinuousGenerate()
    {
        while (doSpawn)
        {
            
            yield return new WaitForSeconds(resourceInterval);
            owner.roundExtraResources++;
        }
    }

    void Update()
    {
        
    }
}
