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
        Debug.Log("제작테이블 오픈");
    }
}
