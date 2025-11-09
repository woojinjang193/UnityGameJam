using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private string[] _lines;
    [SerializeField] private GameObject _chatBox;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _SkipButton;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _typeSpeed = 0.05f;
    [SerializeField] private GameObject _fadout;

    [SerializeField] private GameObject _ufo;
    [SerializeField] private GameObject _transform;

    private bool _isLineDone = false;
    private int _index = 0;
    private bool _isTyping = false;

    private Coroutine _co;

    private void Awake()
    {
        _nextButton.onClick.AddListener(OnClickNext);
        _SkipButton.onClick.AddListener(OnClickSkip);
    }
    public void StartTpying()
    {
        _chatBox.SetActive(true);
        ShowNewLine();
    }

    private void OnClickNext()
    {
        if (!_isLineDone) return;

        _isLineDone = false;
        UpdateButtons(false);
        ShowNewLine();
    }
    private void OnClickSkip()
    {
        if (_isTyping)
        {
            if (_co != null)
            {
                StopCoroutine(_co);
                _text.text = _lines[_index];
                _isTyping = false;
                FinishTyping();
            }
        }
    }

    private void ShowNewLine()
    {
        if (_index >= _lines.Length)
        {
            _ufo.SetActive(false);
            _chatBox.SetActive(false);
            _fadout.SetActive(true);
            return;
        }

        _co = StartCoroutine(TypeLine(_lines[_index]));
    }
    private IEnumerator TypeLine(string line)
    {
        _isTyping = true;
        UpdateButtons(false);
        _text.text = "";

        if (_index == 2)
        {
            _text.color = Color.yellowGreen;
            _ufo.SetActive(true);
        }
        if (_index == 4)
        {
            _transform.SetActive(true);
        }
        foreach (char c in line)
        {
            _text.text += c;
            yield return new WaitForSeconds(_typeSpeed);
        }

        FinishTyping();
    }
    private void FinishTyping()
    {
        _isTyping = false;
        _isLineDone = true;
        _index++;
        UpdateButtons(true);
    }
    private void UpdateButtons(bool lineDone)
    {
        _SkipButton.gameObject.SetActive(!lineDone);
        _nextButton.gameObject.SetActive(lineDone);
    }
}
