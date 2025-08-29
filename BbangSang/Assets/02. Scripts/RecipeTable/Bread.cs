using UnityEngine;
using UnityEngine.UI;

public class Bread : MonoBehaviour
{
    [SerializeField] private Image breadImage;
    [SerializeField] private Button breadButton;
    [SerializeField] private GameObject breadInfoPanel;

    private Recipe currentRecipe;

    private void Start()
    {
        breadButton.onClick.AddListener(OpenBreadInfoPanel);
    }

    public void SetBread(Recipe recipe)
    {
        currentRecipe = recipe;
        breadImage.sprite = recipe.GetIcon();
    }

    public void OpenBreadInfoPanel()
    {
        // BreadInfoPanel 자동 할당 (비활성화된 오브젝트까지 포함해서 탐색)
        if (breadInfoPanel == null)
        {
            BreadInfoPanel panel = FindObjectOfType<BreadInfoPanel>(true); // true 넣어야 비활성화된 것도 찾음
            if (panel != null)
                breadInfoPanel = panel.gameObject;
        }

        if (breadInfoPanel != null)
        {
            breadInfoPanel.SetActive(true);

            BreadInfoPanel panelScript = breadInfoPanel.GetComponent<BreadInfoPanel>();
            if (panelScript != null && currentRecipe != null)
            {
                panelScript.SetBreadInfo(currentRecipe);
            }
        }
        else
        {
            Debug.LogError("BreadInfoPanel을 찾을 수 없습니다. 씬에 존재하는지 확인하세요!");
        }
    }
}
