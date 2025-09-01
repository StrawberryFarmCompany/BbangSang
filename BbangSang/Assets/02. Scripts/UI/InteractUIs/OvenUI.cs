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

    [Header("Oven Image Settings")]
    [SerializeField] Sprite ovenOffSprite;
    [SerializeField] Sprite ovenOnSprite;

    private Oven oven;

    public void Init(Oven oven)
    {
        this.oven = oven;
        selectDoughUI.Init(this.oven);
    }

    public void OnActiveButton()
    {
        if(oven.IsActive)
        {
            // 오븐이 켜져있을 때
            // 빵이 구워지는 중이면 
                // 애초에 안에 빵이 아무것도 없으면 바로 꺼지게
                // 빵이 있으면 그거 폐기할 건지 물어보기
            // 빵이 다 구워졌으면
                // 구워진 빵을 Bread Manager에 Bread에 추가하기
            OvenOff();
        }
        else
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
}
