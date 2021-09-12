using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[AddComponentMenu("Physics/Psoft Body")]
[RequireComponent (typeof(MeshFilter))]
public class PsoftBody : MonoBehaviour {

	[Header("SoftBody Settings:")]
	[Tooltip("Minimum velocity of the collision in order to trigger the psoft calculation.")]
	public float minSoftVelocity = 4f;
	[Tooltip("Scale of the collision impact. A larger scale means a larger dent.")]
	public float impactScale = 1f;
	[Tooltip("Multiplier for the force of the collision. A larger multiplier results in deeper dents.")]
	public float forceMultiplier = 1f;

	[Header("Normals Settings:")]
	[Tooltip("Whether or not to recalculate the normals after the psoft calculation. (Recommended)")]
	public bool recalculateNormals = true;
	[Tooltip("Whether or not to use a better method for recalculating the normals after the psoft calculation. (Recommended)")]
	public bool betterRecalculateNormals = true;
	[Tooltip("When using the better normal recalculation, this is the smoothing angle used in calculation.")]
	public float smoothingAngle = 60f;

	[Header("Collision Settings:")]
	[Tooltip("This will automatically deform the mesh when something collides with it." +
		" (This will only work if the object is not a child of something else that steals its OnCollisionEnter event." +
		" If it is you'll need to relay the collision object to this scripts PsoftCalculate() method.)")]
	public bool deformOnCollision = true;
	[Tooltip("Using this will cause the psoft body to use a mesh collider that will be updated every time the mesh is deformed.")]
	public bool dynamicMeshCollision = true;
	[Tooltip("Take the mass of the colliding object into account when calculating deformation.")]
	public bool useColliderMass = true;
	[Tooltip("Take the scale of the colliding object into account when calculating deformation.")]
	public bool useColliderScale = true;
	[Tooltip("Use the collisions normal for deformation direction instead of the collisions relative velocity. This drastically changes impact force modeling.")]
	public bool useCollisionNormal = false;
	[Tooltip("Use the OnCollisionStay method for triggering deformations, best used with a minSoftVelocity of 0. This can be useful in some situations though is not recommended.")]
	public bool useCollisionStay = false;
	[Tooltip("Make sure the collider used for deformation is this objects collider and not some other one." +
		" This is useful for when you need to call the PsoftCalculate() method from elsewhere with collision info that could contain collisions from other things.")]
	public bool checkColliderIsThisObject = false;

	public bool deformByNormals;

	MeshFilter mF;
	MeshCollider mC;
	Mesh originalMesh;
	bool startUpCol = true;

	// Use this for initialization
	void Start () {
		if(GetComponent<SkinnedMeshRenderer>()){
			Debug.LogError("PsoftBody: Skinned meshes are not supported");
			return;
		}		

		mF = gameObject.GetComponent<MeshFilter> ();
		if(dynamicMeshCollision){
			if(!gameObject.GetComponent<Rigidbody>()){
				swapColliders();
			}else{
				#if UNITY_5
				if((mF.sharedMesh.triangles.Length/3) > 256){
					Debug.LogError("PsoftBody: Dynamic collsion is not supported on Rigidbody meshes with more than 256 polygons in Unity 5! " +
						"This mesh has " + (mF.sharedMesh.triangles.Length/3) + " polygons.");
					dynamicMeshCollision = false;
				}else{
					swapColliders();
					mC.convex = true;
				}
				#else
				swapColliders();
				#endif
			}
		}
		startUpCol = dynamicMeshCollision;
		originalMesh = mF.sharedMesh;
		mF.sharedMesh = Instantiate(mF.sharedMesh) as Mesh;
	}

	void swapColliders(){
		foreach(Collider cld in GetComponents<Collider>()){
			Destroy(cld);
		}
		mC = gameObject.AddComponent<MeshCollider> ();
		mC.sharedMesh = mF.sharedMesh;
	}

	void OnCollisionEnter(Collision col) {
		PsoftCalculate(col);
	}

	void OnCollisionStay(Collision col) {
		if(useCollisionStay)
			PsoftCalculate(col);
	}

	//call this with collision info to perform psoft calculation with that collision.
	public void PsoftCalculate(Collision col){
		if (col.relativeVelocity.magnitude < minSoftVelocity)
			return;
		float scale = 1f;
		if (useColliderScale && col.rigidbody && col.collider) {
			scale = col.collider.bounds.size.magnitude * 0.7f;
		}
		float tIS = impactScale / 40f;
		float tFM = forceMultiplier / 12.5f;

		float maxDist = col.relativeVelocity.magnitude * tIS * scale;
		if (maxDist > scale + 0.25f)
			maxDist = scale + 0.25f;

		Vector3[] softVerts = mF.sharedMesh.vertices;
		Vector3[] normals = mF.sharedMesh.normals;
		ContactPoint[] contcts = col.contacts;
		float tmass = 1;
		if(useColliderMass)
		if(col.rigidbody ){
			tmass = col.rigidbody.mass;
		}
		Vector3 impactVel = col.relativeVelocity / 40f;
		foreach (ContactPoint contct in contcts) {
			if(checkColliderIsThisObject)
			if(contct.thisCollider.transform != transform)
				continue;
			Vector3 lP = contct.point;
			for(int i = 0; i < softVerts.Length; i++){
				Vector3 worldVert = transform.TransformPoint(softVerts[i]);
				float tmpDist = Vector3.Distance(lP, worldVert);
				if(tmpDist < maxDist){
					float tforce = maxDist - tmpDist;
					if(useCollisionNormal)
						worldVert += contct.normal * (tforce * tFM * tmass);
					else
						worldVert += impactVel * (tforce * tFM * tmass);
					var targetPoint = transform.InverseTransformPoint(worldVert);
					if(deformByNormals)
					{
						var pointTranslation = Vector3.Project(targetPoint-softVerts[i],normals[i]);
						softVerts[i] += pointTranslation;
					}
					else
					{
						softVerts[i] = targetPoint;
					}
				}
			}
		}

		FinalizeNewMesh(softVerts);
		//END
	}

