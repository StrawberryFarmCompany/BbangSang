using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectDoughUI : MonoBehaviour
{
    private Oven oven;

    [SerializeField] public Transform doughParent;
    [SerializeField] public GameObject doughPrefab;
    [SerializeField] public SelectDoughCountUI selectDoughCountUI;
    [SerializeField] public TextMeshProUGUI selectedDoughCount; 
    
    private List<(int recipeId, int count)> doughs = new();
    
    
    public void Init(Oven oven)
    {
        this.oven = oven;
    }

    public void Start()
    {
        if (!doughPrefab.TryGetComponent(out Dough dough))
        {
            Debug.LogError("doughPrefab에 dough 클래스가 없음");
        }
        selectDoughCountUI.Init(oven, this);
        RefreshUI();
    }


    public void RefreshUI()
    {
        doughs.Clear();
        BreadManager.Instance.GetAllDough(doughs); // doughs 에 현재 가지고 있는 반죽 아이디 : 개수 들어옴
        ClearDoughs(); // 일단 doughParent 안에 있는 애들 싹 다 지움

        // 반죽이 있을 때
        if (doughs.Count > 0)
        {
            foreach(var dough in doughs)
            {
                GameObject go =  Instantiate(doughPrefab, doughParent);
                Dough d = go.GetComponent<Dough>();
                d.Init(dough.recipeId, dough.count,
                    (recipeId, doughCount) =>
                    {
                        selectDoughCountUI.Setting(recipeId, dough.count);
                        selectDoughCountUI.gameObject.SetActive(true);
                    });
                go.SetActive(true);
            }
        }
    }

    void ClearDoughs()
    {
        for (int i = 0; i < doughParent.childCount; i++)
        {
            Destroy(doughParent.GetChild(i).gameObject);
        }
    }

    public void ChangeSelectedDoughCount()
    {
        selectedDoughCount.text = oven.InOvenCount.ToString();
    }
}
