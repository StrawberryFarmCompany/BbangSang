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
        // 기존 UI 제거
        for (int i = contentParent.childCount - 1; i >= 0; i--)
            Destroy(contentParent.GetChild(i).gameObject);

        foreach (var action in InputManager.Instance.runtimeActions.FindActionMap("Player").actions)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                var binding = action.bindings[i];

                // Composite 루트는 버튼 안 만듦
                if (binding.isComposite)
                    continue;

                var optionItem = Instantiate(settingPrefab, contentParent);
                var optionItemContent = optionItem.GetComponent<KeySettingContent>();

                // 이름 표시 (Composite 파트면 파트 이름 아니면 액션 이름)
                optionItemContent.actionText.text = binding.isPartOfComposite ? binding.name : action.name;

                // 현재 바인딩 키
                string curBindingKeyText = action.GetBindingDisplayString(i);
                optionItemContent.keyText.text = curBindingKeyText == InputActionKey.anyKey ? string.Empty : curBindingKeyText.ToUpper();

                optionItemContent.Init(action, i, settingChangePanel);
            }
        }
    }


}
