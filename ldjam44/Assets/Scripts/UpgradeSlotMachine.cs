﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlotMachine : MonoBehaviour
{
	Animator animator;
	public Text coinText;
	public Image proxyButton;
	public Button spendButton;
	public RawImage[] reels = new RawImage[3];

	float curSpinTime = 0f;
	int[] reelIndex = new int[3];
	float[] reelYChange = new float[3];

	private Character playa;
	private bool spinning = false;
	private bool spinEnding = false;
	public int rewardItemIndex;

	public GameObject infoText;
	public GameObject winnerText;
	public Image rewardImage;

	private float kPixelHeightPerItem = 192f;
	private int kExtraRotations = 6;
	private float kSpinSecsDuration = 5f;
	private int kItemCount;

	private AudioSource spinningSource;
    private AudioSource reelHitSource;
    private AudioSource winSource;
    private AudioSource loseSource;

	// Use this for initialization
	void Start()
	{
		playa = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
		kItemCount = PowerUpManager.Instance.lootTable.Length;

		animator = GetComponent<Animator>();
		int coins = (int)playa.currentHealth;
		if (this.coinText != null)
		{
			coinText.text = coins.ToString();
		}
		animator.Play("SlotMachine_In");
		RandomizeSlotStart();
		UpdateLeverState();

		spinningSource = GetComponent<AudioSource>();
        reelHitSource = transform.Find("Reel_Hit").GetComponent<AudioSource>();
        winSource = transform.Find("Win").GetComponent<AudioSource>();
        loseSource = transform.Find("Lose").GetComponent<AudioSource>();
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			ExitUpgradeScreen();
		}

		if (spinning)
		{
			if (curSpinTime >= kSpinSecsDuration)
			{
				if (!spinEnding)
				{
					spinningSource.Stop();
					if (rewardItemIndex != -1)
					{
						spinEnding = true;
						winSource.Play();
						StartCoroutine(ShowReward());
					}
					else
					{
						spinning = false;
						spinEnding = false;
						spendButton.gameObject.SetActive(true);
						proxyButton.gameObject.SetActive(false);
						loseSource.Play();
					}
				}
				return;
			}

			var prevSpinTime = curSpinTime;
			curSpinTime = Mathf.Clamp(curSpinTime + Time.deltaTime, 0f, kSpinSecsDuration);

			for (int i = 0; i < reels.Length; i++)
			{
				RawImage reel = reels[i];
				if (reel)
				{
					float perReelDuration = Mathf.Max(kSpinSecsDuration - 1f * (reels.Length - i), 0f);
					float time = Mathf.Min(curSpinTime / perReelDuration, 1f);
					Rect uvs = reel.uvRect;
					uvs.y = EaseOutQuad(time, 0f, reelYChange[i], 1f) / (kPixelHeightPerItem * (float)kItemCount);
					reel.uvRect = uvs;

					if (prevSpinTime < perReelDuration && curSpinTime >= perReelDuration)
					{
                        reelHitSource.pitch += 0.4f;
						if (i == reels.Length - 1)
						{
							spinningSource.Stop();
						}
						reelHitSource.Play();
					}
				}
			}
		}
	}

	void RandomizeSlotStart()
	{
		for (int i = 0; i < reels.Length; i++)
		{
			RawImage reel = reels[i];
			if (reel)
			{
				reelIndex[i] = Random.Range(0, kItemCount);
				Rect uvs = reel.uvRect;
				uvs.y = YCoordForSelection(reelIndex[i], 0) / (kPixelHeightPerItem * (float)kItemCount);
				reel.uvRect = uvs;
			}
		}
	}

	IEnumerator ShowReward()
	{
		PowerUpDef pup = PowerUpManager.Instance.lootTable[rewardItemIndex];
		coinText.transform.parent.gameObject.SetActive(false);
		infoText.SetActive(false);
		winnerText.SetActive(true);
		winnerText.GetComponent<Text>().text = pup.name + " Upgrade Won!";
		rewardImage.sprite = pup.icon;
		animator.Play("SlotMachine_Winner");
		yield return new WaitForSeconds(3f);
		GameManager.Instance.SpawnPowerUpReward(pup.prefab);
		GameManager.Instance.CloseUpgradeMachine();
	}

	float EaseOutQuad(float time, float startValue, float change, float duration)
	{
		return -change * (time /= duration) * (time - 2) + startValue;
	}

	public void ExitUpgradeScreen()
	{
		if (spinning)
		{
			return;
		}

		GameManager.Instance.CloseUpgradeMachine();
	}

	void UpdateLeverState()
	{
		if (spendButton == null) return;

		int coins = (int)playa.currentHealth;
		spendButton.interactable = (coins > 1);
	}

	public void PullLever()
	{
		if (spinning || (int)playa.currentHealth <= 1)
		{
			return;
		}


		playa.GetComponent<Character>().ModifyHealth(-1);
		if (this.coinText != null)
		{
			coinText.text = ((int)playa.currentHealth).ToString();
		}
		UpdateLeverState();
		spinningSource.Play();
        reelHitSource.pitch = 1.25f;
		spinning = true;
		spinEnding = false;
		spendButton.gameObject.SetActive(false);
		proxyButton.gameObject.SetActive(true);

		int pityReward = -1;
		if (PowerUpManager.Instance.spinsSinceUpgrade >= PowerUpManager.Instance.pityTimer - 1)
		{
			pityReward = Random.Range(0, kItemCount);
			PowerUpManager.Instance.spinsSinceUpgrade = 0;
		}
		else
		{
			PowerUpManager.Instance.spinsSinceUpgrade++;
		}

		for (int i = 0; i < reels.Length; i++)
		{
			RawImage reel = reels[i];
			if (reel)
			{
				if (pityReward != -1)
				{
					reelIndex[i] = pityReward;
				}
				else
				{
					reelIndex[i] = Random.Range(0, kItemCount);
				}
				reelYChange[i] = YCoordForSelection(reelIndex[i], kExtraRotations + i * 2);
			}
		}

		if (GameManager.Instance.cheatMode || (reelIndex[0] == reelIndex[1] && reelIndex[0] == reelIndex[2]))
		{
			rewardItemIndex = reelIndex[0];
		}
		else
		{
			rewardItemIndex = -1;
		}
		curSpinTime = 0f;
	}

	float YCoordForSelection(int itemIndex, int rotations)
	{
		return itemIndex * kPixelHeightPerItem + kPixelHeightPerItem * kItemCount * rotations;
	}
}
