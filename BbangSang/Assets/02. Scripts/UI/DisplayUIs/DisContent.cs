using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisContent : MonoBehaviour, IPointerClickHandler //IPointerClickHandler : 월드 오브젝트는 무시하고 해당 슬롯 버튼만 감지함.
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI count;
    DisplayStandUI displayStandUI;

    void Start()
    {
        displayStandUI = GetComponentInParent<DisplayStandUI>();
    }
    
    //안에 값이 있을 때 이미지/개수 띄워줌

    public void OnPointerClick(PointerEventData eventData) //아이템 칸 클릭했을 때 정보 띄워줌
    {
        displayStandUI.selectedContent = this; //선택한 칸 넘겨줌
        displayStandUI.DisplayChooseButton();
    }
}
