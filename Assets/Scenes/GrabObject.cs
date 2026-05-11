using UnityEngine;

public class GrabObject : MonoBehaviour, IRayInteractable
{
    private bool isGrabbed = false;
    private Rigidbody rb;
    private int originalLayer; 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalLayer = gameObject.layer; 
    }

    public void OnRayEnter() { }
    public void OnRayStay() { }
    public void OnRayExit() { }
    public void OnRayClick() { ToggleGrab(); }
    public void OnSelect() { }

    public void ToggleGrab()
    {
        isGrabbed = !isGrabbed;

        if (isGrabbed)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Transform eye = player.transform.Find("CameraRig");
                if (eye != null)
                {
                    transform.SetParent(eye);

                    transform.localPosition = new Vector3(0, 0, 2.5f);
                    transform.localRotation = Quaternion.identity;

                    gameObject.layer = 2;

                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        rb.useGravity = false;
                    }
                }
            }
        }
        else
        {
     
            transform.SetParent(null);

          
            gameObject.layer = originalLayer;

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }
    }
}