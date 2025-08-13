using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum OptionState
{
    Input,
    Sound
}

public class OptionUI : MonoBehaviour
{
    public OptionState state;
    public InputOptionUI inputUi;
    public SoundOptionUI soundUi;

    public Button inputButton;
    public Button soundButton;
    public Button restoreButton;

    private void Awake()
    {
        state = OptionState.Input;
    }

    private void OnEnable()
    {
        inputButton.onClick.RemoveAllListeners();
        soundButton.onClick.RemoveAllListeners();
        restoreButton.onClick.RemoveAllListeners();

        inputButton.onClick.AddListener(OnClickInput);
        soundButton.onClick.AddListener(OnClickSound);
        restoreButton.onClick.AddListener(OnClickRestore);
    }
    void Start()
    {
        inputUi.Init(this);
        soundUi.Init(this);
        SettingUI();
    }

    private void OnDisable()
    {
        inputButton.onClick.RemoveAllListeners();
        soundButton.onClick.RemoveAllListeners();
    }

    void SettingUI()
    {
        inputUi.SetUI(state);
        soundUi.SetUI(state);
    }

    public void OnClickInput()
    {
        state = OptionState.Input;
        SettingUI();
    }

    public void OnClickSound()
    {
        state = OptionState.Sound;
        SettingUI();
    }

    private void OnClickRestore()
    {
        InputManager.Instance.ResetToDefault();
        inputUi.InputPrefabSettings();
    }
}
