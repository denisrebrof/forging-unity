using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class DrawMeshBounds : MonoBehaviour
{
    [SerializeField]
    private Renderer mashableRenderer;

    private void Start()
    {
        if(!mashableRenderer)
            mashableRenderer = GetComponent<Renderer>();
    }

    private void OnDrawGizmosSelected()
    {
        // A sphere that fully encloses the bounding box.
        Vector3 center = mashableRenderer.bounds.center;
        float radius = mashableRenderer.bounds.extents.y;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(center, radius);
    }
}
