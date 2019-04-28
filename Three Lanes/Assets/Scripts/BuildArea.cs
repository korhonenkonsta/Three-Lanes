using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildArea : MonoBehaviour
{
    public GameObject buildingPrefab;
    public Player owner;

    void Start()
    {
        
    }

    public void Build(GameObject building)
    {
        Instantiate(building, transform.position, transform.rotation).GetComponent<Building>().owner = owner;
    }

    //void OnMouseDown()
    //{
    //    Build(buildingPrefab);
    //    // this object was clicked - do something
    //}

    void Update()
    {
        
    }
}
