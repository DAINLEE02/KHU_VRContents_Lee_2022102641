using UnityEngine;

public class EX_Actor_Mouse : MonoBehaviour
{
    //[Header("Grab Settings")]
    //public bool canBeGrabbed = true;

    [Header("Physics Settings (On Release)")]
    [Range(0, 10)] public float releaseDrag = 2f;         // 놓았을 때 이동 저항
    [Range(0, 10)] public float releaseAngularDrag = 2f;  // 놓았을 때 회전 저항

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // 잡혔을 때 호출 (EX_Trigger_Ray에서 호출되도록 구성)
    public void SetGrabPhysics()
    {
        if (rb == null) return;
        rb.drag = 0;           // 잡고 휘두를 때는 저항을 없앰
        rb.angularDrag = 0.05f;
    }

    // 놓았을 때 호출
    public void SetReleasePhysics()
    {
        if (rb == null) return;
        rb.drag = releaseDrag;               // 서서히 멈추도록 저항 증가
        rb.angularDrag = releaseAngularDrag; // 구르는 것을 방지하기 위해 회전 저항 증가
    }
}