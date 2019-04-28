using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class TargetCamera : MonoBehaviour
{
	public Transform target = null;
	public float smoothTime = 0.5f;

	Vector3 cameraVelocity = Vector3.zero;

	void LateUpdate()
	{
		if (target)
		{
			Vector3 newPos = Vector3.zero;
			newPos = Vector3.SmoothDamp(transform.position, target.position, ref cameraVelocity, smoothTime);
			if (target.GetComponent<Player>())
			{
				newPos.x = 0;
			}
			newPos.z = transform.position.z;
			transform.position = newPos;
		}
	}
}