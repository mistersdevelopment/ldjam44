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
	private bool playerDead = false;
	private LoadState nextRoomState = LoadState.NONE;

	public int numberOfRooms = 2;

	public GameObject upgradeButton;
	private UpgradeSlotMachine upgradeMachine;
	public GameObject upgradeMachinePrefab;
	public Canvas canvas;
	public GameObject deathText;
	public Text floorText;
	public bool cheatMode = false;

	public AudioClip roomCompletedClip;

    private AudioSource[] sources;
	private List<float> startingVolumes;

	public AudioClip[] musicClips;

	private static GameManager _instance;
	public static GameManager Instance
	{
		get
		{
			return _instance;
		}
	}

	public int GetNextRoomNumber()
	{
		return nextRoomNumber;
	}

	private void Awake()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start()
	{
		sources = GetComponents<AudioSource>();
		startingVolumes = new List<float>();
		for (int i = 0; i < sources.Length; i++)
		{
			startingVolumes.Add(sources[i].volume);
		}
		BeginGameplay();
	}

	public void BeginGameplay()
	{
		LoadRoom(0, Vector3.zero);
		StartCoroutine(playAudioSequentially(sources[1]));
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
			if (activeRoomNumber != 0 && roomCompletedClip)
			{
				var go = new GameObject();
				var oneShot = go.AddComponent<OneShotAudio>();
				oneShot.Play(roomCompletedClip);
			}
			int roomNum = activeRoomNumber + 1;
            //if (roomNum == 1) roomNum = 40;
            if (roomNum == 41) roomNum = 1;
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
			for (int i = 0; i < sources.Length; i++)
			{
				;
				StartCoroutine(FadeOut(sources[i], 0.5f, startingVolumes[i] * 0.25f));
			}
			upgradeMachine = Instantiate(upgradeMachinePrefab, canvas.transform).GetComponent<UpgradeSlotMachine>();
		}
	}

	public void CloseUpgradeMachine()
	{
		if (upgradeMachine != null)
		{
			for (int i = 0; i < sources.Length; i++)
			{
				StartCoroutine(FadeIn(sources[i], 0.5f, startingVolumes[i]));
			}
			Destroy(upgradeMachine.gameObject);
			upgradeMachine = null;
		}
	}


    public void SpawnJackpot(GameObject jackpotMusic)
    {
        if (activeRoom)
        {
            activeRoom.StartJackpot(15, jackpotMusic);
        }
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

	static IEnumerator FadeOut(AudioSource audioSource, float fadeTime, float fadeTo)
	{
		float startVolume = audioSource.volume;

		while (audioSource.volume > fadeTo)
		{
			audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

			yield return null;
		}
	}

	static IEnumerator FadeIn(AudioSource audioSource, float fadeTime, float fadeTo)
	{
		float startVolume = audioSource.volume;

		while (audioSource.volume < fadeTo)
		{
			audioSource.volume += startVolume * Time.deltaTime / fadeTime;

			yield return null;
		}
	}

	public void ShowDeathScreen()
	{
		playerDead = true;
		StartCoroutine(DeathSequence());
	}

	IEnumerator DeathSequence()
	{
		deathText.gameObject.SetActive(true);
		deathText.transform.Find("GameOverFloorNum").GetComponent<Text>().text = "Ace reached room " + activeRoomNumber.ToString();
		yield return new WaitForSeconds(2f); // to prevent accidental skips
		yield return WaitForAnyKeyDown();
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	IEnumerator WaitForAnyKeyDown()
	{
		do
		{
			yield return null; // Once per frame
		} while (!Input.anyKeyDown);
	}

	IEnumerator playAudioSequentially(AudioSource source)
	{
		yield return null;
		for (int i = 0; i < musicClips.Length; i++)
		{
			source.clip = musicClips[i];
			source.Play();
			while (source.isPlaying)
			{
				yield return null;
			}
			Debug.Log("Next clip" + i);
		}

		// Loop forever.
		StartCoroutine(playAudioSequentially(source));
	}
}
