using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    private Animator animator;

    public Transform parentTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void RotateToPointer(Vector2 lookDirection)
    {
        Vector3 scale = parentTransform.localScale;
        if (lookDirection.x > 0)
        {
            scale.x = 1;
        }
        else if (lookDirection.x < 0)
        {
            scale.x = -1;
        }
        parentTransform.localScale = scale;
    }

    // public void PlayAnimation(Vector2 movementInput)
    // {
    //     animator.SetBool("Running", movementInput.magnitude > 0);
    // }
}
