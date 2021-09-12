using UnityEngine;

public class HeatingMapController : MonoBehaviour
{
    [SerializeField]
    private GameObject spotPrefab;
    [SerializeField]
    private RenderTexture heatingMap;
    [SerializeField]
    private Transform coolSpotsRoot;

    private float size = 5;

    public void ClearMap()
    {
        foreach(Transform child in coolSpotsRoot)
            GameObject.Destroy(child.gameObject);
    }

    public RenderTexture GetHeatingMap() => heatingMap;

    public void AddSpot(Vector2 pos)
    {
        pos.x = GetPivotCoord(pos.x);
        pos.y = GetPivotCoord(pos.y);
        var spawnedSpotTransform = Instantiate(spotPrefab, coolSpotsRoot).transform;
        spawnedSpotTransform.localPosition = new Vector3(pos.x, pos.y, 0f)*size;
        spawnedSpotTransform.localRotation = Quaternion.identity;
    }

    private float GetPivotCoord(float coord) => Mathf.Clamp(coord, 0f, 1f) - 0.5f;



}
