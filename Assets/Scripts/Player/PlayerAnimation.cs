using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Component References")]
    private Animator Anim;

    private void Start()
    {
        Anim = GetComponent<Animator>();

    }

    public void updateMovement(float Value)
    {
        Anim.SetFloat("Movement",Value,0.1f,Time.deltaTime);
    }

    public void Crouch(bool Value)
    {
        Anim.SetBool("isCrouching", Value);
    }
    public void slide()
    {
        Anim.SetTrigger("isSliding");
    }

    public void jump(bool Value)
    {
        Anim.SetBool("isjumping",Value);
    }

}
