using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject buildingPrefab;
    public GameObject spellPrefab;
    public Player owner;

    public bool selectedForDiscard;

    //When the mouse hovers over the GameObject, it turns to this color (red)
    Color m_DiscardColor = Color.red;

    //This stores the GameObject’s original color
    Color m_OriginalColor;

    Image img;

    public UnityEvent onLeft;
    public UnityEvent onRight;
    public UnityEvent onMiddle;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onLeft.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRight.Invoke();

            ToggleDiscard();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            onMiddle.Invoke();
        }
    }

    public void ToggleDiscard()
    {
        selectedForDiscard = !selectedForDiscard;
        if (selectedForDiscard)
        {
            img.color = m_DiscardColor;
        }
        else
        {
            img.color = m_OriginalColor;
        }
    }

    void Start()
    {
        string value = null;
        if (buildingPrefab)
        {
            value = buildingPrefab.name;

            value = value.Substring(0, 8);
            if (value == "Building")
            {
                value = buildingPrefab.name;
                value = value.Remove(0, 8);
            }
            else
            {
                value = buildingPrefab.name;
            }

            transform.Find("Manacost Background").GetChild(0).GetComponent<TextMeshProUGUI>().text = buildingPrefab.GetComponent<Building>().cost.ToString();
            transform.Find("Card Description").GetComponent<TextMeshProUGUI>().text = buildingPrefab.GetComponent<Building>().description;
        }
        else
        {
            value = spellPrefab.name;

            if (spellPrefab.GetComponent<Unit>())
            {
                transform.Find("Manacost Background").GetChild(0).GetComponent<TextMeshProUGUI>().text = spellPrefab.GetComponent<Unit>().cost.ToString();
            }
            else
            {
                value += " " + spellPrefab.GetComponent<Spell>().cost;
                transform.Find("Manacost Background").GetChild(0).GetComponent<TextMeshProUGUI>().text = spellPrefab.GetComponent<Spell>().cost.ToString();
            }

            transform.Find("Card Description").GetComponent<TextMeshProUGUI>().text = "Spell description here";
        }

        


        transform.Find("Card Title").GetComponent<TextMeshProUGUI>().text = value;
        


        //Fetch the mesh renderer component from the GameObject
        //Fetch the original color of the GameObject
        img = GetComponent<Image>();
        m_OriginalColor = img.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change the color of the GameObject to red when the mouse is over GameObject
        //img.color = m_MouseOverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset the color of the GameObject back to normal
        //img.color = m_OriginalColor;
    }

    void Update()
    {
        
    }
}
