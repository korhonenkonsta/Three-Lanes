using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    public float resourceInterval = 5;
    public bool doGenerate = true;
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
        while (doGenerate)
        {
            yield return new WaitForSeconds(resourceInterval);
            owner.roundExtraResources++;
        }
    }

    void Update()
    {
        
    }
}
