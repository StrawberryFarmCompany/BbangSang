using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputOptionUI : BaseOptionUI
{
    public GameObject settingPrefab;
    public Transform contentParent;

    public SettingChangePanel settingChangePanel;

    private void Awake()
    {
        State = OptionState.Input;
    }

    private void Start()
    {
        InputPrefabSettings();
    }

    public void InputPrefabSettings()
    {
        for (int i = contentParent.childCount - 1; i >= 0; i--)
            Destroy(contentParent.GetChild(i).gameObject);

        foreach (var action in InputManager.Instance.runtimeActions.FindActionMap("Player").actions)
        {
            if (action.bindings.Count > 1 && action.bindings[0].isComposite)
            {
                // Composite (¿¹: 2D Vector)
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    if (action.bindings[i].isComposite) continue;
                    var optionItem = Instantiate(settingPrefab, contentParent);
                    var optionItemContent = optionItem.GetComponent<KeySettingContent>();
                    optionItemContent.actionText.text = action.bindings[i].name;
                    optionItemContent.keyText.text = action.GetBindingDisplayString(i);
                    optionItemContent.Init(action, i, settingChangePanel);
                }
            }
            else
            {
                var optionItem = Instantiate(settingPrefab, contentParent);
                var optionItemContent = optionItem.GetComponent<KeySettingContent>();
                optionItemContent.actionText.text = action.name;
                optionItemContent.keyText.text = action.bindings[0].ToDisplayString();
                optionItemContent.Init(action, 0, settingChangePanel);
            }
        }
    }

    
}
