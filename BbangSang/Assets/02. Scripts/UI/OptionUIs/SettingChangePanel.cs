using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingChangePanel : MonoBehaviour
{
    public TextMeshProUGUI currentSettingText;

    public void ChangeSettingText(string action, string key)
    {
        currentSettingText.text = action + " : " + key;
    }
   
}
