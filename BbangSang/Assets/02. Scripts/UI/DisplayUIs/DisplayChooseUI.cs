using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayChooseUI : MonoBehaviour
{
    public Button exit;
    
    public Image icon;
    public TextMeshProUGUI breadName;
    public TextMeshProUGUI breadCount;
    public TMP_InputField inputBreadCount;
    
    private DisplayStandUI DisplayStandUI;
    
    private int currentBreadId;
    private int currentBreadCount;
    private int selectBreadCount = 0;
    
    public void Init(DisplayStandUI displayStandUI)
    {
        this.DisplayStandUI = displayStandUI;
    }

    private void Start()
    {
        exit.onClick.AddListener(ExitButton);
    }
    
    public void Setting(int recipeId, int count) //가지고 있는 빵 정보 UI에 표시
    {
        icon.sprite = BreadManager.Instance.GetRecipe(recipeId).GetIcon();
        breadName.text = BreadManager.Instance.GetRecipe(recipeId).Name;
        breadCount.text = count.ToString();
        currentBreadId = recipeId;
        currentBreadCount = count;
    }
    
    public void ValidateCountText(string text) //inputField에서 숫자 입력 시 호출
    {
        if (text == String.Empty || text.Length == 0) return;
        if (int.TryParse(text, out int result)) //숫자인 경우
        {
            inputBreadCount.text = Mathf.Min(result, currentBreadCount).ToString();
            selectBreadCount = int.Parse(inputBreadCount.text); //실제 선택 수량 저장
        }
        else //필드 초기화
        {
            inputBreadCount.text = "0";
        }
    }
    
    public void SelectDisplay() //진열하기 눌렀을 때
    {
        if(selectBreadCount <= 0) return; //개수 0이면 무시

        bool breadDisplay = BreadManager.Instance.UseBread(currentBreadId, selectBreadCount);
        if (!breadDisplay) //전시할 빵이 없으면
        {
            Debug.LogWarning("소유한 빵이 부족합니다");
        }
        // 진열된 빵 1개 이상 있으면 이미지 변경하기

        currentBreadCount -= selectBreadCount; //빵 보유개수 차감
        Setting(currentBreadId, currentBreadCount);
        inputBreadCount.text = "0";
        selectBreadCount = 0;
        
        gameObject.SetActive(false);
    }

    public void ExitButton()
    {
        gameObject.SetActive(false);
    }
}
