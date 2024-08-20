
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunAnimations : MonoBehaviour
{
    public static PlayerGunAnimations Instance { get; private set; }

    [SerializeField] private Animator animator;

    private void Awake()
    {
        Instance = this;
    }

    public void BasicAttackAnimation()
    {
        animator.SetTrigger("Basic Attack");
    }

    public void SpecialAttackAnimation(string ScaleName)
    {
        Debug.Log(ScaleName + " Special");
        animator.SetTrigger(ScaleName + " Special");
    }
}
