using System.Linq;
using UnityEngine;

public class HeatedObject : MonoBehaviour
{
    private HeatingMapController mapController;
    private RenderTexture heatingRT;
    public float fullpaintPercent;
    public float paintPercent;

    private void Start()
    {
        mapController = Resources.FindObjectsOfTypeAll<HeatingMapController>().FirstOrDefault();
        heatingRT = mapController.GetHeatingMap();
        var materials = GetComponentsInChildren<MeshRenderer>().SelectMany(mr => mr.materials);
        foreach(var mat in materials)
            mat.SetTexture("heating", heatingRT);
    }

    public void AddSpot(Vector2 uvPoint)
    {
        mapController.AddSpot(uvPoint);
        TextureMath.CalcPercentColorRtAsync(heatingRT, val => paintPercent = val);
    }

    public void OnCollisionEnter(Collision collision)
    {
        var ray = new Ray(collision.contacts[0].point -collision.contacts[0].normal, collision.contacts[0].normal);
        var hits = Physics.RaycastAll(ray);
        foreach(var hit in hits)
        {
            if(hit.transform==transform)
                AddSpot(hit.textureCoord);
        }
    }
}
