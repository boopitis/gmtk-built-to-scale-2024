using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunSound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource is null) Debug.LogError("AudioSource is null!");
    }

    private void Start()
    {
        PlayerGun.Instance.OnAttack += PlayerGun_OnAttack;
    }

    private void PlayerGun_OnAttack(object sender, PlayerGun.OnAttackEventArgs e)
    {
        audioSource.PlayOneShot(e.FiredNoteSO.audioClip);
    }
}