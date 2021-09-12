using UnityEngine;

public abstract class Raycaster : MonoBehaviour
{
	private Camera raycastCam;
	private int timer = 0;
	private bool isRaycasting = false;

	public int rate = 1;

	protected virtual void Start()
    {
        Time.fixedDeltaTime = 0.01f;
		raycastCam = Camera.main;
	}

    public void SetRaycasting(bool raycasting) => isRaycasting = raycasting;

	protected virtual void Update()
	{
		timer += rate;
		if(isRaycasting && timer > 60)
		{
			timer = 0;
			Ray ray = raycastCam.ScreenPointToRay(GetRaycastPosition());
			if(Physics.Raycast(ray, out var hit))
				OnRaycasted(hit);
		}
	}

	protected abstract Vector2 GetRaycastPosition();

	protected abstract void OnRaycasted(RaycastHit hit);
}
