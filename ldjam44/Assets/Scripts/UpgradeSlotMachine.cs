using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlotMachine : MonoBehaviour
{
    Animator animator;
    public Text coinText;
    public Button spendButton;

    // Use this for initialization
    void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    void Show()
    {

    }

    void Process()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            ExitUpgradeScreen();
        }
    }

    public void EnterUpgradeScreen()
    {
        int coins = GameManager.Instance.currentHP;

        this.gameObject.SetActive(true);
        if (this.coinText != null)
        {
            coinText.text = coins.ToString();
        }
        animator.Play("SlotMachine_In");
        UpdateLeverState();
    }

    public void ExitUpgradeScreen()
    {
        this.gameObject.SetActive(false);
    }

    void UpdateLeverState()
    {
        if (spendButton == null) return;

        int coins = GameManager.Instance.currentHP;
        spendButton.interactable = (coins > 0);
    }

    public void PullLever()
    {
        GameManager.Instance.spendCoin();
        if (this.coinText != null)
        {
            coinText.text = GameManager.Instance.currentHP.ToString();
        }
        UpdateLeverState();
    }
}
