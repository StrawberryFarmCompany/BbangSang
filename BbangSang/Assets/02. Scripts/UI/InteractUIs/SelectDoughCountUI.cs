using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectDoughCountUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI doughName;
    public TextMeshProUGUI doughCount;
    public TMP_InputField inputDoughCount;

    private Oven oven;
    private SelectDoughUI  selectDoughUI;

    private int currentDoughId;
    private int currentDoughCount;

    private int selectDoughCount = 0;

    public void Init(Oven oven, SelectDoughUI selectDoughUI)
    {
        this.oven = oven;
        this.selectDoughUI = selectDoughUI;
    }
    public void Setting(int recipeId, int count)
    {
        icon.sprite = BreadManager.Instance.GetRecipe(recipeId).GetIcon();
        doughName.text = BreadManager.Instance.GetRecipe(recipeId).Name;
        doughCount.text = count.ToString();
        currentDoughId = recipeId;
        currentDoughCount = count;
    }

    public void ValidateCountText(string text)
    {
        if (text == String.Empty || text.Length == 0) return;
        if (int.TryParse(text, out int result))
        {
            inputDoughCount.text = Mathf.Min(result, currentDoughCount).ToString();
            selectDoughCount = int.Parse(inputDoughCount.text);
        }
        else
        {
            inputDoughCount.text = String.Empty; 
        }
    }

    public void SelectDoughToOven()
    {
        if(selectDoughCount == 0) return;
        if (oven.TryAddDoughToOven(currentDoughId, selectDoughCount))
        {
            gameObject.SetActive(false);
            selectDoughUI.ChangeSelectedDoughCount();
        }
    }
}
