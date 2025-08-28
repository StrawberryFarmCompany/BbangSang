
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [SerializeField] private Transform breadContext;
    [SerializeField] private GameObject breadPrefabs;

    [SerializeField] private Button exit;
    public GameObject recipeInfoPanel;

    public RecipeList breadList { get { return BreadManager.Instance.recipeList; } }

    private void Start()
    {
        exit.onClick.AddListener(ExitButton);

        LoadRecipes();
    }


    // 패널 닫기
    public void ExitButton()
    {
        gameObject.SetActive(false);
    }

    public void LoadRecipes()
    {
        for (int i = 0; i < breadList.Recipes.Count; i++)
        {
            GameObject newbread = Instantiate(breadPrefabs, breadContext);
            Recipe recipe = breadList.Recipes[i];

            Bread bread = newbread.GetComponent<Bread>();
            if (bread != null)
            {
                bread.SetBread(recipe); // 슬롯에 데이터 & 이미지 적용
            }
        }
    }

    //public void ShowBreadInfoByID(int id)
    //{
    //    // 조건에 맞는 빵 하나 찾기
    //    Recipe selectedBread = breadList.Recipes.Find(b => b.ID == id);
    //    if (selectedBread != null)
    //    {
    //        selectedBread.Icon = Resources.Load<Sprite>($"Art/{selectedBread.Name}"); // 아이콘 로드

    //        recipeInfoPanel.GetComponent<BreadInfoPanel>().SetBreadInfo(selectedBread);
    //        recipeInfoPanel.SetActive(true);
    //    }
    //    else
    //    {
    //        Debug.LogWarning($"'{id}' 를 찾을 수 없습니다.");
    //    }
    //}
}
