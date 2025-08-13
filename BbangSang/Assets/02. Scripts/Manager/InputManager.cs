using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private InputActionAsset defaultActions; 
    public InputActionAsset runtimeActions;

    private InputActionRebindingExtensions.RebindingOperation currentRebindOperation;
    private void Awake()
    {
        runtimeActions = Instantiate(defaultActions);
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKey.InputSettingKey))
        {
            Debug.Log("얍");
            LoadBindings();
        }
        else
        {
            SaveBindings();
        }

        runtimeActions.Enable();
    }


    public void SaveBindings()
    {
        try
        {
            string json = runtimeActions.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(PlayerPrefsKey.InputSettingKey, json);
            PlayerPrefs.Save();
            Debug.Log("테스트 : " + PlayerPrefs.GetString(PlayerPrefsKey.InputSettingKey));
            Debug.Log("Input bindings saved to PlayerPrefs.");
        }
        catch (Exception e)
        {
            Debug.LogError($"SaveBindings failed: {e.Message}");
        }
    }

    public void LoadBindings()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKey.InputSettingKey))
        {
            string json = PlayerPrefs.GetString(PlayerPrefsKey.InputSettingKey);
            runtimeActions.LoadBindingOverridesFromJson(json);
            runtimeActions.Enable();
            Debug.Log("Input bindings loaded from PlayerPrefs.");
        }
        else
        {
            Debug.Log("No saved input bindings found in PlayerPrefs. Using defaults.");
        }
    }

    public void ResetToDefault()
    {
        runtimeActions.RemoveAllBindingOverrides();
        SaveBindings();
    }

    public void RebindKey(InputAction action, int bindingIndex, Action onComplete = null)
    {
        if (action == null || bindingIndex < 0 || bindingIndex >= action.bindings.Count)
            return;

        var binding = action.bindings[bindingIndex];

        // Composite 루트는 직접 바인딩할 수 없음
        if (binding.isComposite)
        {
            Debug.LogWarning($"'{binding.name}'은(는) Composite 루트 바인딩입니다. 개별 파트를 선택하세요.");
            return;
        }

        // 이전 바인딩 저장 (override 있으면 그걸, 없으면 원래 경로)
        string previousOverride = !string.IsNullOrEmpty(binding.overridePath)
            ? binding.overridePath
            : binding.path;

        action.Disable();

        currentRebindOperation = 
            action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse")
            .WithControlsExcluding("<Keyboard>/backspace")
            .OnPotentialMatch(operation =>
            {
                // Backspace 감지 시 할당 해제
                if (Keyboard.current.backspaceKey.wasPressedThisFrame)
                {
                    action.RemoveBindingOverride(bindingIndex);
                    Debug.Log("할당을 삭제함");
                    operation.Cancel();
                    SaveBindings();
                }
            })
            .OnCancel(operation =>
            {
                // 취소 시 복원
                action.ApplyBindingOverride(bindingIndex, previousOverride);

                Debug.Log($"리바인딩 취소: {binding.name} → {previousOverride}");

                action.Enable();
                SaveBindings();
                operation.Dispose();
                currentRebindOperation = null;
                onComplete?.Invoke();
            })
            .OnComplete(operation =>
            {
                if (operation.selectedControl != null)
                {
                    string controlName = operation.selectedControl.name.ToLower();
                    var newPath = operation.selectedControl.path;

                    if (controlName == "escape")
                    {
                        action.ApplyBindingOverride(bindingIndex, previousOverride);
                        Debug.Log($"ESC 입력 → '{binding.name}' 복원됨.");
                    }
                    else if (!IsBindingConflict(action, bindingIndex, newPath))
                    {
                        action.ApplyBindingOverride(bindingIndex, newPath);
                        Debug.Log($"바인딩 변경: {binding.name} → {newPath}");
                    }
                    else
                    {
                        action.ApplyBindingOverride(bindingIndex, previousOverride);
                        Debug.Log("동일한 키가 있음.");
                    }

                    SaveBindings();
                }

                action.Enable();
                operation.Dispose();
                onComplete?.Invoke();
            });

        Debug.Log($"리바인딩 시작: {action.name} ({binding.name})");

        currentRebindOperation.Start();
    }

    bool IsBindingConflict(InputAction targetAction, int targetBindingIndex, string newBindingPath)
    {
        if (string.IsNullOrEmpty(newBindingPath))
            return false;

        
        if (!newBindingPath.StartsWith("<") && newBindingPath.Contains("/Keyboard/"))
            newBindingPath = newBindingPath.Replace("/Keyboard/", "<Keyboard>/");
        

        var asset = targetAction.actionMap != null ? targetAction.actionMap.asset : null;

        if (asset == null) return false;

        var playerMap = asset.FindActionMap(InputActionKey.playerMap);

        foreach(var action in playerMap)
        {
            for (int i = 0; i < action.bindings.Count;i++)
            {
                var b = action.bindings[i];

                if (b.isComposite) continue;
                if (action == targetAction && i == targetBindingIndex) continue;
                if (string.IsNullOrEmpty(b.effectivePath)) continue;
                if (string.Equals(b.effectivePath, newBindingPath)) return true;
            }
        }

        
        return false;
    }
}
