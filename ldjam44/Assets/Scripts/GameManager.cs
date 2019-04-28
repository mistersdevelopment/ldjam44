using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

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
    void Start () {
		BeginGameplay();
	}

    public void BeginGameplay()
    {
        SceneManager.LoadScene("Room_0", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
