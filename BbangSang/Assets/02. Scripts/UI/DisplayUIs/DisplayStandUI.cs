using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStandUI : MonoBehaviour
{
    public GameObject displayChoose;
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
        if (!breadPrefab.TryGetComponent(out Dough dough))
        {
            Debug.LogError("doughPrefab에 dough 클래스가 없음");
        }
        RefreshUI();
    }
    
    public void RefreshUI()
    {
        breads.Clear();
        BreadManager.Instance.GetAllDough(breads); // doughs 에 현재 가지고 있는 반죽 아이디 : 개수 들어옴
        ClearDoughs(); // 일단 doughParent 안에 있는 애들 싹 다 지움

        //빵이 있을 때
        if (breads.Count > 0)
        {
            foreach(var dough in breads)
            {
                GameObject go =  Instantiate(breadPrefab, breadParent); //슬롯 생성
                Dough d = go.GetComponent<Dough>();
                d.Init(dough.recipeId, dough.count,
                    (recipeId, doughCount) =>
                    {
                        displayChooseUI.Setting(recipeId, dough.count); //수량 세팅
                        displayChooseUI.gameObject.SetActive(true); //수량 UI 활성화
                    });
                go.SetActive(true);
            }
        }
    }
    
    void ClearDoughs()
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
        displayChoose.gameObject.SetActive(true);
    }
}
