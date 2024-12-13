using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MusicType
{
    MainTheme,
    GameOver,
    Victory,
    Game,
    ShopMusic,
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class MusicManager : MonoBehaviour
{
    [SerializeField] private MusicList[] _musicList;
    private static MusicManager _instance;
    private AudioSource _audioSource;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(MusicType music, float volume = 1)
    {
        AudioClip[] clips = _instance._musicList[(int)music].Musics;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        _instance._audioSource.PlayOneShot(randomClip, volume);
    }

#if UNITY_EDITOR

    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(MusicType));
        Array.Resize(ref _musicList, names.Length);
        for (int i = 0; i < _musicList.Length; i++)
        {
            _musicList[i].Name = names[i];
        }
    }
#endif

    [Serializable]
    public struct MusicList
    {
        public AudioClip[] Musics
        {
            get => _musics;
        }

        [HideInInspector] public string Name;
        [SerializeField] private AudioClip[] _musics;
    }
}