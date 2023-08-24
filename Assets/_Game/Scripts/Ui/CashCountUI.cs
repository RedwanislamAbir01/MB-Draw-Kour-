using TMPro;
using UnityEngine;

namespace _Game
{
    public class CashCountUI : MonoBehaviour
    {
        private const string CASH = "Cash";
        
        [SerializeField] private TextMeshProUGUI _valueText;
        
        private void Start()
        {
            PlayerInventory.OnItemUpdated += PlayerInventory_OnItemUpdated;
            UpdateValueText();
        }

        private void OnDestroy()
        {
            PlayerInventory.OnItemUpdated -= PlayerInventory_OnItemUpdated;
        }

        private void PlayerInventory_OnItemUpdated()
        {
            UpdateValueText();
        }

        private void UpdateValueText()
        {
            _valueText.text = $"{PlayerInventory.GetItemValue(CASH)}";
        }
    }
}
