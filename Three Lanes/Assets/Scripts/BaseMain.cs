using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMain : MonoBehaviour
{
    protected bool isQuitting = false;
    public Player owner;

    void Start()
    {
        
    }

    void OnDestroy()
    {
        if (!isQuitting)
        {
            owner.baseCount -= 2;
        }
    }

    void Update()
    {
        
    }
}
