using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : BaseInteractable
{
    public GameObject craftingUI;
    private bool isUIOpen = false;

    public override void Interact()
    {
        craftingUI.SetActive(true);
        isUIOpen = true;
        Debug.Log("제작테이블 열림");
    }

    public void CloseUI()
    {
        craftingUI.SetActive(false);
        isUIOpen = false;
        Debug.Log("제작테이블 닫힘");
    }
}
