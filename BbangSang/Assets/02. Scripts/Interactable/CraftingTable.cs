using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : BaseInteractable
{
    public GameObject craftingUI;

    public override void Interact()
    {
        craftingUI.SetActive(true);
    }
}
