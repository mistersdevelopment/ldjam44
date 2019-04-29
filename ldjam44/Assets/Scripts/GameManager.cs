using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	private enum LoadState
	{
		NONE,
		LOADING,
		LOADED
	}
	private int activeRoomNumber = -1;
	private Room activeRoom;
	private int nextRoomNumber = -1;
	private Room nextRoom;
	private LoadState nextRoomState = LoadState.NONE;

	public int numberOfRooms = 2;

	public int currentHP;
	public GameObject upgradeButton;
	private UpgradeSlotMachine upgradeMachine;
	public GameObject upgradeMachinePrefab;
	public Canvas canvas;
	public bool cheatMode = false;

	private static GameManager _instance;
	public static GameManager Instance
	{
		get
		{
			return _instance;
		}
	}

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
		nextRoomState = LoadState.LOADING;
		StartCoroutine(LoadRoomAsync(roomNum, position));
	}

	public bool IsUpgradeMachineOpen()
	{
		return upgradeMachine;
	}

	IEnumerator LoadRoomAsync(int roomNum, Vector3 position)
	{
		string roomName = "Room_" + roomNum;
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

		// Wait until the asynchronous scene fully loads
		while (!asyncLoad.isDone)
		{
			yield return null;
		}

		nextRoomState = LoadState.LOADED;
		Scene s = SceneManager.GetSceneByName(roomName);
		GameObject[] gameObjects = s.GetRootGameObjects();
		var roomObj = gameObjects[0];
		roomObj.transform.position = position;
		var room = roomObj.GetComponent<Room>();

		// Check if we are the first room ever.
		if (!activeRoom)
		{
			activeRoomNumber = roomNum;
			activeRoom = room;
			activeRoom.Activate();
			nextRoomState = LoadState.NONE;
		}
		else
		{
			nextRoomNumber = roomNum;
			nextRoom = room;
			activeRoom.SetTopDoor(true);
			activeRoom.SetBottomDoor(false);
			nextRoom.SetBottomDoor(true);
			nextRoom.SetTopDoor(false);
		}
	}

	// Update is called once per frame
	void Update()
	{
		// Load a new room if the active one is complete.
		if (activeRoom && activeRoom.isComplete() && nextRoomState == LoadState.NONE)
		{
			int roomNum = -1;
			do
			{
				roomNum = Random.Range(1, numberOfRooms);
			} while (roomNum == activeRoomNumber);
			LoadRoom(roomNum, activeRoom.transform.position + new Vector3(0, 10.5f, 0));
		}

		// Show and hide upgrade button
		if (activeRoomNumber != 0 && activeRoom && activeRoom.isComplete() && upgradeMachine == null)
		{
			if (!upgradeButton.activeSelf)
			{
				upgradeButton.SetActive(true);
			}
		}
		else
		{
			if (upgradeButton.activeSelf)
			{
				upgradeButton.SetActive(false);
			}
		}

		// Change the active room.
		if (nextRoom && nextRoomState == LoadState.LOADED && nextRoom.isActive())
		{
			SceneManager.UnloadSceneAsync("Room_" + activeRoomNumber);
			activeRoom = nextRoom;
			nextRoom = null;
			nextRoomState = LoadState.NONE;
			activeRoomNumber = nextRoomNumber;
			Debug.Log("Active Room " + activeRoomNumber);
			nextRoomNumber = -1;
		}
	}

	public void OpenUpgradeMachine()
	{
		if (upgradeMachine == null)
		{
			upgradeMachine = Instantiate(upgradeMachinePrefab, canvas.transform).GetComponent<UpgradeSlotMachine>();
		}
	}

	public void CloseUpgradeMachine()
	{
		if (upgradeMachine != null)
		{
			Destroy(upgradeMachine.gameObject);
			upgradeMachine = null;
		}
	}

	public void spendCoin()
	{
		currentHP--;
	}

	public void SpawnPowerUpReward(PowerUp prefab)
	{
		if (activeRoom)
		{
			Instantiate(prefab, activeRoom.transform);
		}
	}

	public Room GetActiveRoom()
	{
		return activeRoom;
	}
}
