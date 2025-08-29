using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStandUI : MonoBehaviour
{
    public GameObject displayStandUI;
    public GameObject displayChoose;
    public Button exit;
    public List<(int recipeId, int count)> Buffer = new();
    public DisContent selectedContent; //default null. 선택한 칸 확인용

    private void Start()
    {
        exit.onClick.AddListener(ExitButton);
    }

    public void ExitButton()
    {
        gameObject.SetActive(false);
    }
    
    public void DisplayChooseButton()
    {
        displayChoose.gameObject.SetActive(true);
    }

    public void DisplayBread() //빵 전시
    {
        BreadManager.Instance.GetAllDough(Buffer); //(함수 호출할 때마다)현재 가지고 있는 빵 정보 가져오기
        //인벤 목록에 뜨도록 하기
    }
}
