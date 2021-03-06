﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildArea : MonoBehaviour
{
    public GameObject buildingPrefab;
    public Player owner;
    public Lane currentLane;
    public Building b;

    void Start()
    {
        
    }

    public void Build(GameObject building, int cost, Draggable d, Card c)
    {
        if (owner.resources + owner.roundExtraResources >= cost)
        {
            if (owner.roundExtraResources >= cost)
            {
                owner.roundExtraResources -= cost;
            }
            else
            {
                int remainder = cost - owner.roundExtraResources;
                owner.resources -= remainder;
                owner.roundExtraResources = 0;
            }

            GameObject buildingTemp = Instantiate(building, transform.position + new Vector3(0f, 1f, 0f), transform.rotation);
            b = buildingTemp.GetComponent<Building>();

            if (buildingTemp.GetComponent<Spawner>())
            {
                buildingTemp.GetComponent<Spawner>().owner = owner;
                buildingTemp.GetComponent<Spawner>().currentLane = currentLane;
            }
            
            b.owner = owner;
            b.currentLane = currentLane;

            //if (buildingTemp.GetComponent<ResourceGenerator>())
            //{
            //    buildingTemp.GetComponent<ResourceGenerator>().Init(owner, b);
            //}

            owner.hand.DiscardCard(d.gameObject);
            owner.hand.discardQueue.Enqueue(d.gameObject);
            //d.transform.SetParent(owner.discardPile.transform);
            d.parentToReturnTo = owner.discardPile.transform;
            //owner.discardPile.cards.Add(c.gameObject);
            //owner.hand.cards.Remove(c.gameObject);

            owner.availableBuildAreas.Remove(this);
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
