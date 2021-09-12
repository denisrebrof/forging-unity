Thanks for using my [pseudo soft body] script!


Setup:
1. Drag the PsoftBody component onto your mesh gameobject.
  (you can find it under Physics->Psoft Body)

2. Here is where you can adjust the settings of the component in the inspector to your liking.

3. By default, The mesh will now deform on collisions, if you plan to use raycasts or some other method to cause deformations
   then you can/should turn off deformOnCollision in the inspector settings.
   You can also call some of the built in methods of the script from other scripts at runtime.


Useful Methods in the PsoftBody script:

  //will reset the mesh of the object to its original form
    public void ResetMesh(){...}
  //usage example
    GetComponent<PsoftBody>().ResetMesh();

  //will lerp all vertices in the mesh of the psoftbody towards the original mesh vertices
    public void LerpTowardsOriginal(float t){...}
  //usage example
    GetComponent<PsoftBody>().LerpTowardsOriginal(Time.deltaTime * 8f);

  //will perform random impacts on the mesh of the psoftbody
  //can be called as is or some optional parameters can be specified
    public void RandomImpact(float impactForce = 15f, float impactScale = 1f, bool inverseImpacts = false){...}
  //usage examples
    GetComponent<PsoftBody>().RandomImpact();
      or
    GetComponent<PsoftBody>().RandomImpact(30f, 2f, true);

  //will perform an impact at a specified world point, great for use with raycasts
  //requires a world point and normal vector but some optional parameters can be specified
    public void DeformAtPoint(Vector3 worldPoint, Vector3 impactNormal, float impactForce = 15f, float impactScale = 1f, bool inverseImpacts = false){...}
  //usage example
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    if(Physics.Raycast(ray, out hit)){
        PsoftBody hitBody = hit.transform.GetComponent<PsoftBody>();
        if(hitBody)
            hitBody.DeformAtPoint(hit.point, hit.normal, 35f, 0.7f);
    }


Thanks to Charis Marangos at The Scheming Developer (schemingdeveloper.com) for providing the script
for better normal recalculation free to the public

And thanks to you, hopefully all the settings have good tooltips and the script does everything you need.
If not, feel free to send me a message :]