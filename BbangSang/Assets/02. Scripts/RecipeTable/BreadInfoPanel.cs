using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreadInfoPanel : MonoBehaviour
{
    public Image breadImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI descText;
    public Button cancelButton;
    public Button selectButton;

    private Recipe currentRecipe;

    public void Start()
    {
        cancelButton.onClick.AddListener(ExitButton);
        selectButton.onClick.AddListener(SelectButton);
    }


    public void SetBreadInfo(Recipe recipe)
    {
        currentRecipe = recipe; // 저장

        breadImage.sprite = recipe.GetIcon();
        nameText.text = recipe.Name;
        priceText.text = recipe.BreadPrice.ToString() + "원";
        descText.text = recipe.Description;
    }


    public void ExitButton()
    {
        gameObject.SetActive(false);
    }

    public void SelectButton()
    {
        if (currentRecipe.Icon != null)
        {
            SelectedBread selector = FindObjectOfType<SelectedBread>();
            if (selector != null)
            {
                selector.ShowBreadIcon(currentRecipe.GetIcon());
            }
        }

        if (BreadManager.Instance != null && currentRecipe != null)
        {
            BreadManager.Instance.SelectRecipe(currentRecipe.ID);
            Debug.Log($"[RECIPE] 선택: ID {currentRecipe.ID} ({currentRecipe.Name})");
        }

        var listUI = FindObjectOfType<DoughListUI>();
        if (listUI != null) listUI.Refresh();

        gameObject.SetActive(false);
    }
}

