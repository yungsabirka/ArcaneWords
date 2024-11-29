using ArcaneWords.Scripts.Game.Gameplay.Typepaper;
using R3;
using Zenject;

namespace ArcaneWords.Scripts.Game.GameRoot.Bank
{
    public class BankViewModel
    {
        public ReadOnlyReactiveProperty<int> Coins => _bankModel.Coins;
        public ReadOnlyReactiveProperty<int> Bulbs => _bankModel.Bulbs;
        public Subject<Unit> OnSpendBulbsFailed = new();
        
        private BankModel _bankModel;

        [Inject]
        private void Construct(BankModel bankModel)
        {
            _bankModel = bankModel;
        }
        
        public bool TryToSpendCoins(int coins)
        {
            return _bankModel.TryToSpendCoins(coins);
        }

        public bool TryToSpendBulbs(int bulbs)
        {
            if (_bankModel.TryToSpendBulbs(bulbs))
                return true;
            
            OnSpendBulbsFailed.OnNext(Unit.Default);
            return false;
        }

        public void AddCoins(int coins)
        {
            _bankModel.AddCoins(coins);
        }

        public void AddBulbs(int bulbs)
        {
            _bankModel.AddBulbs(bulbs);
        }

        public void SaveBankData()
        {
            _bankModel.SaveBankData();
        }
    }
}