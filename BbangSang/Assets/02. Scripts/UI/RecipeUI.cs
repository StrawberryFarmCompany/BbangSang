using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [SerializeField] private Button whiteBread;
    [SerializeField] private Button soboroBread;
    [SerializeField] private Button exit;
    public GameObject recipeInfoPanel;


    private void Start()
    {
        //onClick에 추가
        whiteBread.onClick.AddListener(WhiteBreadButton);
        soboroBread.onClick.AddListener(SoboroBreadButton);
        exit.onClick.AddListener(ExitButton);
    }

    // 식빵 정보
    public void WhiteBreadButton()
    {
        recipeInfoPanel.SetActive(true);
    }

    // 소보로 빵 정보
    public void SoboroBreadButton()
    {
        recipeInfoPanel.SetActive(true);
    }

    // 패널 닫기
    public void ExitButton()
    {
        gameObject.SetActive(false);
    }
}
