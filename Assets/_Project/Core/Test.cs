using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshRenderer))]
public class Test : MonoBehaviour
{
    public Color color = Color.white;

    private MeshRenderer meshRenderer;
    private MaterialPropertyBlock materialPropertyBlock;

    private void OnEnable()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

        materialPropertyBlock ??= new MaterialPropertyBlock();
    }

    private void Update()
    {
        if (meshRenderer == null || materialPropertyBlock == null)
            return;

        meshRenderer.GetPropertyBlock(materialPropertyBlock);
        materialPropertyBlock.SetColor("_Color", color);
        meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}
