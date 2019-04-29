using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	private GameObject[] enemies;
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

        var characters = GetComponentsInChildren<Character>();
        enemies = new GameObject[characters.Length];
        for (int i = 0; i < characters.Length; ++i)
        {
            var character = characters[i];
           enemies[i] = character.gameObject;
        }
    }

	public bool isActive()
	{
		return active;
	}

	public bool isComplete()
	{
		return complete;
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
		BroadcastMessage("OnRoomActivate");
		SetBottomDoor(false);
		// Move the camera to the center of the room.
		Camera.main.GetComponent<TargetCamera>().target = transform;
        if (GameManager.Instance.GetNextRoomNumber() > 0)
        {
            GameManager.Instance.floorText.gameObject.SetActive(true);
            GameManager.Instance.floorText.text = "Room " + GameManager.Instance.GetNextRoomNumber().ToString();
        }
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

    public void StartJackpot(int spawnCount, GameObject jackpotMusic)
    {
        StartCoroutine(Jackpot(spawnCount, jackpotMusic));
    }

    IEnumerator Jackpot(int spawnCount, GameObject jackpotMusic)
    {
        jackpotMusic.transform.parent = this.transform;
        while (spawnCount-- > 0)
        {
            PowerUpDef pup = PowerUpManager.Instance.RandomPowerUp();
            Instantiate(pup.prefab, transform);
            yield return new WaitForSeconds(.19f);
        }
        Destroy(jackpotMusic);
    }
}
