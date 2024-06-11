using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
	{
	    [SerializeField] private Transform target;
		[SerializeField] private float smoothSpeed = 0.125f;
		public Vector3 offset;
		[Header("Camera bounds")]
		public Vector3 minCamerabounds;
		public Vector3 maxCamerabounds;
	

    private void Start()
	{
		if (!Restart.isCrest) 
		{ 
		GameObject targetGO = GameObject.FindGameObjectWithTag("Player");
		target = targetGO.GetComponent<Transform>();
		}

		if (Restart.isCrest)
		{
			Search();
			smoothSpeed = 100;
		}
    }

	void Search()
    {
		GameObject targetGO = GameObject.FindGameObjectWithTag("Player");
		target = targetGO.GetComponent<Transform>();
		smoothSpeed = 0.125f;
		//Restart.isCrest = false;
	}


    private void LateUpdate()
		{
			Vector3 desiredPosition = target.localPosition + offset;
			var localPosition = transform.localPosition;
			Vector3 smoothedPosition = Vector3.Lerp(localPosition, desiredPosition, smoothSpeed);
			localPosition = smoothedPosition;

			localPosition = new Vector3(
				Mathf.Clamp(localPosition.x, minCamerabounds.x, maxCamerabounds.x),
				Mathf.Clamp(localPosition.y, minCamerabounds.y, maxCamerabounds.y),
				Mathf.Clamp(localPosition.z, minCamerabounds.z, maxCamerabounds.z)
				);
			transform.localPosition = localPosition;
		}

		public void SetTarget(Transform targetToSet)
		{
			target = targetToSet;
		}
}
