using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator Anim;

    private void Start()
    {
        Anim = GetComponent<Animator>();

    }

    public void updateMovement(bool Value)
    {
        Anim.SetBool("isWalking", Value);
    }

    public void Crouch(bool Value)
    {
        Anim.SetBool("isCrouching", Value);
    }
}
