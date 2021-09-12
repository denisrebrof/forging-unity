using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WheelGroup {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor;
	public bool steering;
}

public class CarController : MonoBehaviour {
	public List <WheelGroup> axleInfos;
	public float maxMotorTorque;
	public float maxSteeringAngle;

	public List<PsoftBody> psoftWheels;
	[Header("Psoft Settings")]
	public bool toggle = true;
	public float forceMult = 1f;
	public float scaleMult = 1f;
	public float reboundSpeed = 1.5f;
	[Range(0f, 2f)]
	public float timeScale = 1f;

	private float prevRealTime;
	private float thisRealTime;
	void Start(){
		Time.fixedDeltaTime = 0.006f;
	}

	void Update(){
		if(toggle)
		foreach (WheelGroup wc in axleInfos) {
			WheelHit hit;
			if(wc.leftWheel.GetGroundHit(out hit)){
				//Debug.DrawRay(hit.point, hit.normal * 0.5f, Color.cyan, 5f);
				PsoftBody psoftWheel = wc.leftWheel.GetComponentInChildren<PsoftBody>();
				if(psoftWheel){					
					psoftWheel.DeformAtPoint(hit.point, hit.normal, hit.force * forceMult, scaleMult, true, 1f);
				}
			}
			if(wc.rightWheel.GetGroundHit(out hit)){
				//Debug.DrawRay(hit.point, hit.normal * 0.5f, Color.cyan, 5f);
				PsoftBody psoftWheel = wc.rightWheel.GetComponentInChildren<PsoftBody>();
				if(psoftWheel){
					psoftWheel.DeformAtPoint(hit.point, hit.normal, hit.force * forceMult, scaleMult, true, 1f);
				}
			}
		}

		Time.timeScale = timeScale;
		prevRealTime = thisRealTime;
		thisRealTime = Time.realtimeSinceStartup;
		foreach(PsoftBody pw in psoftWheels)
			pw.LerpTowardsOriginal(deltaTime * reboundSpeed);
		
	}

	public float deltaTime {
		get {
			if (Time.timeScale > 0f)  return  Time.deltaTime / Time.timeScale;
			return Time.realtimeSinceStartup - prevRealTime; // Checks realtimeSinceStartup again because it may have changed since Update was called
		}
	}

	public bool onGround(){
		bool retb = true;
		foreach (WheelGroup wc in axleInfos) {
			retb = wc.leftWheel.isGrounded;
			retb = wc.rightWheel.isGrounded;
		}
		return retb;
	}

	// finds the corresponding visual wheel
	// correctly applies the transform
	public void ApplyLocalPositionToVisuals(WheelCollider collider)
	{
		if (collider.transform.childCount == 0) {
			return;
		}
		
		Transform visualWheel = collider.transform.GetChild(0);
		
		Vector3 position;
		Quaternion rotation;
		collider.GetWorldPose(out position, out rotation);
		
		visualWheel.transform.position = position;
		visualWheel.transform.rotation = rotation;
	}
	
	public void FixedUpdate()
	{
		float motor = maxMotorTorque * Input.GetAxis("Vertical");
		float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
		
		foreach (WheelGroup axleInfo in axleInfos) {
			if (axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = steering;
				axleInfo.rightWheel.steerAngle = steering;
			}
			if (axleInfo.motor) {
				axleInfo.leftWheel.motorTorque = motor;
				axleInfo.rightWheel.motorTorque = motor;
			}
			ApplyLocalPositionToVisuals(axleInfo.leftWheel);
			ApplyLocalPositionToVisuals(axleInfo.rightWheel);
		}
	}


	void OnCollisionEnter(Collision col) {
		foreach(PsoftBody pw in psoftWheels)
			pw.PsoftCalculate(col);
	}


	//end
}