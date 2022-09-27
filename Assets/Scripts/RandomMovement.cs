using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 _direction = default;
    [SerializeField]
    private float _speed = 2.0f;
    [SerializeField]
    private float _orthoSpeed = 2.0f;
    [SerializeField]
    private float _amplitude = 2.0f;

    private Vector2 _orthogonal;
    private float _curOrthoTime;

    public SpawnerInfo.EMode Mode = SpawnerInfo.EMode.Random;

    private void Start()
    {
        var player = UIManager.Instance.GetPlayer();

        var currentPosition = (Vector2)transform.position;
        var targetPosition = player.GetRandomPosOnScreen();

        _direction = targetPosition - currentPosition;
        _direction.Normalize();

        if(UnityEngine.Random.value < 0.5f)
        {
            _orthogonal = new Vector2(_direction.y, -_direction.x);
        }
        else
        {
            _orthogonal = new Vector2(-_direction.y, _direction.x);
        }
    }

    private void Update()
    {
        if(Mode == SpawnerInfo.EMode.Random)
        {
            _curOrthoTime += Time.deltaTime;

            float sin = Mathf.Sin(_curOrthoTime) * _amplitude;

            var pos = (Vector2)transform.position;
            pos += (_direction * _speed + _orthogonal * sin * _orthoSpeed) * Time.deltaTime;
            transform.position = pos;
        }
        else if(Mode == SpawnerInfo.EMode.Horizontal)
        {
            //TODO:
        }

    }

}
