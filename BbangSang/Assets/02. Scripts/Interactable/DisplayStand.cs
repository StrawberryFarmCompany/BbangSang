using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayStand : BaseInteractable
{
    [SerializeField] private GameObject displayStandUI;
    public override void Interact()
    {
        if (displayStandUI != null)
        {
            displayStandUI.SetActive(true);
            Debug.Log("매대 보기");
        }
    }
}
