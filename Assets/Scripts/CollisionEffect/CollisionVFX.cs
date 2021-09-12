using UnityEngine;

public class CollisionVFX : CollisionEffect
{
    [SerializeField]
    private GameObject vfx;

    public override void OnCollision(Vector3 point)
    {
        Instantiate(vfx, point, Quaternion.identity);
    }
    
}
