using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private InputActionAsset defaultActions; 
    public InputActionAsset runtimeActions;

    private InputActionRebindingExtensions.RebindingOperation currentRebindOperation;
    private InputSystemUIInputModule uiModule;
    private void Awake()
    {
        if (runtimeActions == null &&  defaultActions != null)
        {
            runtimeActions = Instantiate(defaultActions);
            runtimeActions.name = defaultActions.name + "_Runtime";
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        if(!Application.isBatchMode && Application.isPlaying)
            DontDestroyOnLoad(gameObject);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UIModuleSettings();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // changed runtimeActions apply to EventSystem
    private void UIModuleSettings()
    {
        uiModule =  FindObjectOfType<InputSystemUIInputModule>();
        if (uiModule == null)
        {
            return;
        }
        uiModule.actionsAsset = runtimeActions;
        uiModule.move = InputActionReference.Create(runtimeActions.FindAction("UI/Move"));
        uiModule.submit = InputActionReference.Create(runtimeActions.FindAction("UI/Submit"));
        uiModule.cancel = InputActionReference.Create(runtimeActions.FindAction("UI/Cancel"));
        uiModule.point = InputActionReference.Create(runtimeActions.FindAction("UI/Point"));
        uiModule.leftClick = InputActionReference.Create(runtimeActions.FindAction("UI/Click"));
        uiModule.scrollWheel = InputActionReference.Create(runtimeActions.FindAction("UI/ScrollWheel"));
        uiModule.rightClick = InputActionReference.Create(runtimeActions.FindAction("UI/RightClick"));
        uiModule.middleClick = InputActionReference.Create(runtimeActions.FindAction("UI/MiddleClick"));
        
        Debug.Log("UI Module Settings Done.");
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
            string json = runtimeActions.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(PlayerPrefsKey.InputSettingKey, json);
            PlayerPrefs.Save();
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
        
        // Composite example : Move
        if (binding.isComposite)
        {
            return;
        }
        
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
                // if input key is backspace, no key override.
                if (Keyboard.current.backspaceKey.wasPressedThisFrame)
                {
                    action.RemoveBindingOverride(bindingIndex);
                    operation.Cancel();
                    SaveBindings();
                }
            })
            .OnCancel(operation =>
            {
                action.ApplyBindingOverride(bindingIndex, previousOverride);

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
                    }
                    else if (!IsBindingConflict(action, bindingIndex, newPath))
                    {
                        action.ApplyBindingOverride(bindingIndex, newPath);
                    }
                    else
                    {
                        action.ApplyBindingOverride(bindingIndex, previousOverride);
                    }

                    SaveBindings();
                }

                action.Enable();
                operation.Dispose();
                onComplete?.Invoke();
            });
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
