using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	private Room activeRoom;
	private Room nextRoom;
	private bool nextRoomLoaded = false;
    private UpgradeSlotMachine upgradeMachine;

    public int currentHP;
    public GameObject upgradeMachinePrefab;
    public Canvas canvas;

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
		string roomName = "Room_" + roomNum;
		SceneManager.LoadScene(roomName, LoadSceneMode.Additive);
		Scene s = SceneManager.GetSceneByName(roomName);
        Debug.Log(s.isLoaded);
		GameObject[] gameObjects = s.GetRootGameObjects();
		Debug.Log(gameObjects.Length);
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
		if (activeRoom && activeRoom.isComplete() && !nextRoomLoaded)
		{
			// TODO Only do this once.
			LoadRoom(1, activeRoom.transform.position + new Vector3(0, 20, 0));
			// TODO Check for room enter then switch activeRoom var and clear nextRoom and setNextRoom loaded to false.
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
}
