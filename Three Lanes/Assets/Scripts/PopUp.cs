using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public float moveSpeed = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.up, moveSpeed * Time.deltaTime);
    }
}
