using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunAnimations : MonoBehaviour
{
    public static PlayerGunAnimations Instance { get; private set; }

    [SerializeField] private Animator animator;
    private static readonly int RED_SPECIAL = Animator.StringToHash("Red Special");
    private static readonly int PURPLE_SPECIAL = Animator.StringToHash("Purple Special");
    private static readonly int BASIC_ATTACK = Animator.StringToHash("Basic Attack");
    private static readonly int YELLOW_SPECIAL = Animator.StringToHash("Yellow Special");

    public enum GunAnimation
    {
        BasicAttack,
        PurpleSpecial,
        RedSpecial,
        YellowSpecial
    }

    private void Awake()
    {
        Instance = this;
    }

    public void BasicAttackAnimation()
    {
        animator.SetTrigger("Basic Attack");
    }

    public void SpecialAttackAnimation(ScaleSO scaleSO)
    {
        switch (scaleSO.gunAnimation)
        {
            default:
            case GunAnimation.BasicAttack:
                animator.SetTrigger(BASIC_ATTACK);
                break;
            case GunAnimation.PurpleSpecial:
                animator.SetTrigger(PURPLE_SPECIAL);
                break;
            case GunAnimation.RedSpecial:
                animator.SetTrigger(RED_SPECIAL);
                break;
            case GunAnimation.YellowSpecial:
                animator.SetTrigger(YELLOW_SPECIAL);
                break;
        }
    }
}
