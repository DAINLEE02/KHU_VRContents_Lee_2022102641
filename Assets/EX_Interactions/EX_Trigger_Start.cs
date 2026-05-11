using UnityEngine;
using UnityEngine.Events;

public class EX_Trigger_Start : MonoBehaviour
{
   
    public UnityEvent onStart;

    void Start()
    {
        
        onStart.Invoke();
    }
}