using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Shoot,
    ShootFail,
    Explosion,
    BoatHit,
    GoldIn,
    GoldOut,
    Sinking,
    Splash,
    WaterHit,
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _soundList;
    private static SoundManager _instance;
    private AudioSource _audioSource;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        _instance._audioSource.PlayOneShot(_instance._soundList[(int)sound], volume);
    }
}