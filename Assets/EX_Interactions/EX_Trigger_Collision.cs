using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EX_Trigger_Collision : MonoBehaviour
{
    public GameObject InterfaceObject; // 인터페이스가 붙어있는 게임 오브젝트를 지정할 수 있도록 public으로 선언
    IInterface Interface;

    void Awake(){
        if(InterfaceObject == null){
            Interface = GetComponent<IInterface>();
        }
        else
        {
            Interface = InterfaceObject.GetComponent<IInterface>();
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger Enter");
        Interface.OnEnter();       
    }

    void OnTriggerExit(Collider other)
    {
        print("Trigger Exit");
        Interface.OnExit();  
    }

    void OnCollisionEnter(Collision collision)
    {
        print("Collision Enter");
        Interface.OnEnter();
    }

    void OnCollisionExit(Collision collision)
    {
        print("Collision Exit");
        Interface.OnExit();
    }
}
