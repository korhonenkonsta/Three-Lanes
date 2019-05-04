using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildArea : MonoBehaviour
{
    public GameObject buildingPrefab;
    public Player owner;
    public Lane currentLane;

    void Start()
    {
        
    }

    public void Build(GameObject building)
    {
        GameObject buildingTemp = Instantiate(building, transform.position, transform.rotation);
        buildingTemp.GetComponent<Spawner>().owner = owner;
        buildingTemp.GetComponent<Building>().owner = owner;
        buildingTemp.GetComponent<Building>().currentLane = currentLane;
        buildingTemp.GetComponent<Spawner>().currentLane = currentLane;
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
