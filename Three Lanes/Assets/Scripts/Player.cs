using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int baseCount;
    public int roundScore;
    public int winCount;

    public Player opponent;
    public GameManager gm;

    public List<Transform> enemyUnitsAll = new List<Transform>();
    public List<Transform> enemyUnits1 = new List<Transform>();
    public List<Transform> enemyUnits2 = new List<Transform>();
    public List<Transform> enemyUnits3 = new List<Transform>();

    public List<BuildArea> allBuildAreas = new List<BuildArea>();
    public List<BuildArea> availableBuildAreas = new List<BuildArea>();

    public Deck deck;
    public DiscardPile discardPile;
    public Hand hand;
    public Inventory inventory;

    public int resources;
    public int startingResources;
    public int roundExtraResources;

    public int armorLevel;

    //public Lane lane1;
    //public Lane lane2;
    //public Lane lane3;

    void Start()
    {
        
    }

    public void ResetBuildAreas()
    {
        foreach (BuildArea BA in allBuildAreas)
        {
            if (BA.b)
            {
                if (!BA.b.permanent)
                {
                    BA.b = null;
                    availableBuildAreas.Add(BA);
                }
            }
        }
    }

    public void ClearBuildAreaLists()
    {
        allBuildAreas.Clear();
        allBuildAreas.Clear();
    }

    public void ClearEnemyLists()
    {
        enemyUnitsAll.Clear();
        enemyUnits1.Clear();
        enemyUnits2.Clear();
        enemyUnits3.Clear();
    }

    public void RemoveUnitFromTargetLists(Transform t)
    {
        enemyUnitsAll.Remove(t);
        enemyUnits1.Remove(t);
        enemyUnits2.Remove(t);
        enemyUnits3.Remove(t);
    }

    public void LoseRound()
    {
        opponent.roundScore++;
        gm.UpdateScoreTexts();

        if (opponent.roundScore >= 2)
        {
            opponent.winCount++;
            opponent.roundExtraResources = 0;
            gm.GameOver(this);
        }
        else
        {
            gm.ResetRound();
        }
    }

    void Update()
    {
        if (roundExtraResources >= 20)
        {
            if (opponent)
            {
                opponent.LoseRound();
            }
        }
        
    }
}
