using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private Button exit;
    [SerializeField] private Button upgrade1;
    [SerializeField] private Button upgrade2;
    [SerializeField] private Button upgrade3;


    private void Start()
    {
        exit.onClick.AddListener(ExitButton);
        upgrade1.onClick.AddListener(UpgradeButton);
        upgrade2.onClick.AddListener(UpgradeButton);
        upgrade3.onClick.AddListener(UpgradeButton);
    }


    public void UpgradeButton()
    {
        Debug.Log("업그레이드");
    }

    public void ExitButton()
    {
        gameObject.SetActive(false);
    }
}
