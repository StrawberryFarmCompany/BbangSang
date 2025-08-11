using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputOptionUI : BaseOptionUI
{
    public InputActionAsset inputAction;
    public GameObject settingPrefab;
    public Transform contentParent;

    private void Awake()
    {
        State = OptionState.Input;
    }

    private void Start()
    {
        if(contentParent.childCount != 0)
        {
            for (int i = 0; i < contentParent.childCount; i++)
            {
                Destroy(contentParent.GetChild(i));
                i--;
            }
        }

        foreach (var action in inputAction.FindActionMap("Player").actions)
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
                }
            }
            else
            {
                var optionItem = Instantiate(settingPrefab, contentParent);
                var optionItemContent = optionItem.GetComponent<KeySettingContent>();
                optionItemContent.actionText.text = action.name;
                optionItemContent.keyText.text = action.bindings[0].ToDisplayString();
            }
        }
    }

}
