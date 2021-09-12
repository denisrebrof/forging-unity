using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbit : MonoBehaviour {
	
	public Transform target;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	public float smoothSpeed = 16f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	
	public float distanceMin = .5f;
	public float distanceMax = 15f;

	
	float x = 0.0f;
	float y = 0.0f;
	
	// Use this for initialization
	void Start () {
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}

	private float prevRealTime;
	private float thisRealTime;
	void Update(){
		prevRealTime = thisRealTime;
		thisRealTime = Time.realtimeSinceStartup;

		if (Input.GetMouseButtonDown (2)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;			
			if (Physics.Raycast (ray, out hit)) {
				if(hit.collider.transform.parent)
				if(hit.collider.transform.parent.name == "Targets"){
					target = hit.collider.transform;
				}
			}
		}
	}

	public float deltaTime {
		get {
			if (Time.timeScale > 0f)  return  Time.deltaTime / Time.timeScale;
			return Time.realtimeSinceStartup - prevRealTime; // Checks realtimeSinceStartup again because it may have changed since Update was called
		}
	}

	void LateUpdate () {
		if (target) {

			if(Input.GetMouseButton(1)){

			x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			
			y = ClampAngle(y, yMinLimit, yMaxLimit);

			}
			
			Quaternion rotation = Quaternion.Euler(y, x, 0);

			distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);

			/*if (Physics.Linecast (target.position, transform.position, out hit)) {
				distance -=  hit.distance;
			}*/
			Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + target.position;

			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, deltaTime * smoothSpeed);
			transform.position = Vector3.Lerp(transform.position, position, deltaTime * smoothSpeed);
			
		}
		
	}
	
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
	
	
}