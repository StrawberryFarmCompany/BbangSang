using UnityEngine;

public class RecipesLoader : MonoBehaviour
{
    public BreadList breadList;
    public GameObject breadPanelPrefab;
    public Transform panelParent;

    void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("RecipesData"); // 확장자 제외
        breadList = JsonUtility.FromJson<BreadList>(jsonFile.text);

        foreach (var bread in breadList.Recipes)
        {
            Debug.Log($"{bread.Name} - {bread.BreadPrice} 원");
        }
    }



    void ShowBreadUI()
    {
        foreach (var bread in breadList.Recipes)
        {
            GameObject panel = Instantiate(breadPanelPrefab, panelParent);
            panel.GetComponent<BreadPanel>().SetBreadInfo(bread);
        }
    }
}
