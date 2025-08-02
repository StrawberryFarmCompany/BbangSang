using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreadPanel : MonoBehaviour
{
    public Text nameText;
    public Text priceText;
    public Text descText;

    public void SetBreadInfo(Bread bread)
    {
        nameText.text = bread.Name;
        priceText.text = bread.BreadPrice.ToString() + "¿ø";
        descText.text = bread.Description;
    }
}

