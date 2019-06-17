using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustSaturation : MonoBehaviour
{
    public Image mainImage;

    void Start()
    {
        Color color = mainImage.color;
        Color.RGBToHSV(color, out float H, out float S, out float V);
        S *= 0.5f;
        GetComponent<Image>().color = Color.HSVToRGB(H, S, V);
    }

    void Update()
    {
        
    }
}
