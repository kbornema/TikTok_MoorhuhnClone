using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private Canvas _canvas = default;
    [SerializeField]
    private WorldText _textPrefab = default;
    [SerializeField]
    private Player _player = default;
    public Player GetPlayer() => _player;

    [SerializeField]
    private TMPro.TextMeshProUGUI _factionText = default;
    [SerializeField]
    private TMPro.TextMeshProUGUI _scoreText = default;

    [SerializeField]
    private Image _comboSlider = default;

    [SerializeField]
    private int _score = 0;
    [SerializeField]
    private int _negativeScore;
    [SerializeField]
    private int _positiveScore;

    [SerializeField]
    private float _comboTime = 1.0f;
    private Player.Tool _comboTool = Player.Tool.Negative;
    private Faction _comboFaction = default;

    private float _curComboTime = 0.0f;

    private int _shootAmmoCount;
    private int _helpAmmoCount;

    public List<Image> ShootAmmo = default;
    public List<Image> HelpAmmo = default;

    public bool IsComboActive => _curComboTime > 0.0f;
    private int _comboCount = 0;

    private void Awake()
    {
        Instance = this;
        _shootAmmoCount = ShootAmmo.Count;
        _helpAmmoCount = HelpAmmo.Count;
    }

    private void Start()
    {
        _player.FactionChangedEvent.AddListener(FactionChangedListener);
    }

    private void Update()
    {
        if (_curComboTime > 0.0f)
        {
            _curComboTime -= Time.deltaTime;

            float t = _curComboTime / _comboTime;
            _comboSlider.fillAmount = t;

            if (_curComboTime <= 0.0f)
            {
                _curComboTime = 0.0f;
                StopCombo();
            }
        }
    }

    public void StartCombo()
    {
        _curComboTime = _comboTime;
        _comboSlider.fillAmount = 1.0f;
        Debug.Log("Start Combo");
    }

    internal void AddOppositeAmmo(Player.Tool tool, int count)
    {
        if(tool == Player.Tool.Negative)
        {
            AddAmmo(Player.Tool.Positive, count);
        }
        else if(tool == Player.Tool.Positive)
        {
            AddAmmo(Player.Tool.Negative, count);
        }
    }

    public void ConsumeShootAmmo()
    {
        if(_shootAmmoCount > 0)
        {
            _shootAmmoCount--;
            UpdateImages(ShootAmmo, _shootAmmoCount);
        }
    }

    public void AddShootAmmo(int delta)
    {
        _shootAmmoCount += delta;

        if(_shootAmmoCount > ShootAmmo.Count)
        {
            _shootAmmoCount = ShootAmmo.Count;
        }

        UpdateImages(ShootAmmo, _shootAmmoCount);
    }

    public void AddHelpAmmo(int delta)
    {
        _helpAmmoCount += delta;

        if (_helpAmmoCount > HelpAmmo.Count)
        {
            _helpAmmoCount = HelpAmmo.Count;
        }

        UpdateImages(HelpAmmo, _helpAmmoCount);
    }

    public void AddAmmo(Player.Tool tool, int count)
    {
        if(tool == Player.Tool.Negative)
        {
            AddShootAmmo(count);
        }
        else if(tool == Player.Tool.Positive)
        {
            AddHelpAmmo(count);
        }
    }

    public void ConsumeAmmo(Player.Tool tool)
    {
        if (tool == Player.Tool.Negative)
        {
            ConsumeShootAmmo();
        }
        else if (tool == Player.Tool.Positive)
        {
            ConsumeHelpAmmo();
        }
    }

    public bool HasAmmo(Player.Tool tool)
    {
        if(tool == Player.Tool.Negative)
        {
            return HasShootAmmo();
        }
        else if(tool == Player.Tool.Positive)
        {
            return HasHelpAmmo();
        }

        return true;
    }

    public void ConsumeHelpAmmo()
    {
        if (_helpAmmoCount > 0)
        {
            _helpAmmoCount--;
            UpdateImages(HelpAmmo, _helpAmmoCount);
        }
    }

    private void UpdateImages(List<Image> images, int count)
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < count; i++)
        {
            images[i].gameObject.SetActive(true);
        }
    }

    public bool HasShootAmmo()
    {
        return _shootAmmoCount > 0;
    }

    public bool HasHelpAmmo()
    {
        return _helpAmmoCount > 0;
    }

    private void FactionChangedListener(Player arg0)
    {
        _factionText.text = arg0.CurrentFaction.ToString();
    }

    public void AddScore(int score)
    {
        if (score != 0)
        {
            if (score < 0)
            {
                _negativeScore += Mathf.Abs(score);
            }
            else
            {
                if(IsComboActive)
                {

                }

                _positiveScore += score;
            }

            _score += score;
            _scoreText.text = _score.ToString();
            CreateText(score.ToString());
        }
    }

    public void CheckCombo(int points, Player.Tool tool, Faction faction)
    {
        if (IsComboActive)
        {
            if (_comboTool == tool && _comboFaction == faction)
            {
                StartCombo();
                _comboCount++;
            }
            else
            {
                StopCombo();
            }
        }
        else
        {
            StopCombo();
            StartCombo();
            _comboCount++;
            _comboFaction = faction;
            _comboTool = tool;
        }
    }

    private void StopCombo()
    {
        _comboCount = 0;
        _curComboTime = 0.0f;
        _comboSlider.fillAmount = 0.0f;
        Debug.Log("Stop Combo");
    }
    public void CreateText(string text)
    {
        var mousePos = _canvas.worldCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;
        CreateText(mousePos, text);
    }

    public void CreateText(Vector3 pos, string text)
    {
        var instance = GameObject.Instantiate(_textPrefab, pos, Quaternion.identity);
        instance.SetText(text);
    }
}
