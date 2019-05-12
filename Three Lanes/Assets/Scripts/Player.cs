using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int baseCount;
    public int roundScore;

    public Player opponent;
    public GameManager gm;

    public List<Transform> enemyUnitsAll = new List<Transform>();
    public List<Transform> enemyUnits1 = new List<Transform>();
    public List<Transform> enemyUnits2 = new List<Transform>();
    public List<Transform> enemyUnits3 = new List<Transform>();

    public List<BuildArea> availableBuildAreas = new List<BuildArea>();

    public Deck deck;
    public DiscardPile discardPile;
    public Hand hand;

    public int resources;
    public int startingResources;
    public int roundExtraResources;

    //public Lane lane1;
    //public Lane lane2;
    //public Lane lane3;

    void Start()
    {
        
    }

    public void ResetBuildAreas()
    {
        foreach (BuildArea BA in availableBuildAreas)
        {
            if (BA.b)
            {
                if (!BA.b.permanent)
                {
                    BA.b = null;
                    availableBuildAreas.Remove(BA);
                }
            }
        }
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
            gm.GameOver();
        }
        else
        {
            gm.ResetRound();
        }
    }

    void Update()
    {
        
    }
}
