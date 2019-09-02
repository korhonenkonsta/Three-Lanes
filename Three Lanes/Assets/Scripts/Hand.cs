using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Player owner;
    public List<GameObject> cards = new List<GameObject>();

    public Queue<GameObject> discardQueue = new Queue<GameObject>();

    public int handSize = 7;
    public float drawInterval = 10f;
    public float nextDrawTime;

    void Start()
    {
        if (owner)
        {
            AddChildCardsToList();
        }
    }

    public void AddChildCardsToList()
    {
        foreach (Transform child in transform)
        {
            cards.Add(child.gameObject);
            child.GetComponent<Card>().owner = owner;
        }
    }

    public void StartDrawing()
    {
        StartCoroutine(ContinuousDraw());
    }

    private IEnumerator ContinuousDraw()
    {
        while (gameObject)
        {
            DiscardMarkedAndDrawReplacements();
            yield return new WaitForSeconds(drawInterval);
        }
    }

    public void DiscardMarkedAndDrawReplacements()
    {
        int discardCount = 0;
        int cardCount = cards.Count;
        for (int i = cardCount - 1; i >= 0; i--)
        {
            if (cards[i].GetComponent<Card>().selectedForDiscard)
            {
                discardCount++;
                cards[i].transform.SetParent(owner.discardPile.transform);
                cards[i].GetComponent<Card>().ToggleDiscard();
                owner.discardPile.cards.Add(cards[i]);

                cards.Remove(cards[i]);
            }
        }

        DrawHandShuffleIfNeeded(discardCount);
        DrawHandShuffleIfNeeded(handSize - cards.Count);
    }

    public void DiscardAll()
    {
        int cardCount = cards.Count;
        for (int i = cardCount - 1; i >= 0; i--)
        {
            if (cards[i].GetComponent<Card>().selectedForDiscard)
            {
                cards[i].transform.SetParent(owner.discardPile.transform);
                cards[i].GetComponent<Card>().ToggleDiscard();
                owner.discardPile.cards.Add(cards[i]);

                cards.Remove(cards[i]);
            }
        }
    }

    public void DiscardCard(GameObject cardObject)
    {
        cardObject.transform.SetParent(owner.discardPile.transform);
        cardObject.GetComponent<Card>().selectedForDiscard = false;
        cardObject.GetComponent<Card>().ToggleColor();

        owner.discardPile.cards.Add(cardObject);
        cards.Remove(cardObject);
    }

    public void ShuffleHandToDeck()
    {
        int cardCount = cards.Count;

        for (int i = cardCount - 1; i >= 0; i--)
        {
            cards[i].transform.SetParent(owner.deck.transform);
            owner.deck.cards.Add(cards[i]);
            cards.Remove(cards[i]);
        }
    }

    public void DrawHandShuffleIfNeeded(int amount)
    {
        if (owner)
        {
            print(owner);
            if (owner.deck.transform.childCount >= amount)
            {
                if (amount > handSize - cards.Count)
                {
                    DrawAmountOfCards(handSize - cards.Count);
                }
                else
                {
                    DrawAmountOfCards(amount);
                }
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
                    DrawAmountOfCards(owner.deck.cards.Count);
                }
            }
        }
    }

    void DrawAmountOfCards(int amount)
    {
        if (amount <= 0 /*&& owner.deck.cards.Count > 0*/)
        {
            return;
        }

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

    void Update()
    {

        if (discardQueue.Count > 0)
        {
            if (Time.time > nextDrawTime)
            {
                nextDrawTime = Time.time + drawInterval;
                if (discardQueue.Peek().GetComponent<Card>().selectedForDiscard)
                {
                    DiscardCard(discardQueue.Dequeue());
                }
                else
                {
                    discardQueue.Dequeue();
                }
                
                DrawHandShuffleIfNeeded(1);
                print("Deck card count:" +owner.deck.cards.Count);
            }
        }
        else
        {
            nextDrawTime = Time.time + drawInterval;
        }
    }
}
