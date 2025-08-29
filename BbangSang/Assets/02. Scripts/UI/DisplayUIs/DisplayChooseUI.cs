using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayChooseUI : MonoBehaviour
{
    public GameObject displayChooseUI;
    public Button exit;
    public TextMeshProUGUI menuText;

    private void Start()
    {
        exit.onClick.AddListener(ExitButton);
    }

    public void ExitButton()
    {
        gameObject.SetActive(false);
    }
}
