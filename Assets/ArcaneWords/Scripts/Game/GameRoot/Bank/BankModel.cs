using System;
using System.IO;
using R3;
using UnityEngine;

namespace ArcaneWords.Scripts.Game.GameRoot.Bank
{
    [Serializable]
    public class BankModel
    {
        [SerializeField] private int _coinsValue;
        [SerializeField] private int _bulbsValue;
        
        public ReadOnlyReactiveProperty<int> Coins => _coins;
        public ReadOnlyReactiveProperty<int> Bulbs => _bulbs;
        public bool Inited { get; private set; }

        private ReactiveProperty<int> _coins = new();
        private ReactiveProperty<int> _bulbs = new();

        private string FilePath => Path.Combine(Application.persistentDataPath, "BankData.json");

        public void Init()
        {
            if (File.Exists(FilePath))
            {
                LoadBankData();
            }
            else
            {
                _coins.Value = 100;
                _bulbs.Value = 10;
                SaveBankData();   
            }

            Inited = true;
        }

        public void AddCoins(int amount)
        {
            if (amount < 0) 
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));
            
            _coins.Value += amount;
        }

        public bool TryToSpendCoins(int amount)
        {
            if (amount < 0) 
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));
            
            if (_coins.Value < amount) 
                return false;

            _coins.Value -= amount;
            return true;
        }

        public void AddBulbs(int amount)
        {
            if (amount < 0) 
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));
            
            _bulbs.Value += amount;
        }

        public bool TryToSpendBulbs(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));
            
            if (_bulbs.Value < amount) 
                return false;

            _bulbs.Value -= amount;
            return true;
        }

        public void SaveBankData()
        {
            try
            {
                _coinsValue = _coins.Value;
                _bulbsValue = _bulbs.Value;

                var jsonData = JsonUtility.ToJson(this, true);
                File.WriteAllText(FilePath, jsonData);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        public void LoadBankData()
        {
            try
            {
                var jsonData = File.ReadAllText(FilePath);
                JsonUtility.FromJsonOverwrite(jsonData, this);
                
                _coins.Value = _coinsValue;
                _bulbs.Value = _bulbsValue;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
    }
}