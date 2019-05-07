using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Player owner;
    public List<GameObject> cards = new List<GameObject>();

    public void DrawHand(int amount)
    {
        if (owner.deck.transform.childCount >= amount)
        {
            DrawAmountOfCards(amount);
        }
        else
        {
            //Shuffle discardpile to drawpile, then draw
            foreach (GameObject card in owner.discardPile.cards)
            {
                owner.deck.cards.Add(card);
                card.transform.SetParent(owner.deck.transform);
            }

            owner.discardPile.cards.Clear();

            if (owner.deck.transform.childCount >= amount)
            {
                DrawAmountOfCards(amount);
            }
            else
            {
                DrawAmountOfCards(owner.deck.transform.childCount);
            }
        }
    }

    void DrawAmountOfCards(int amount)
    {
        int index;
        for (int i = 0; i < amount; i++)
        {
            index = Random.Range(0, owner.deck.cards.Count);
            cards.Add(owner.deck.cards[index]);
            owner.deck.cards[index].transform.SetParent(transform);
            owner.deck.cards.Remove(owner.deck.cards[index]);
        }
    }

    public GameObject SelectRandomCard()
    {
        //Add card type as parameter
        if (cards.Count > 0)
        {
            int index = Random.Range(0, cards.Count - 1);
            return cards[index];
        }
        else
        {
            return null;
        }
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
