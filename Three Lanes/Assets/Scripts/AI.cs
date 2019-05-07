using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public Player p;
    public bool doBuild = true;
    public float buildInterval = 3;

    void Start()
    {
        StartCoroutine(ContinuousBuild());
    }

    IEnumerator ContinuousBuild()
    {
        while (doBuild)
        {
            BuildRandom();
            yield return new WaitForSeconds(buildInterval);
        }
    }

    public void BuildRandom()
    {
        //Need to differentiate between card types, to select only buildings
        GameObject cardObj = p.hand.SelectRandomCard();
        if (!cardObj)
        {
            print("No cards found in hand");
            return;
        }
        Card c = cardObj.GetComponent<Card>();
        //!!!!
        //Need to check if building on build area already exists
        //!!!!
        p.buildAreas[Random.Range(0, p.buildAreas.Count)].Build(c.buildingPrefab, c.buildingPrefab.GetComponent<Building>().cost, c.GetComponent<Draggable>(), c);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            BuildRandom();
        }
    }
}
