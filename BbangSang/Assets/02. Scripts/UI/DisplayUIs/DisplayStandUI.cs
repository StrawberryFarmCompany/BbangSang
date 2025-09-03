using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStandUI : MonoBehaviour
{
    public Button exit;
    
    public Transform breadParent;
    public GameObject breadPrefab;
    public GameObject displayChooseObj;
    private DisplayChooseUI displayChooseUI;
    public TextMeshProUGUI selectedBreadCount; 
    
    private List<(int recipeId, int count)> breads = new();
    
    void Awake()
    {
        displayChooseUI = displayChooseObj.GetComponent<DisplayChooseUI>();
    }

    private void Start()
    {
        exit.onClick.AddListener(ExitButton);
        if (!breadPrefab.TryGetComponent(out Dough dough)) //빵으로 변경하기
        {
            Debug.LogError("doughPrefab에 dough 클래스가 없음");
        }
        RefreshUI();
    }
    
    public void RefreshUI()
    {
        breads.Clear();
        BreadManager.Instance.GetAllDough(breads); // recipeId, count 리스트 받기
        ClearBreads();

        //빵이 있을 때
        if (breads.Count == 0) return;
        
        foreach (var bread in breads)
        {
            GameObject go = Instantiate(breadPrefab, breadParent);
            ProductBread b = go.GetComponent<ProductBread>();
            int countCopy = bread.count; // 클로저 문제 방지
            b.Init(bread.recipeId, bread.count, (recipeId, breadCount) =>
            {
                displayChooseUI.Setting(recipeId, countCopy); //수량 세팅
                displayChooseUI.gameObject.SetActive(true); //수량 UI 활성화
            });
            go.SetActive(true);
        }
    }
    
    public void SetDisplaySelection(int recipeId, int count)
    {
        displayChooseUI.Setting(recipeId, count); //선택 UI에 전달한 빵 정보를 반영
        displayChooseUI.gameObject.SetActive(true);
    }
    
    void ClearBreads()
    {
        for (int i = 0; i < breadParent.childCount; i++)
        {
            Destroy(breadParent.GetChild(i).gameObject);
        }
    }
    public void ExitButton()
    {
        gameObject.SetActive(false);
    }
    
    public void DisplayChooseButton()
    {
        displayChooseObj.gameObject.SetActive(true);
    }
}
