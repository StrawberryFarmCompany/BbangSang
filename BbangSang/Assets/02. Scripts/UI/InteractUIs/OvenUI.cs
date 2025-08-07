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

    [Header("Oven Image Settings")]
    [SerializeField] Sprite ovenOffSprite;
    [SerializeField] Sprite ovenOnSprite;

    private Oven oven;
    bool isActive = false;

    public void Init(Oven oven)
    {
        this.oven = oven;
    }

    public void OnActiveButton()
    {
        if(isActive)
        {
            OvenOff();
        }
        else
        {
            OvenOn();
        }
    }

    public void OnExitButton()
    {
        gameObject.SetActive(false);
    }

    public void OvenOff()
    {
        isActive = false;
        ovenImage.sprite = ovenOffSprite;
        activeButtonText.text = "»§ ±Á±â";
        oven.OvenImageSetting(ovenOffSprite);
    }

    public void OvenOn()
    {
        isActive = true;
        ovenImage.sprite = ovenOnSprite;
        activeButtonText.text = "¿Àºì ²ô±â";
        oven.OvenImageSetting(ovenOnSprite);
    }
}
