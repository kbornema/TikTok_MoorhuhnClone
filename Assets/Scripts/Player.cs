using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Tool
    {
        Positive,
        Negative,
        Neutral
    }

    public Camera Cam = default;
    public Vector2 MoveSpeed = default;

    public Transform LevelMin = default;
    public Transform LevelMax = default;

    public Transform CamMin = default;

    public Transform CamMax = default;

    public Transform Crosshair;

    public LayerMask _layerMask = default;

    public Faction CurrentFaction = Faction.Neutral;

    public List<AudioClip> ShootSfx = default;
    public List<AudioClip> HelpSfx = default;

    public GenericEvent<Player> FactionChangedEvent = new GenericEvent<Player>();

    public MusicComboService MusicComboService;

    private void Start()
    {
        MusicComboService = new MusicComboService(161);
    }

    private void Update()
    {
        var mousePos = Cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0.0f;

        MusicComboService.Update(Time.deltaTime);
        CheckMove(mousePos);
        CheckBounds();
        CheckMouse(mousePos, 0, Tool.Negative);
        CheckMouse(mousePos, 1, Tool.Positive);
        CheckMouse(mousePos, 2, Tool.Neutral);

        Crosshair.position = mousePos;
    }

    private void CheckMouse(Vector3 mousePos, int mouseButton, Tool tool)
    {
        if (Input.GetMouseButtonDown(mouseButton))
        {
            var ui = UIManager.Instance;

            if (!ui.HasAmmo(tool))
            {
                return;
            }

            ui.ConsumeAmmo(tool);

            if (mouseButton == 0)
            {
                SfxClip.Play(ShootSfx);
            }
            else
            {
                SfxClip.Play(HelpSfx);
            }

            var hit = Physics2D.Raycast(mousePos, Vector2.zero, 100.0f, _layerMask);

            if (hit)
            {
                var clicks = hit.collider.GetComponents<IClickCallback>();

                for (int i = 0; i < clicks.Length; i++)
                {
                    clicks[i].OnClick(this, tool);
                }
            }
        }
    }

    public Vector2 GetRandomPosOnScreen()
    {
        var min = LevelMin.position;
        var max = LevelMax.position;
        var x = UnityEngine.Random.Range(min.x, max.x);
        var y = UnityEngine.Random.Range(min.y, max.y);
        return new Vector2(x, y);
    }
    public void SetHostile(Faction faction)
    {
        CurrentFaction = GetAntiFaction(faction);
        FactionChangedEvent.Invoke(this);
    }

    private Faction GetAntiFaction(Faction faction)
    {
        switch (faction)
        {
            case Faction.A:
                return Faction.B;
            case Faction.B:
                return Faction.A;
            default:
                Debug.LogError("Neutral not allowed");
                return Faction.Neutral;
        }
    }

    public void SetFriend(Faction faction)
    {
        CurrentFaction = faction;
        FactionChangedEvent.Invoke(this);
    }

    private void CheckBounds()
    {
        var camTransform = Cam.transform;
        //float camWidth = Cam.orthographicSize * Cam.aspect;

        var camPos = camTransform.position;
        camPos.x = Mathf.Clamp(camPos.x, LevelMin.position.x, LevelMax.position.x);
        camPos.y = Mathf.Clamp(camPos.y, LevelMin.position.y, LevelMax.position.y);
        camTransform.position = camPos;
    }

    private void CheckMove(Vector3 mousePos)
    {
        if (mousePos.x < CamMin.position.x)
        {
            Move(-MoveSpeed.x, 0.0f);
        }

        if (mousePos.x > CamMax.position.x)
        {
            Move(MoveSpeed.x, 0.0f);
        }

        if (mousePos.y < CamMin.position.y)
        {
            Move(0.0f, -MoveSpeed.y);
        }

        if (mousePos.y > CamMax.position.y)
        {
            Move(0.0f, MoveSpeed.y);
        }
    }

    private void Move(float x, float y)
    {
        var camPos = Cam.transform.position;
        camPos += new Vector3(x, y, 0.0f) * Time.deltaTime;
        Cam.transform.position = camPos;
    }
}
