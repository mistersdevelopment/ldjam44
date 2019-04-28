using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private Room activeRoom;
	private Room nextRoom;
	private bool nextRoomLoaded = false;

	private static GameManager _instance;
	public static GameManager Instance
	{
		get
		{
			return _instance;
		}
	}

    public int currentHP;
    public UpgradeSlotMachine slotMachine;

	private void Awake()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start()
	{
		BeginGameplay();
	}

	public void BeginGameplay()
	{
		LoadRoom(0, Vector3.zero);
	}

	void LoadRoom(int roomNum, Vector3 position)
	{
		string roomName = "Room_" + roomNum;
		SceneManager.LoadScene(roomName, LoadSceneMode.Additive);
		Scene s = SceneManager.GetSceneByName(roomName);
		GameObject[] gameObjects = s.GetRootGameObjects();
		var roomObj = gameObjects[0];
		roomObj.transform.position = position;
		var room = roomObj.GetComponent<Room>();

		if (!activeRoom)
		{
			activeRoom = room;
		}
		else
		{
			nextRoom = room;
			nextRoomLoaded = true;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (activeRoom.isComplete() && !nextRoomLoaded)
		{
			// TODO Only do this once.
			LoadRoom(1, activeRoom.transform.position + new Vector3(0, 20, 0));
			// TODO Check for room enter then switch activeRoom var and clear nextRoom and setNextRoom loaded to false.
		}
	}

    public void ToggleUpgradesSlots()
    {
        if (slotMachine)
        {
            slotMachine.EnterUpgradeScreen();
        }
    }

    public void spendCoin()
    {
        currentHP--;
    }
}
