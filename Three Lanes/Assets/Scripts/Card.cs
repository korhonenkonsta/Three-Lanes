using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public GameObject buildingPrefab;
    public Player owner;

    void Start()
    {
        transform.Find("Card Title").GetComponent<TextMeshProUGUI>().text = buildingPrefab.name;
        transform.Find("Card Description").GetComponent<TextMeshProUGUI>().text = buildingPrefab.GetComponent<Building>().description;
    }

    void Update()
    {
        
    }
}
