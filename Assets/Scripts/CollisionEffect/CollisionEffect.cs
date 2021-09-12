using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CollisionEffect : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var points = collision.contacts.Select(contact => contact.point);
        var averagePoint = GetAveragePoint(points);
        OnCollision(collision.contacts[0].point);
    }

    private Vector3 GetAveragePoint(IEnumerable<Vector3> points)
    {
        var average = Vector3.zero;
        foreach(var point in points)
            average+=point;
        average/=points.Count();
        return average;
    }

    public abstract void OnCollision(Vector3 point);
}
