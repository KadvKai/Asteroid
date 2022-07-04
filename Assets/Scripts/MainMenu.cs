using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Canvas _mainMenu;
    [SerializeField] private Button _continue;
    [SerializeField] private Button _newGame;
    [SerializeField] private Button _control;
    [SerializeField] private Button _exit;
    [SerializeField] private string _textControlKeyboard;
    [SerializeField] private string _textControlKeyboardMouse;
    private TMP_Text _controlText;
    private bool _gameOver;
    private bool _readyToContinue;
    private bool _controlKeyboard;

    private void Awake()
    {
        _continue.onClick.AddListener(Continue);
        _newGame.onClick.AddListener(NewGame);
        _control.onClick.AddListener(Control);
        _exit.onClick.AddListener(Exit);
        _controlText = _control.GetComponentInParent<TMP_Text>();
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
        _continue.interactable = _readyToContinue;
        ControlButtonText();
    }
    private void Continue()
    {
        Time.timeScale = 1;
        _mainMenu.gameObject.SetActive(false);
    }

    private void NewGame()
    {
        _readyToContinue = true;
    }

    private void Control()
    {
        _controlKeyboard = !_controlKeyboard;
        ControlButtonText();
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void Start()
    {
        ShowMainMenu();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ShowMainMenu();
    }
    private void ShowMainMenu()
    {
        if (!_gameOver) _mainMenu.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        _gameOver = true;
    }

    private void ControlButtonText()
    {
        _controlText.text = _controlKeyboard ? _textControlKeyboard : _textControlKeyboardMouse;
    }
}
