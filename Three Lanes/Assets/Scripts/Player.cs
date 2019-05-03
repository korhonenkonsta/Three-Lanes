﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int baseCount;
    public int roundScore;

    public Player opponent;
    public GameManager gm;

    public List<Transform> enemyUnits = new List<Transform>();

    void Start()
    {
        
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
