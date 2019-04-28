using System.Collections;
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

    private bool spinning = false;
    private bool spinEnding = false;
    public int rewardItemIndex;
    public int itemCount = 4;

    public GameObject infoText;
    public GameObject winnerText;
    public Image rewardImage;

    private float kPixelHeightPerItem = 192f;
    private int kExtraRotations = 6;
    private float kSpinSecsDuration = 5f;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        int coins = GameManager.Instance.currentHP;
        if (this.coinText != null)
        {
            coinText.text = coins.ToString();
        }
        animator.Play("SlotMachine_In");
        RandomizeSlotStart();
        UpdateLeverState();
    }

    void Show()
    {

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
                    if (rewardItemIndex != -1)
                    {
                        spinEnding = true;
                        StartCoroutine(ShowReward());
                    }
                    else
                    {
                        spinning = false;
                        spinEnding = false;
                        spendButton.gameObject.SetActive(true);
                        proxyButton.gameObject.SetActive(false);
                    }
                }
                return;
            }

            curSpinTime = Mathf.Clamp(curSpinTime + Time.deltaTime, 0f, kSpinSecsDuration);

            for (int i = 0; i < reels.Length; i++)
            {
                RawImage reel = reels[i];
                if (reel)
                {
                    float perReelDuration = Mathf.Max(kSpinSecsDuration - 0.5f * (reels.Length - i), 0f);
                    float time = Mathf.Min(curSpinTime / perReelDuration, 1f);
                    Rect uvs = reel.uvRect;
                    uvs.y = EaseOutQuad(time, 0f, reelYChange[i], 1f) / (kPixelHeightPerItem * (float)itemCount);
                    reel.uvRect = uvs;
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
                reelIndex[i] = Random.Range(0, itemCount - 1);
                Rect uvs = reel.uvRect;
                uvs.y = YCoordForSelection(reelIndex[i], 0) / (kPixelHeightPerItem * (float)itemCount);
                reel.uvRect = uvs;
            }
        }
    }

    IEnumerator ShowReward()
    {
        coinText.transform.parent.gameObject.SetActive(false);
        infoText.SetActive(false);
        winnerText.SetActive(true);
        animator.Play("SlotMachine_Winner");
        yield return new WaitForSeconds(3f);
        //GameManager.Instance.SpawnUpgradeReward(rewardItemIndex);
        ExitUpgradeScreen();
    }

    float EaseOutQuad(float time, float startValue, float change, float duration)
    {
        return -change * (time /= duration) * (time - 2) + startValue;
    }

    public void ExitUpgradeScreen()
    {
        GameManager.Instance.CloseUpgradeMachine();
    }

    void UpdateLeverState()
    {
        if (spendButton == null) return;

        int coins = GameManager.Instance.currentHP;
        spendButton.interactable = (coins > 0);
    }

    public void PullLever()
    {
        if (spinning || GameManager.Instance.currentHP <= 0)
        {
            return;
        }

        GameManager.Instance.spendCoin();
        if (this.coinText != null)
        {
            coinText.text = GameManager.Instance.currentHP.ToString();
        }
        UpdateLeverState();
        spinning = true;
        spinEnding = false;
        spendButton.gameObject.SetActive(false);
        proxyButton.gameObject.SetActive(true);

        for (int i = 0; i < reels.Length; i++)
        {
            RawImage reel = reels[i];
            if (reel)
            {
                reelIndex[i] = Random.Range(0, itemCount - 1);
                reelYChange[i] = YCoordForSelection(reelIndex[i], kExtraRotations + i*2);
            }
        }

        if (reelIndex[0] == reelIndex[1] && reelIndex[0] == reelIndex[2])
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
        return itemIndex * kPixelHeightPerItem + kPixelHeightPerItem * itemCount * rotations;
    }
}
