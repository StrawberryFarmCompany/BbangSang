using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStandUI : MonoBehaviour
{
    public GameObject displayStandUI;
    public Button exit;

    private void Start()
    {
        exit.onClick.AddListener(ExitButton);
    }

    public void ExitButton()
    {
        gameObject.SetActive(false);
    }
}
