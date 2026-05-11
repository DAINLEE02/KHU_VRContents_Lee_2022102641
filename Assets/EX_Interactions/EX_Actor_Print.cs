using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EX_Actor_Print : MonoBehaviour
{
    public string Message;
    public void PrintMessage(string message)
    {
        Debug.Log(message);
    }

    public void PrintMessage()
    {
        Debug.Log(Message);
    }
}
