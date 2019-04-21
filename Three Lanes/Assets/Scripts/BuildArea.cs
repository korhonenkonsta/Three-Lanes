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

    void OnMouseDown()
    {
        Instantiate(buildingPrefab, transform.position, transform.rotation).GetComponent<Building>().owner = owner;
        // this object was clicked - do something
    }

    void Update()
    {
        
    }
}
