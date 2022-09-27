using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxClip : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audio = default;

    public static void Play(List<AudioClip> clips, Vector3 pos)
    {
        if (clips != null && clips.Count > 0)
        {
            var clipPrefab = Resources.Load<SfxClip>("Audio");
            var instance = GameObject.Instantiate(clipPrefab, pos, Quaternion.identity);
            instance._audio.clip = clips[UnityEngine.Random.Range(0, clips.Count)];
            instance._audio.Play();
        }
    }

    public static void Play(List<AudioClip> clips)
    {
        Play(clips, Vector3.zero);
    }
}
