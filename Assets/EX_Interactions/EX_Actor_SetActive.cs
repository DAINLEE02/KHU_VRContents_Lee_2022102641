using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EX_Actor_SetActive : MonoBehaviour
{
    public GameObject Target;
    public bool setActive = true;


    private void Awake()
    {
        if(Target == null) Target = gameObject;
    }
    public void SetActive()
    {
        Target.SetActive(setActive);
    }

    public void Setactive(bool active)
    {
        setActive = active;
        Target.SetActive(setActive);
    }

    public void SetActiveToggle()
    {
        if (Target.activeSelf == true)
        {
            Target.SetActive(false);
        }
        else
        {
            Target.SetActive(true);
        }
    }
    
}
