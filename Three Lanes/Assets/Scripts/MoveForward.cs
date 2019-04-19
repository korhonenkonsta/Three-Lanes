using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{

    Rigidbody rb;
    public float factor; //0.2

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * Time.deltaTime * factor);
    }
}
