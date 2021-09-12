using UnityEngine;
using System.Collections;

public class ObjectThrower : MonoBehaviour {

	[Header("Throw Settings:")]
	public float throwforce = 5f;
	public int throwRate = 1;
	public GameObject throwable;

	GameObject mainCam;
	GameObject oParent;

	// Use this for initialization
	void Start () {
		mainCam = GameObject.FindGameObjectWithTag ("MainCamera");
		oParent = new GameObject ();
		oParent.name = "Throwables";
		Time.fixedDeltaTime = 0.01f;
	}
	
	int tt = 0;
	void Update(){
		tt += throwRate;
		if (Input.GetMouseButton (0) && tt > 60) {
			tt = 0;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;			
			if (Physics.Raycast (ray, out hit)) {
				GameObject throwInst = Instantiate (throwable, mainCam.transform.position + (mainCam.transform.forward * 1.75f), mainCam.transform.rotation) as GameObject;
				Rigidbody tmpRB = throwInst.GetComponent<Rigidbody>();
				throwInst.transform.LookAt(hit.point);
				tmpRB.AddRelativeForce(new Vector3(0,0,throwforce));
				throwInst.transform.SetParent(oParent.transform);

				Renderer tR = throwInst.GetComponent<Renderer>();
				tR.material = Instantiate(tR.material) as Material;
				tR.material.color = RandomColor();

			}
		}
	}

	Color RandomColor(){
		float r = Random.Range (0.0f, 1.0f);
		float g = Random.Range (0.0f, 1.0f);
		float b = Random.Range (0.0f, 1.0f);
		return new Color(r,g,b);
	}

	//END CLASS
}
