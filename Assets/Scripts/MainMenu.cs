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
    private Player _player;
    private Health _health;
    private Battlefield _battlefield;

    private void Awake()
    {
        _continue.onClick.AddListener(Continue);
        _newGame.onClick.AddListener(NewGame);
        _control.onClick.AddListener(Control);
        _exit.onClick.AddListener(Exit);
        _controlText = _control.GetComponentInChildren<TMP_Text>();
        _player = FindObjectOfType<Player>();
        _health = _player.GetComponent<Health>();
        _health.GameOver += GameOver;
        _battlefield = FindObjectOfType<Battlefield>();
    }

    private void Continue()
    {
        Time.timeScale = 1;
        Cursor.visible = !_controlKeyboard;
        _player.enabled = true;
        _player.ControlKeyboard = _controlKeyboard;
        _mainMenu.gameObject.SetActive(false);
    }

    private void NewGame()
    {
        Continue();
        _readyToContinue = true;
        _player.NewGame();
        _health.NewGame();
        _battlefield.NewGame();
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
        if (!_gameOver)
        {
            _mainMenu.gameObject.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
            _player.enabled = false;
            _continue.interactable = _readyToContinue;
            ControlButtonText();
        }
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
