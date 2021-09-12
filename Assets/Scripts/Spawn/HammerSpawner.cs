using System;
using System.Linq;
using UnityEngine;

public class HammerSpawner : MonoBehaviour, ISpawner
{
    [Header("Throw Settings:")]
    public float throwforce = 5f;
    
    public GameObject throwable;

	[SerializeField]
	private Transform[] spawns;

	private GameObject oParent;

	private Action<GameObject> onSpawned;

	Action<GameObject> ISpawner.OnSpawned => onSpawned;

	private void Start()
    {
        oParent = new GameObject();
        oParent.name = "Throwables";
    }

	public void SpawnHammer(Vector3 target)
	{
		var spawnPos = spawns.OrderBy(spawn => (target-spawn.position).magnitude).First().position;
		var spawnRot = Quaternion.LookRotation(target-spawnPos, Vector3.up);
		var throwInst = Instantiate(throwable, spawnPos, spawnRot);
		Rigidbody tmpRB = throwInst.GetComponent<Rigidbody>();
		throwInst.transform.LookAt(target);
		tmpRB.AddRelativeForce(new Vector3(0, 0, throwforce));
		throwInst.transform.SetParent(oParent.transform);
	}

	
	
}
