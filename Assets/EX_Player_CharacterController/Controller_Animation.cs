using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Animation : MonoBehaviour
{
    public Animator animator;

    public void SetInteger(int value)
    {
        animator.SetInteger("DoorControl", value);
    }
}
