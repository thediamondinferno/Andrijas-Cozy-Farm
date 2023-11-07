using TMPro;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour {
    public static PlayerInventoryManager Instance { get; private set; }

    [SerializeField] private TMP_Text moneyText;

    private int playerMoney;
    public int PlayerMoney {
        get => playerMoney;
        private set {
            if (playerMoney != value) {
                playerMoney = value;
                UpdateMoneyDisplay();
            }
        }
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        UpdateMoneyDisplay();
    }

    public void AddMoney(int amount) {
        PlayerMoney += amount;
    }

    public bool SpendMoney(int amount) {
        if (amount <= PlayerMoney) {
            PlayerMoney -= amount;
            return true;
        } else {
            Debug.LogWarning("Not enough money to spend!");
            return false;
        }
    }

    private void UpdateMoneyDisplay() {
        if (moneyText != null) { 
            moneyText.text = $"Money: {PlayerMoney}";
        }
    }
}