	void FinalizeNewMesh(Vector3[] softVerts){
		mF.sharedMesh.vertices = softVerts;
		mF.sharedMesh.RecalculateBounds ();

		if(recalculateNormals)
		if(betterRecalculateNormals)
			mF.sharedMesh.BetterRecalculateNormals (60);
		else
			mF.sharedMesh.RecalculateNormals();

		CheckDynamic();
	}

	void CheckDynamic(){
		if(dynamicMeshCollision != startUpCol){
			Debug.LogWarning("PsoftBody: Dyanamic collision should not be changed at runtime.");
			dynamicMeshCollision = startUpCol;
		}

		if (dynamicMeshCollision)
			mC.sharedMesh = mF.sharedMesh;
	}


	//Useful public methods
	public void ResetMesh(){
		//if(!mF)
		//	return;
		Destroy(mF.sharedMesh);
		mF.sharedMesh = Instantiate(originalMesh) as Mesh;

		CheckDynamic();
	}

	float lastTime = 0f;
	public void LerpTowardsOriginal(float t){
		Vector3[] softVerts = mF.sharedMesh.vertices;
		for(int i = 0; i < softVerts.Length; i++){
			softVerts[i] = Vector3.Lerp(softVerts[i], originalMesh.vertices[i], t);
		}
		mF.sharedMesh.vertices = softVerts;
		mF.sharedMesh.RecalculateBounds ();

		if(recalculateNormals)
		if(betterRecalculateNormals)
			mF.sharedMesh.BetterRecalculateNormals (60);
		else
			mF.sharedMesh.RecalculateNormals();
	}

	public void RandomImpact(float impactForce = 15f, float impactScale = 1f, bool inverseImpacts = false){
		int nVert = Random.Range(0, mF.sharedMesh.vertexCount);

		float tIS = impactScale / 40f;
		float tFM = forceMultiplier / 12.5f;

		float maxDist = impactForce * tIS * impactScale;
		if (maxDist > impactScale + 0.25f)
			maxDist = impactScale + 0.25f;

		Vector3[] softVerts = mF.sharedMesh.vertices;
		Vector3[] normals = mF.sharedMesh.normals;
		Vector3 lP = transform.TransformPoint(softVerts[nVert]);
		for(int i = 0; i < softVerts.Length; i++){
			Vector3 worldVert = transform.TransformPoint(softVerts[i]);
			float tmpDist = Vector3.Distance(lP, worldVert);
			if(tmpDist < maxDist){
				float tforce = maxDist - tmpDist;
				if(inverseImpacts)
					worldVert += mF.sharedMesh.normals[nVert] * (tforce * tFM);
				else
					worldVert += -mF.sharedMesh.normals[nVert] * (tforce * tFM);
				var targetPoint = transform.InverseTransformPoint(worldVert);
				if(deformByNormals)
				{
					var pointTranslation = Vector3.Project(targetPoint-softVerts[i], normals[i]);
					softVerts[i] += pointTranslation;
				}
				else
				{
					softVerts[i] = targetPoint;
				}
			}
		}

		FinalizeNewMesh(softVerts);
		//END
	}

	public void DeformAtPoint(Vector3 worldPoint, Vector3 impactNormal, float impactForce = 15f, float impactScale = 1f, bool inverseImpacts = false, float custForceMult = 0f){
		float tIS = impactScale / 40f;
		float tFM = forceMultiplier / 12.5f;
		if(custForceMult != 0f)
			tFM = custForceMult / 12.5f;		

		float maxDist = impactForce * tIS * impactScale;
		if (maxDist > impactScale + 0.25f)
			maxDist = impactScale + 0.25f;

		Vector3[] softVerts = mF.sharedMesh.vertices;
		Vector3[] normals = mF.sharedMesh.normals;
		for(int i = 0; i < softVerts.Length; i++){
			Vector3 worldVert = transform.TransformPoint(softVerts[i]);
			float tmpDist = Vector3.Distance(worldPoint, worldVert);
			if(tmpDist < maxDist){
				float tforce = maxDist - tmpDist;
				if(inverseImpacts)
					worldVert += impactNormal * (tforce * tFM);
				else
					worldVert += -impactNormal * (tforce * tFM);
				var targetPoint = transform.InverseTransformPoint(worldVert);
				if(deformByNormals)
				{
					var pointTranslation = Vector3.Project(targetPoint-softVerts[i], normals[i]);
					softVerts[i] += pointTranslation;
				}
				else
				{
					softVerts[i] = targetPoint;
				}
			}
		}

		FinalizeNewMesh(softVerts);
		//END
	}



	//END CLASS
}
