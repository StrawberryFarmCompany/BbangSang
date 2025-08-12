using System;
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
            string json = runtimeActions.ToJson();
            PlayerPrefs.SetString(PlayerPrefsKey.InputSettingKey, json);
            PlayerPrefs.Save();
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

        action.Disable();

        // 이전 바인딩 저장 (override가 있으면 그걸, 없으면 원래 경로)
        string previousOverride = !string.IsNullOrEmpty(action.bindings[bindingIndex].overridePath)
            ? action.bindings[bindingIndex].overridePath
            : action.bindings[bindingIndex].path;

        currentRebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse")  // 마우스 입력 제외
            .OnCancel(operation =>
            {
                // 취소 시 이전 바인딩으로 복원
                action.ApplyBindingOverride(bindingIndex, previousOverride);

                action.Enable();
                operation.Dispose();
                currentRebindOperation = null;

                SaveBindings();
                onComplete?.Invoke();
            })
            .OnComplete(operation =>
            {
                // ESC 키 입력 시에도 이전 바인딩으로 복원 (추가 옵션)
                if (operation.selectedControl != null && operation.selectedControl.name.ToLower() == "escape")
                {
                    action.ApplyBindingOverride(bindingIndex, previousOverride);
                    Debug.Log("ESC 눌러서 리바인딩 취소됨.");
                }

                action.Enable();
                operation.Dispose();
                currentRebindOperation = null;

                SaveBindings();
                onComplete?.Invoke();
            });

        currentRebindOperation.Start();
    }
}
