using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreadPanel : MonoBehaviour
{
    public Image breadImage;
    public Text nameText;
    public Text priceText;
    public Text descText;
    public Button cancelButton;
    public Button selectButton;
    private GameObject recipeInfoPanel;

    private Bread currentBread;

    public void Start()
    {
        if (recipeInfoPanel == null)
        {
            // 부모 중에 이름이 RecipeInfoPanel인 GameObject 찾아서 자동 지정
            Transform parent = transform;
            while (parent != null)
            {
                if (parent.name == "RecipeInfoPanel")
                {
                    recipeInfoPanel = parent.gameObject;
                    break;
                }
                parent = parent.parent;  // 확인한 오브젝트의 부모를 부모로 다시 지정
            }
        }

        cancelButton.onClick.AddListener(ExitButton);
        selectButton.onClick.AddListener(SelectButton);
    }


    public void SetBreadInfo(Bread bread)
    {
        currentBread = bread; // 저장

        breadImage.sprite = bread.Icon;
        nameText.text = bread.Name;
        priceText.text = bread.BreadPrice.ToString() + "원";
        descText.text = bread.Description;
    }


    public void ExitButton()
    {
        recipeInfoPanel.SetActive(false);
    }

    public void SelectButton()
    {
        if (currentBread.Icon != null)
        {
            SelectedBread selector = FindObjectOfType<SelectedBread>();
            if (selector != null)
            {
                selector.ShowBreadIcon(currentBread.Icon);
            }
        }

        if (BreadManager.Instance != null && currentBread != null)
        {
            BreadManager.Instance.SelectRecipe(currentBread.ID);
            Debug.Log($"[RECIPE] 선택: ID {currentBread.ID} ({currentBread.Name})");
        }

        var listUI = FindObjectOfType<DoughListUI>();
        if (listUI != null) listUI.Refresh();

        recipeInfoPanel.SetActive(false);
    }
}

