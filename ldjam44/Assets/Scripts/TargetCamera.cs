using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class TargetCamera : MonoBehaviour
{
	public Transform target = null;
	float speed = 50f;

	Vector3 cameraVelocity = Vector3.zero;

	void LateUpdate()
	{
		if (target)
		{
			Vector3 newPos = Vector3.zero;
			newPos = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
			newPos.z = transform.position.z;
			transform.position = newPos;
		}
	}
}