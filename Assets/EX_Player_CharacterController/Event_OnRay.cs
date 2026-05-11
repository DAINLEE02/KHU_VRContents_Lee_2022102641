using UnityEngine;

public class Event_OnRay : MonoBehaviour
{
    public LineRenderer lineRenderer; // 인스펙터에서 연결할 것
    public float rayLength = 10f;      // 레이저 길이

    void Update()
    {
        if (Input.GetMouseButton(0)) // 클릭 중일 때
        {
            lineRenderer.enabled = true;
            // 레이저 그리기
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * rayLength);

            // 레이저 판정
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
            {
                var trigger = hit.collider.GetComponent<EX_Trigger_Mouse>();
                if (trigger != null)
                {
                    trigger.ExecuteInteraction(); // 곰돌이 작동
                }
            }
        }
        else
        {
            lineRenderer.enabled = false; // 클릭 안 하면 끔
        }
    }
}