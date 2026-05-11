using UnityEngine;

// 이 스크립트를 아웃라인이 필요한 오브젝트에 붙입니다.
[RequireComponent(typeof(Outline))]
public class EX_Actor_Outline : MonoBehaviour
{
    private Outline outline;
    Color OutlineColor;

    void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false; // 시작할 때는 끕니다.
        OutlineColor = outline.OutlineColor;
        //print("Controller_Outline");
    }

    public void Outline_Show()
    {
        outline.enabled = true;
        outline.OutlineColor = OutlineColor;
    }


    public void Outline_Hide()
    {
        outline.enabled = false;
        outline.OutlineColor = OutlineColor;
    }

    public void Outline_Red()
    {
        Debug.Log($"{gameObject.name}을 클릭했습니다!");
        outline.OutlineColor = Color.red; // 클릭 시 색상 변경 예시
    }
}