using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class TitleUI : MonoBehaviour
{
    [Header("Main Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button OptionButton;
    [SerializeField] private Button quitButton;

    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject namePanel;
    [SerializeField] private GameObject optionPanel;

    [Header("Name Panel")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button confirmNameButton;
    [SerializeField] private Button cancelNameButton;

    [Header("Next Scene")]
    [SerializeField] private string nextSceneName = "TitleTestScene";

    void Awake()
    {
        newGameButton.onClick.AddListener(OnClickNewGame);
        loadGameButton.onClick.AddListener(OnClickLoad);
        OptionButton.onClick.AddListener(OnClickOption);
        quitButton.onClick.AddListener(OnClickQuit);

        confirmNameButton.onClick.AddListener(OnConfirmName);
        cancelNameButton.onClick.AddListener(OnClickCancelName);

        ShowNamePanel(false);
        ShowOptionPanel(false);
    }

    void ShowNamePanel(bool show)
    {
        mainPanel.SetActive(!show);
        namePanel.SetActive(show);

        if (show)
        {
            nameInput.text = "";
            nameInput.Select();
            nameInput.ActivateInputField();
        }
    }

    void ShowOptionPanel(bool show)
    {
        mainPanel.SetActive(!show);
        optionPanel.SetActive(show);
    }

    void OnClickNewGame()
    {
        ShowNamePanel(true);
    }

    void OnClickOption()
    {
        ShowOptionPanel(true);
    }

    public void OnClickCloseOption()
    {
        ShowOptionPanel(false);
    }

    void OnClickCancelName()
    {
        ShowNamePanel(false);
    }

    void OnConfirmName()
    {
        string nameStr = nameInput.text?.Trim();

        if (string.IsNullOrEmpty(nameStr))
        {
            Debug.LogWarning("이름입력");
            return;
        }

        GameManager.Instance.IsNewGame = true;
        GameManager.Instance.PendingNewPlayerName = nameStr;

        SceneManager.LoadScene(nextSceneName);
    }

    void OnClickLoad()
    {
        if (File.Exists(Player.SaveFilePath))
        {
            GameManager.Instance.IsNewGame = false;
            GameManager.Instance.PendingNewPlayerName = null;
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            ShowNamePanel(true);
        }
    }

    void OnClickQuit()
    {
        Application.Quit();
    }
}
