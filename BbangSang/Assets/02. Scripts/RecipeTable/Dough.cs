using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dough : MonoBehaviour
{
    public int recipeId;
    public int doughCount;
    public Image image;
    public TextMeshProUGUI count;
    public Button button;

    public void Init(int recipeId, int itemCount, Action<int, int> onClick)
    {
        this.recipeId = recipeId;
        this.doughCount = itemCount;
        image.sprite = BreadManager.Instance.GetRecipe(recipeId).GetIcon();
        count.text = itemCount.ToString();
        button.onClick.RemoveAllListeners();
        // Dough가 상황 따라 다르게 쓰일 수 있어서 onClick 액션 담아가도록 설정함
        // 다른데에서 쓰여야할 경우 Recipe가 매개변수 타입인 함수 만들어서 넣으면 됨
        button.onClick.AddListener(() =>
        {
            onClick?.Invoke(recipeId, itemCount);
        });
    }


}
