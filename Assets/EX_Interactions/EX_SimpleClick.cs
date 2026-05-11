using UnityEngine;
using UnityEngine.Events; 
public class EX_SimpleClick : MonoBehaviour
{
    
    public UnityEvent OnClick;

   
    void OnMouseDown()
    {
        OnClick.Invoke();
    }
}