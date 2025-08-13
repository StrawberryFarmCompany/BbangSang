using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeySettingContent : MonoBehaviour
{
    public TextMeshProUGUI actionText;
    public TextMeshProUGUI keyText;
    public Button changeButton;

    private InputOptionUI inputOptionUI;

    public InputAction targetAction;
    public int bindingIndex;

    SettingChangePanel settingChangePanel;

    private void Awake()
    {
        changeButton.onClick.RemoveAllListeners();
        changeButton.onClick.AddListener(OnChangeKeyClicked);
    }

    public void Init(InputAction action, int index, SettingChangePanel panel)
    {
        targetAction = action;
        bindingIndex = index;

        if(action.bindings.Count > 2)
        {
            actionText.text = action.bindings[index].name;
        }
        else
        {
            actionText.text = action.name;
        }

        // UI Ç¥½Ã
        settingChangePanel = panel;
        
        string readableText = InputControlPath.ToHumanReadableString(
            action.bindings[index].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );
        keyText.text = readableText == InputActionKey.anyKey ? string.Empty : readableText.ToUpper();
    }

    private void OnChangeKeyClicked()
    {
        settingChangePanel.gameObject.SetActive(true);
        settingChangePanel.ChangeSettingText(actionText.text, keyText.text);

        InputManager.Instance.RebindKey(targetAction, bindingIndex, () =>
        {
            string readableText = InputControlPath.ToHumanReadableString(
            targetAction.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
            );

            keyText.text = readableText == InputActionKey.anyKey ? string.Empty : readableText.ToUpper();
            settingChangePanel.gameObject.SetActive(false);
            InputManager.Instance.SaveBindings();
        });
    }
}
