using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceGenerator : MonoBehaviour
{
    public float resourceInterval = 5;
    public float nextResourceTime = 0;
    public int resourceAmount = 1;
    public bool doGenerate = true;
    public Player owner;
    public Building b;

    public GameObject resourcePopupPrefab;
    public float height = 1f;

    public Image coolDownBarForeground;
    public float fill;

    void Start()
    {
        
    }

    public void Init(Player p, Building bu)
    {
        owner = p;
        b = bu;
        StartCoroutine(ContinuousGenerate());
    }

    public void CreatePopup()
    {
        GameObject popup = Instantiate(resourcePopupPrefab, transform.position + new Vector3(0, height, 0), Quaternion.identity);
        popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + resourceAmount.ToString();
        Destroy(popup, 3f);
    }

    IEnumerator ContinuousGenerate()
    {
        while (doGenerate)
        {
            yield return new WaitForSeconds(resourceInterval);
            owner.roundExtraResources += resourceAmount;
            CreatePopup();
        }
    }

    void Update()
    {
        if (Time.time > nextResourceTime)
        {
            nextResourceTime = Time.time + resourceInterval;
            fill = 0;
        }

        fill += Time.deltaTime;

        if (coolDownBarForeground)
        {
            coolDownBarForeground.fillAmount = fill / resourceInterval;
        }

    }
}
