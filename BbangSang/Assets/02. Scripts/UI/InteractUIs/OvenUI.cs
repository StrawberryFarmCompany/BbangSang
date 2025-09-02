using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OvenUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Image ovenImage;
    [SerializeField] Button activeButton;
    [SerializeField] TextMeshProUGUI activeButtonText;
    [SerializeField] SelectDoughUI selectDoughUI;
    [SerializeField] OvenWarningUI warningUI;

    [Header("Oven Image Settings")]
    [SerializeField] Sprite ovenOffSprite;
    [SerializeField] Sprite ovenOnSprite;

    private Oven oven;

    public void Init(Oven oven)
    {
        this.oven = oven;
        selectDoughUI.Init(this.oven);
        warningUI.Init(this.oven);
    }

    private void OnEnable()
    {
        if (oven.IsActive)
        {
            activeButtonText.text = "오븐 끄기";
        }
        
        else if (oven.IsBakeDone)
        {
            activeButtonText.text = "빵 꺼내기";
        }
        else
        {
            activeButtonText.text = "빵 굽기";
        }
    }
    

    public void OnActiveButton()
    {
        if(oven.IsActive)
        {
            // 폐기할건지 물어보기
            warningUI.gameObject.SetActive(true);
        }
        else if (oven.IsBakeDone) // 빵 다 구워졌으면
        {
            oven.GetBakedBread();
            Debug.Log("빵 꺼냄");
            oven.IsBakeDone = false;
            gameObject.SetActive(false);
        }
        
        else // 오븐 꺼져있고 빵이 다 구워진것도 아님
        {
            // 오븐이 꺼져있는데 누르면 빵 선택 창 띄우기
            selectDoughUI.RefreshUI();
            selectDoughUI.gameObject.SetActive(true);
            // OvenOn();
        }
    }

    public void OnExitButton()
    {
        gameObject.SetActive(false);
    }

    public void OvenOff()
    {
        oven.IsActive = false;
        ovenImage.sprite = ovenOffSprite;
        activeButtonText.text = "빵 굽기";
        oven.OvenImageSetting(ovenOffSprite);
    }

    public void OvenOn()
    {
        oven.IsActive = true;
        ovenImage.sprite = ovenOnSprite;
        activeButtonText.text = "오븐 끄기";
        oven.OvenImageSetting(ovenOnSprite);
    }

    public void CloseAllUI()
    {
        selectDoughUI.gameObject.SetActive(false);
        warningUI.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
