using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    public float delay = 3.0f;

    void Start()
    {
        
        Invoke("Hide", delay);
    }

    void Hide()
    {
        
        gameObject.SetActive(false);
    }
}