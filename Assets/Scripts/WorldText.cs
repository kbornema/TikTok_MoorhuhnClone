using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldText : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _text = default;
    [SerializeField]
    private CanvasGroup _canvasGroup = default;
    [SerializeField]
    private float _fadeTime = 5.0f;

    private float _curFadeTime = 0.0f;

    public void SetText(string s)
    {
        _text.text = s;
    }

    private void Update()
    {
        _curFadeTime += Time.deltaTime;

        float t = _curFadeTime / _fadeTime;
        _canvasGroup.alpha = 1.0f - t;

        if(_curFadeTime >= _fadeTime)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
