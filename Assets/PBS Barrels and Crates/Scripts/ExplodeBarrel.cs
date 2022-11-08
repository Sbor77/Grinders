// Note: This file is for demonstration purposes only. This pack is built to provide you with models, not scripts.
// While you may feel free to use this file alongside these models, please understand that it is not what this package
// is all about. Thank you.

using UnityEngine;
using System.Collections;

public class ExplodeBarrel : MonoBehaviour 
{
	[SerializeField] public float explosionForce = 2000f;
	[SerializeField] public float explosionRadius = 50f;
	[SerializeField] public float upForceMin = 0.0f;
	[SerializeField] public float upForceMax = 0.5f;
	[SerializeField] public float lifeTime = 5.0f;
	[SerializeField] public bool autoDestroy = false;

	void Start () 
	{
		// explode the barrel, called automatically when instantiated b/c it is in the start method
		// you may want to move this to a different peice of code...
		//Explode ();

		// auto destruction setup
		if (autoDestroy)
			Destroy(gameObject, lifeTime);

		Invoke(nameof(Explode), 3f);
	}

	public void Explode()
	{
		Vector3 centerPos = transform.position;

		foreach (Transform child in transform) 
		{
			Rigidbody rb = child.gameObject.GetComponent<Rigidbody> ();
			rb.AddExplosionForce (explosionForce, centerPos, explosionRadius, Random.Range(upForceMin, upForceMax), ForceMode.Impulse);

			print(explosionForce);
		}
	}
}
