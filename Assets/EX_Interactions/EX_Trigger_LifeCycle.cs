using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EX_Trigger_LifeCycle : MonoBehaviour
{
    public GameObject InterfaceObject; // 인터페이스가 붙어있는 게임 오브젝트를 지정할 수 있도록 public으로 선언
    IInterface Interface;
    //public EX_Actor_Text TextActor;

    [Header("지연된 실행: 카운더")]
    public float maxCount = 10f;
    bool isCount;
    float currentCount = 0;

    [Header("지연된 실행: 타이머")]
    public float maxTime = 10f;
    bool isRun;

    void Awake()
    {
        if (InterfaceObject == null)
        {
            Interface = GetComponent<IInterface>();
        }
        else
        {
            Interface = InterfaceObject.GetComponent<IInterface>();
        }
    }

    private void Start()
    {
        Interface.OnAction();
    }
    private void Update()
    {
        if (isCount)
        {
            Count();
            //OutputText();
        }

        if (isRun)
        {
            Timer();
            //OutputText();
        }
    }

    private void OnEnable()
    {
        Interface.OnEnter();
    }

    private void OnDisable()
    {
        Interface.OnExit();
    }

    public void StartCounter()
    {
        isCount = true;
    }

    public void StartCounter(int _maxCount)
    {
        maxCount = _maxCount;
        isCount = true;
    }

    void Count()
    {
        currentCount++;
        if (currentCount >= maxCount)
        {
            currentCount = 0;
            isCount = false;
            Interface.OnAction();
        }
    }

    public void StartTimer(float _maxTime)
    {
        maxTime = _maxTime;
        isRun = true;
    }

    public void StartTimer()
    {
        isRun = true;
    }

    void Timer()
    {
        currentCount += Time.deltaTime;
        if (currentCount >= maxTime)
        {
            currentCount = 0;
            isRun = false;
            Interface.OnAction();
        }
    }

    public void OutputText(EX_Actor_Text TextActor)
    {
        if (TextActor == null) return;
        TextActor.SetText();
    }
}
