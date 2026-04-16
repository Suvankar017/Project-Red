using UnityEngine;

public class PortInteractionHandler : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D portCircleCollider;
    [SerializeField]
    private MeshRenderer portGraphicsRenderer;

    private float defaultColliderRadius;
    private float defaultGraphicsScale;
    private MaterialPropertyBlock materialPropertyBlock;

    private void Awake()
    {
        defaultColliderRadius = portCircleCollider.radius;
        defaultGraphicsScale = portGraphicsRenderer.transform.localScale.x;

        materialPropertyBlock = new MaterialPropertyBlock();
    }

    public void OnMouseEnter()
    {
        const float scaleMultiplier = 1.5f;

        portCircleCollider.radius = defaultColliderRadius * scaleMultiplier;
        portGraphicsRenderer.transform.localScale = defaultGraphicsScale * scaleMultiplier * Vector3.one;

        portGraphicsRenderer.GetPropertyBlock(materialPropertyBlock);
        materialPropertyBlock.SetColor("_Color", Color.grey);
        portGraphicsRenderer.SetPropertyBlock(materialPropertyBlock);
    }

    private void OnMouseExit()
    {
        portCircleCollider.radius = defaultColliderRadius;
        portGraphicsRenderer.transform.localScale = defaultGraphicsScale * Vector3.one;

        portGraphicsRenderer.GetPropertyBlock(materialPropertyBlock);
        materialPropertyBlock.SetColor("_Color", Color.black);
        portGraphicsRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}
