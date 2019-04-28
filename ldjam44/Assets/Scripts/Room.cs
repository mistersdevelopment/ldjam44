using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	public GameObject[] enemies;
	public Transform startPos;

	bool complete = false;
	bool active = false;

	GameObject topDoorOpen;
	GameObject topDoorClosed;
	GameObject bottomDoorOpen;
	GameObject bottomDoorClosed;

	// Use this for initialization
	void Start()
	{
		topDoorOpen = transform.Find("Doors/Door_Top/Door_Open").gameObject;
		topDoorClosed = transform.Find("Doors/Door_Top/Door_Closed").gameObject;
		bottomDoorOpen = transform.Find("Doors/Door_Bottom/Door_Open").gameObject;
		bottomDoorClosed = transform.Find("Doors/Door_Bottom/Door_Closed").gameObject;

		// Sleep all the enemies.
		DeactivateEnemies();
	}

	public bool isActive()
	{
		return active;
	}

	public bool isComplete()
	{
		return complete;
	}

	private void ActivateEnemies()
	{
		for (int i = 0; i < enemies.Length; i++)
		{
			// Sleep all the enemies.
			if (enemies[i])
			{
				enemies[i].SetActive(true);
			}
		}
	}

	private void DeactivateEnemies()
	{
		for (int i = 0; i < enemies.Length; i++)
		{
			// Sleep all the enemies.
			if (enemies[i])
			{
				enemies[i].SetActive(false);
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		int deadCount = 0;
		for (int i = 0; i < enemies.Length; i++)
		{
			if (!enemies[i])
			{
				deadCount++;
			}
		}

		if (deadCount >= enemies.Length)
		{
			complete = true;
		}
	}

	public void SetTopDoor(bool open)
	{
		topDoorOpen.SetActive(open);
		topDoorClosed.SetActive(!open);
	}

	public void SetBottomDoor(bool open)
	{
		bottomDoorOpen.SetActive(open);
		bottomDoorClosed.SetActive(!open);
	}

	public void Activate()
	{
		active = true;
		var player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
		// Move the player to the start pos.
		player.GetComponent<Rigidbody2D>().position = startPos.position;
		// Awaken all the enemies.
		Invoke("ActivateEnemies", 2);
		SetBottomDoor(false);
		// Move the camera to the center of the room.
		Camera.main.GetComponent<TargetCamera>().target = transform;
	}

	public void PlayerEnteredTrigger(string name)
	{
		if (name == "EntryPad")
		{
			Activate();
		}
	}

	public void PlayerExitedTrigger(string name)
	{
		// Do nothing.
	}
}
