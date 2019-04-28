using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

	public GameObject[] enemies;

	GameObject topDoorOpen;
	GameObject topDoorClosed;
	GameObject bottomDoorOpen;
	GameObject bottomDoorClosed;
	GameObject leftDoorOpen;
	GameObject leftDoorClosed;
	GameObject rightDoorOpen;
	GameObject rightDoorClosed;

	// Use this for initialization
	void Start()
	{
		topDoorOpen = transform.Find("Doors/Door_Top/Door_Open").gameObject;
		topDoorClosed = transform.Find("Doors/Door_Top/Door_Closed").gameObject;
		bottomDoorOpen = transform.Find("Doors/Door_Bottom/Door_Open").gameObject;
		bottomDoorClosed = transform.Find("Doors/Door_Bottom/Door_Closed").gameObject;
		leftDoorOpen = transform.Find("Doors/Door_Left/Door_Open").gameObject;
		leftDoorClosed = transform.Find("Doors/Door_Left/Door_Closed").gameObject;
		rightDoorOpen = transform.Find("Doors/Door_Right/Door_Open").gameObject;
		rightDoorClosed = transform.Find("Doors/Door_Right/Door_Closed").gameObject;
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
			SetTopDoor(true);
		}
	}

	void SetTopDoor(bool open)
	{
		topDoorOpen.SetActive(open);
		topDoorClosed.SetActive(!open);
	}

	void SetBottomDoor(bool open)
	{
		bottomDoorOpen.SetActive(open);
		bottomDoorClosed.SetActive(!open);
	}

	// Floor trigger.
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Player>())
		{
			SetBottomDoor(false);
		}
	}

}
