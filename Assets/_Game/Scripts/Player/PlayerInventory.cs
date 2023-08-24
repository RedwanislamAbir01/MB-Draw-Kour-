using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game
{
    public static class PlayerInventory
    {
        public static event Action OnItemUpdated;

        private const string CASH = "Cash";
        
        private static Dictionary<string, int> _inventoryDict;

        static PlayerInventory()
        {
            _inventoryDict = new Dictionary<string, int>();
            
            SetItemValue(CASH, PlayerPrefs.GetInt(CASH, 0));
        }

        public static void AddItem(string key, int value)
        {
            if (_inventoryDict.ContainsKey(key))
            {
                _inventoryDict[key] += value;
            }
            else
            {
                _inventoryDict[key] = value;
            }
            
            PlayerPrefs.SetInt(key, _inventoryDict[key]);
            
            OnItemUpdated?.Invoke();
        }

        public static void RemoveItem(string key, int value)
        {
            if (_inventoryDict.ContainsKey(key))
            {
                _inventoryDict[key] -= value;
                
                PlayerPrefs.SetInt(key, _inventoryDict[key]);
                
                OnItemUpdated?.Invoke();
            }
        }

        public static int GetItemValue(string key)
        {
            return _inventoryDict.ContainsKey(key) ? _inventoryDict[key] : 0;
        }

        private static void SetItemValue(string key, int value)
        {
            _inventoryDict[key] = value;
        }
    }
}