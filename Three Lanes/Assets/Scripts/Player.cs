using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int baseCount;
    public int roundScore;

    public Player opponent;
    public GameManager gm;

    void Start()
    {
        
    }

    public void LoseRound()
    {
        opponent.roundScore++;
        gm.ResetRound();
    }

    void Update()
    {
        
    }
}
