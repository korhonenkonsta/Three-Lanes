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

    public void Build(GameObject building, int cost, Draggable d, Card c)
    {
        if (owner.resources >= cost)
        {
            GameObject buildingTemp = Instantiate(building, transform.position, transform.rotation);
            Building b = buildingTemp.GetComponent<Building>();

            buildingTemp.GetComponent<Spawner>().owner = owner;
            b.owner = owner;
            buildingTemp.GetComponent<Spawner>().currentLane = currentLane;
            b.currentLane = currentLane;

            d.transform.SetParent(owner.discardPile.transform);
            //d.parentToReturnTo = owner.discardPile.transform;
            owner.discardPile.cards.Add(c.gameObject);
            owner.hand.cards.Remove(c.gameObject);

            owner.resources -= b.cost;
        }
        else
        {
            print("Not enough resources!");
        }
        
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
