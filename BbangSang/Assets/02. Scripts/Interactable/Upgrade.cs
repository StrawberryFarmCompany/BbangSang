using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : BaseInteractable
{
    [SerializeField] private GameObject upgradeUI;
    public override void Interact()
    {
        if (upgradeUI != null)
        {
            upgradeUI.SetActive(true);
            Debug.Log("������ ���̺� ����");
        }
    }
}
