using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerInfo : MonoBehaviour
{
    public enum EMode { Random, Horizontal }

    public EMode Mode = default;
    public Vector2 Direction = default;
    public BoxCollider2D Collider = default;
    public string Tag = default;
}
