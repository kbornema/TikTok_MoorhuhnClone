using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private List<SpawnerInfo> _spawnInfo = default;
    [SerializeField]
    private float _cooldown = 5.0f;
    [SerializeField]
    private List<Entry> _spawnPrefabs = default;
    [SerializeField]
    private int _numSpawnsMin = 1;
    [SerializeField]
    private int _numSpawnsMax = 1;

    private float _curCd = 0.0f;

    private void OnValidate()
    {
        float totalWeight = 0.0f;

        for (int i = 0; i < _spawnPrefabs.Count; i++)
        {
            totalWeight += _spawnPrefabs[i].Weight;
        }

        for (int i = 0; i < _spawnPrefabs.Count; i++)
        {
            _spawnPrefabs[i].Chance = _spawnPrefabs[i].Weight / totalWeight;
        }

        for (int i = 0; i < _spawnPrefabs.Count; i++)
        {
            if(i > 0)
            {
                _spawnPrefabs[i].AccumChance = _spawnPrefabs[i - 1].AccumChance + _spawnPrefabs[i].Chance;
            }
            else
            {
                _spawnPrefabs[i].AccumChance = 0.0f;
            }
        }
    }

    private void Update()
    {
        _curCd -= Time.deltaTime;

        if(_curCd <= 0.0)
        {
            _curCd = _cooldown;

            int spawns = UnityEngine.Random.Range(_numSpawnsMin, _numSpawnsMax);

            for (int i = 0; i < spawns; i++)
            {
                SpawnPrefab();
            }
        }
    }

    private void SpawnPrefab()
    {
        var spawnInfo = _spawnInfo[UnityEngine.Random.Range(0, _spawnInfo.Count)];

        if(spawnInfo == null)
        {
            Debug.LogError("?!");
            return;
        }

        var box = spawnInfo.Collider;

        var pos = (Vector2)box.transform.position;

        var scale = box.transform.localScale;
        var rect = new Rect(pos + box.offset, box.size);

        float randX = UnityEngine.Random.Range(rect.xMin, rect.xMax) * scale.x - rect.width * scale.x * 0.5f;
        float randY = UnityEngine.Random.Range(rect.yMin, rect.yMax) * scale.y - rect.height * scale.y * 0.5f;

        Entry prefab = null;
        prefab = GetPrefabNext();

        //if(!prefab.HasTag(spawnInfo.Tag))
        //{
        //    SpawnPrefab();
        //    return;
        //}
        //else
        //{
        //}
        var instance = GameObject.Instantiate(prefab.Prefab, new Vector3(randX, randY, 0.0f), Quaternion.identity);
    }

    private Entry GetPrefabNext()
    {
        float chance = UnityEngine.Random.value;
        Entry currentEntry = null;
        for (int i = 0; i < _spawnPrefabs.Count; i++)
        {
            if(_spawnPrefabs[i].AccumChance <= chance)
            {
                currentEntry = _spawnPrefabs[i];
            }
        }
        return currentEntry;
    }

    [System.Serializable]
    public class Entry
    {
        public GameObject Prefab = default;
        public float Weight = 1.0f;
        public float Chance = default;
        public float AccumChance = default;

        public List<string> Tags = default;

        public bool HasTag(string tag)
        {
            return Tags.Contains(tag);
        }
    }
}
