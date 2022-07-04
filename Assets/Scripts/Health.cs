using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(SpriteRenderer))]
public class Health : MonoBehaviour
{
    [SerializeField] private int _startHealth;
    [SerializeField] private int _invulnerabilityTime = 3;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _gameOverText;
    private int _health;
    private bool _invulnerability;
    private SpriteRenderer _sprite;
    public event UnityAction GameOver;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void NewGame()
    {
        gameObject.SetActive(true);
        _health = _startHealth;
        _healthText.text = _health.ToString();
        StartCoroutine(Invulnerability());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_invulnerability) TakeDamage();
    }

    private IEnumerator Invulnerability()
    {
        _invulnerability = true;
        for (int i = 0; i < 2 * _invulnerabilityTime; i++)
        {
            yield return StartCoroutine(ColorChange(new Color(1, 1, 1, 0)));
            yield return StartCoroutine(ColorChange(new Color(1, 1, 1, 1)));
        }
        _invulnerability = false;
    }

    private IEnumerator ColorChange(Color newColor)
    {
        var oldColor = _sprite.color;
        var time = 0.5f;
        while (time > 0)
        {
            _sprite.color = Color.Lerp(newColor, oldColor, time / 0.25f);
            time -= Time.deltaTime;
            yield return null;
        }
        _sprite.color = newColor;
    }
    private void TakeDamage()
    {
        _health--;
        _healthText.text = _health.ToString();
        StartCoroutine(Invulnerability());
        if (_health == 0) Dead();
    }

    private void Dead()
    {
        _gameOverText.gameObject.SetActive(true);
        gameObject.SetActive(false);
        Invoke(nameof(ReloadGame), 5);
        GameOver?.Invoke();
    }
    private void ReloadGame()
    {
        SceneManager.LoadScene(0);
    }
}
