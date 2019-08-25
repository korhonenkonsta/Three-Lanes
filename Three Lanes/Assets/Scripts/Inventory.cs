using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Player owner;
    public List<GameObject> items = new List<GameObject>();

    void Start()
    {
        if (owner)
        {
            AddChildItemsToList();
        }
    }

    public void AddChildItemsToList()
    {
        foreach (Transform child in transform)
        {
            items.Add(child.gameObject);
            child.GetComponent<Card>().owner = owner;
            child.GetComponent<Item>().owner = owner;
            if (child.GetComponent<Spawner>())
            {
                child.GetComponent<Spawner>().owner = owner;
            }
        }
    }

    public void InitSpawners()
    {
        foreach (GameObject item in items)
        {
            if (item.GetComponent<Spawner>())
            {
                item.GetComponent<Spawner>().Init();
            }
        }
    }

    void Update()
    {
        
    }
}
