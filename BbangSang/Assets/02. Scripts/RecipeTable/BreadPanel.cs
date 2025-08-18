using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreadPanel : MonoBehaviour
{
    public Text nameText;
    public Text priceText;
    public Text descText;
    public Button cancelButton;
    public Button selectButton;
    private GameObject recipeInfoPanel;

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
    }


    public void SetBreadInfo(Bread bread)
    {
        nameText.text = bread.Name;
        priceText.text = bread.BreadPrice.ToString() + "원";
        descText.text = bread.Description;
    }


    public void ExitButton()
    {
        gameObject.SetActive(false);
        recipeInfoPanel.SetActive(false);
    }
}

