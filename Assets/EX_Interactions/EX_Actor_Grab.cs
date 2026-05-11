using UnityEngine;

public class EX_Actor_Grab : MonoBehaviour
{
    private bool isGrabbed = false;
    public void Execute()
    {
        isGrabbed = !isGrabbed;
        transform.SetParent(isGrabbed ? Camera.main.transform : null);
        if (isGrabbed) { transform.localPosition = new Vector3(0, 0, 1.5f); transform.localRotation = Quaternion.identity; }
    }
}