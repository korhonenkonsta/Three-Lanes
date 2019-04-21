using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMain : Base
{
    void Start()
    {
        
    }

    public override void OnDeath()
    {
        owner.LoseRound();
    }

    void Update()
    {
        
    }
}
