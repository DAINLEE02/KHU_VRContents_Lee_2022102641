using UnityEngine;

public class EX_Trigger_Mouse : MonoBehaviour
{
    // 클릭 신호를 받으면 실행
    public void ExecuteInteraction()
    {
        // 1. 잡기 기능 찾아서 실행
        var grab = GetComponent<EX_Actor_Grab>();
        if (grab != null) grab.Execute();

        // 2. 씬 이동 기능 찾아서 실행
        var scene = GetComponent<EX_Actor_Scene>();
        if (scene != null) scene.Execute();
    }
}