using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject buildingPrefab;
    public Player owner;

    public bool selectedForDiscard;

    //When the mouse hovers over the GameObject, it turns to this color (red)
    Color m_MouseOverColor = Color.red;

    //This stores the GameObject’s original color
    Color m_OriginalColor;

    Image img;

    void Start()
    {
        transform.Find("Card Title").GetComponent<TextMeshProUGUI>().text = buildingPrefab.name;
        transform.Find("Card Description").GetComponent<TextMeshProUGUI>().text = buildingPrefab.GetComponent<Building>().description;

        //Fetch the mesh renderer component from the GameObject
        //Fetch the original color of the GameObject
        img = GetComponent<Image>();
        m_OriginalColor = img.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("Mouse over");
        // Change the color of the GameObject to red when the mouse is over GameObject
        img.color = m_MouseOverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset the color of the GameObject back to normal
        img.color = m_OriginalColor;
        //var tempColor = img.color;
        //tempColor.a = 1f;
        //img.color = tempColor;
    }

    void Update()
    {
        
    }
}
