﻿using System.Collections;
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
	private string activeRoomName = "";
	private Room activeRoom;
	private string nextRoomName = "";
	private Room nextRoom;
	private LoadState nextRoomState = LoadState.NONE;
    private bool firstRoom = true;

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
			activeRoomName = roomName;
			activeRoom = room;
			activeRoom.Activate();
			nextRoomState = LoadState.NONE;
		}
		else
		{
            if (activeRoomName != "Room_0")
            {
                firstRoom = false;
            }

			nextRoomName = roomName;
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
		if (activeRoom && activeRoom.isComplete() && nextRoomState == LoadState.NONE)
		{
			LoadRoom(1, activeRoom.transform.position + new Vector3(0, 10.5f, 0));
		}

        // Show and hide upgrade button
        if (!firstRoom && activeRoom && activeRoom.isComplete() && upgradeMachine == null)
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

		if (nextRoom && nextRoomState == LoadState.LOADED && nextRoom.isActive())
		{
			SceneManager.UnloadSceneAsync(activeRoomName);
			activeRoom = nextRoom;
			nextRoom = null;
			nextRoomState = LoadState.NONE;
			activeRoomName = nextRoomName;
			nextRoomName = "";
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
            prefab.transform.position = activeRoom.transform.position;
        }
    }
}
