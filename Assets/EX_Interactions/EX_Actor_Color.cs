using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EX_Actor_Color : MonoBehaviour
{
    [Header("Set Color")]
    public Renderer ActorRenderer;
    private Material ActorMaterial;
    private Color OriginalColor;
    public Color EnterColor = Color.yellow;
    public Color ActionColor = Color.red;  
    public Color CustomColor = Color.white;

    void Awake()
    {
        if (ActorRenderer == null) ActorRenderer = GetComponent<Renderer>();
        
        if (ActorRenderer != null)
        {
            ActorMaterial = ActorRenderer.material;
            OriginalColor = ActorMaterial.color;
        }
    }


    public void SetEnterColor()
    {
        if (ActorMaterial != null)
        {
            ActorMaterial.color = EnterColor;
        }
    }

    public void SetActioColor()
    {
        if (ActorMaterial != null)
        {
            ActorMaterial.color = ActionColor;
        }
    }

    public void SetCustomColor()
    {
        if (ActorMaterial != null)
        {
            ActorMaterial.color = CustomColor;
        }
    }

    public void SetOriginalColor()
    {
        if (ActorMaterial != null)
        {
            ActorMaterial.color = OriginalColor;
        }
    }
}
