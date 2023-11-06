using TMPro;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour {
    public static PlayerInventoryManager Instance { get; private set; }

    [SerializeField] private TMP_Text moneyText; // Assign this via the inspector

    private int playerMoney;
    public int PlayerMoney {
        get => playerMoney;
        private set {
            if (playerMoney != value) {
                playerMoney = value;
                UpdateMoneyDisplay(); // Update the UI when money changes
            }
        }
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist singleton instance across scenes.
        }
        UpdateMoneyDisplay(); // Initial UI update on awake
    }

    public void AddMoney(int amount) {
        PlayerMoney += amount; // Set operation triggers the UI update
    }

    public bool SpendMoney(int amount) {
        if (amount <= PlayerMoney) {
            PlayerMoney -= amount; // Set operation triggers the UI update
            return true;
        } else {
            Debug.LogWarning("Not enough money to spend!");
            return false;
        }
    }

    // Method to update money text when money changes
    private void UpdateMoneyDisplay() {
        if (moneyText != null) { // Check to ensure the text component is assigned.
            moneyText.text = $"Money: {PlayerMoney}";
        }
    }
}
