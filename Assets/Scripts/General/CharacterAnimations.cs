using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;

    [SerializeField] private Transform parentTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // public void PlayAnimation(Vector2 movementInput)
    // {
    //     animator.SetBool("Running", movementInput.magnitude > 0);
    // }
}
