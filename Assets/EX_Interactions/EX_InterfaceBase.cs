using UnityEngine;
using UnityEngine.Events;

// 인스펙터에 확실히 보이게 하기 위한 선언
[System.Serializable]
public class GameObjectEvent : UnityEvent<GameObject> { }

public class EX_InterfaceBase : MonoBehaviour, IInterface
{
    [Header("Enter: 감지/진입")]
    public UnityEvent OnEventEnter;

    [Header("Action(Click): 실행")]
    public UnityEvent OnEventAction;

    [Header("Exit: 해제/이탈")]
    public UnityEvent OnEventExit;

    [Header("Enter: GameObject 매개변수")]
    public UnityEvent<GameObject> OnEventEnterWithSender;

    [Header("Action(Click): GameObject 매개변수")]
    public UnityEvent<GameObject> OnEventActionWithSender;

    [Header("Exit: GameObject 매개변수")]
    public UnityEvent<GameObject> OnEventExitWithSender;

    /*
    //IInteractable 인터페이스의 실제 구현부
    //인터페이스에 정의된 이름과 정확히 일치해야 함
    public virtual void OnEnter() => OnEventEnter?.Invoke();
    public virtual void OnAction() => OnEventAction?.Invoke();
    public virtual void OnExit() => OnEventExit?.Invoke();
    */

    // 매개변수가 없는 버전은 매개변수 있는 버전을 호출하되 sender에 null이나 자신을 전달
    public virtual void OnEnter() => OnEnter(null);
    public virtual void OnAction() => OnAction(null);
    public virtual void OnExit() => OnExit(null);
    public virtual void OnEnter(GameObject sender)
    {
        // 인수가 없는 Invoke
        OnEventEnter?.Invoke();

        // 인수를 1개 담아서 보내는 Invoke (이제 에러가 나지 않습니다!)
        OnEventEnterWithSender?.Invoke(sender);
    }

    public virtual void OnAction(GameObject sender)
    {
        // 인수가 없는 Invoke
        OnEventAction?.Invoke();

        // 인수를 1개 담아서 보내는 Invoke (이제 에러가 나지 않습니다!)
        OnEventActionWithSender?.Invoke(sender);
    }

    public virtual void OnExit(GameObject sender)
    {
        // 인수가 없는 Invoke
        OnEventExit?.Invoke();

        // 인수를 1개 담아서 보내는 Invoke (이제 에러가 나지 않습니다!)
        OnEventExitWithSender?.Invoke(sender);
    }
    
}

public interface IInterface
{
    // 1. 상태 변화 (Hover)
    // 인터랙션 대상 근처에 가거나 벗어날 때
    void OnEnter();
    void OnExit();

    // 2. 실행 동작 (Click)
    // 클릭하거나, 손가락 끝으로 찌르거나(Poke), 엄지와 검지로 집을 때(Pinch)
    // 주로 '버튼 누르기'나 '선택'의 의미
    void OnAction();

    
    // 호출자(GameObject)를 매개변수로 받도록 설계 추가
    void OnEnter(GameObject sender);
    void OnExit(GameObject sender);
    void OnAction(GameObject sender);
    
}
