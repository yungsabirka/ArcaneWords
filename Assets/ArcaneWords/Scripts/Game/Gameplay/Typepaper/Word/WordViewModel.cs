using R3;
using UnityEngine;

namespace ArcaneWords.Scripts.Game.Gameplay.Typepaper.Word
{
    public class WordViewModel
    {
        public ReadOnlyReactiveProperty<bool> IsDesctiptionAvailable => _isDesctiptionAvailable;
        public ReadOnlyReactiveProperty<bool> IsWordAvailable => _isWordAvailable;
        
        public string Word => _model.Word;
        public string Description => _model.Description;
        
        private ReactiveProperty<bool> _isDesctiptionAvailable = new();
        private ReactiveProperty<bool> _isWordAvailable = new();
        private WordModel _model;

        public WordViewModel(WordModel wordModel)
        {
            _model = wordModel;
        }

        public void AllowDescription()
        {
            _isDesctiptionAvailable.Value = true;
        }

        public void OpenWord()
        {
            _isWordAvailable.Value = true;
            _isDesctiptionAvailable.Value = true;
        }
    }
}