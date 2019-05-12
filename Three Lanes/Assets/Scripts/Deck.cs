using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public Player owner;
    public List<GameObject> cards = new List<GameObject>();

    void Start()
    {

    }

    public void AddChildCardsToList()
    {
        foreach (Transform child in transform)
        {
            cards.Add(child.gameObject);
        }
    }

    void Update()
    {
        
    }
}
