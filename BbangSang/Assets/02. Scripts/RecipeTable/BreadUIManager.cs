using UnityEngine;
using System.Collections.Generic;

public class BreadUIManager : MonoBehaviour
{
    public GameObject breadPanelPrefab;    // BreadPanel 프리팹
    public Transform panelParent;          // UI 패널들이 붙을 부모 오브젝트

    private BreadList breadList;

    void Start()
    {
        LoadBreadData();
    }

    public void LoadBreadData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("RecipesData");
        breadList = JsonUtility.FromJson<BreadList>(jsonFile.text);
    }

    public void ShowBreadInfoByID(int id)
    {
        foreach (Transform child in panelParent)
        {
            Destroy(child.gameObject);
        }

        // 조건에 맞는 빵 하나 찾기
        Bread selectedBread = breadList.Recipes.Find(b => b.ID == id);
        if (selectedBread != null)
        {
            GameObject panel = Instantiate(breadPanelPrefab, panelParent);
            panel.GetComponent<BreadPanel>().SetBreadInfo(selectedBread);
        }
        else
        {
            Debug.LogWarning($"'{id}' 이라는 이름의 빵을 찾을 수 없습니다.");
        }
    }
}
