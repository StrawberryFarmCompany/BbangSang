using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductBread : MonoBehaviour
{
    public int recipeId;
    public int breadCount;
    public Image image;
    public TextMeshProUGUI count;
    public Button button;

    public void Init(int recipeId, int itemCount, Action<int, int> onClick)
    {
        this.recipeId = recipeId;
        this.breadCount = itemCount;
        image.sprite = BreadManager.Instance.GetRecipe(recipeId).GetIcon();
        count.text = itemCount.ToString();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            onClick?.Invoke(recipeId, itemCount);
        });
    }
}
