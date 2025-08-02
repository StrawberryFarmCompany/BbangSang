using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeTableUI : MonoBehaviour
{
    [SerializeField] private Button noramlBread;
    [SerializeField] private Button creamBread;
    [SerializeField] private Button exit;


    // 기본 빵 정보
    public void normalBreadButton()
    {
        // 버튼 누르면 정보 표시
    }

    // 크림 빵 정보
    public void creamBreadButton()
    {
        // 버튼 누르면 정보 표시
    }

    // 패널 닫기
    public void ExitButton()
    {
        gameObject.SetActive(false);
    }
}
