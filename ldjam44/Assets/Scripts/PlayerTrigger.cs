using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
	public string triggerName;

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Player>())
		{
			gameObject.SendMessageUpwards("PlayerExitedTrigger", triggerName);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Player>())
		{
			gameObject.SendMessageUpwards("PlayerEnteredTrigger", triggerName);
		}
	}
}
