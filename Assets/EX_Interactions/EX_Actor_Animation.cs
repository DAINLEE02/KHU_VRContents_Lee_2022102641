using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EX_Actor_Animation : MonoBehaviour
{
    public Animator animator;

    public void SetInteger(int value)
    {
        animator.SetInteger("DoorControl", value);
    }

    public void SetInteger(string Param, int value)
    {
        animator.SetInteger(Param, value);
    }
}
