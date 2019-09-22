using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public Player p;
    public bool doBuild = true;
    public float buildInterval = 5;

    public enum AIType {Rush, Defend, Econ};
    public AIType type;

    void Start()
    {
        StartCoroutine(ContinuousBuild());
    }

    IEnumerator ContinuousBuild()
    {
        while (doBuild)
        {
            if (p.roundScore < 1 && p.opponent.roundScore < 1)
            {
                if ((p.resources + p.roundExtraResources) > p.startingResources / 2)
                {
                    BuildRandom();
                }
            }
            else
            {
                BuildRandom();
            }
            
            yield return new WaitForSeconds(buildInterval);
        }
    }

    public void BuildRandom()
    {
        GameObject cardObj = null;
        switch (type)
        {
            case AIType.Rush:
                cardObj = p.hand.SelectRandomCard(Card.Type.Building, Card.SubType.Factory);
                break;
            case AIType.Defend:
                cardObj = p.hand.SelectRandomCard(Card.Type.Building);
                break;
            case AIType.Econ:
                cardObj = p.hand.SelectRandomCard(Card.Type.Building, Card.SubType.Generator);
                break;
            default:
                break;
        }

        if (!cardObj)
        {
            cardObj = p.hand.SelectRandomCard(Card.Type.Building);
        }

        if (!cardObj)
        {
            print("No cards found in hand");
            return;
        }
        Card c = cardObj.GetComponent<Card>();
        if (p.availableBuildAreas.Count > 0)
        {
            p.availableBuildAreas[Random.Range(0, p.availableBuildAreas.Count)].Build(c.buildingPrefab, c.buildingPrefab.GetComponent<Building>().cost, c.GetComponent<Draggable>(), c);
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            BuildRandom();
        }
    }
}
