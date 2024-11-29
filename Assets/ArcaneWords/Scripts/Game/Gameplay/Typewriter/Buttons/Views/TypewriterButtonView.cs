using DG.Tweening;
using ObservableCollections;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneWords.Scripts.Game.Gameplay.Typewriter.Buttons.Views
{
    [RequireComponent(typeof(Button))]
    public abstract class TypewriterButtonView : MonoBehaviour
    {
        [SerializeField] private Transform _buttonTop;
        [SerializeField] private AudioClip _buttonSound;

        public Subject<Unit> OnPressed = new();
        public Subject<AudioClip> OnPressedSound = new();
        public bool IsActive => _button.interactable;
        public bool IsPressed => _isPressed;

        public float AnimateResetButtonTime { get; private set; }

        private Button _button;
        private bool _isPressed;
        private float _pressMoveValue = 10f;

        public void Init(float animateResetButtonTime)
        {
            AnimateResetButtonTime = animateResetButtonTime;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);
        }

        public abstract void InteractWithText(ObservableList<char> text);

        protected abstract void OnButtonClicked();

        public Tween Press()
        {
            if (IsActive == false || _isPressed)
                return null;
            
            return AnimatePress()
                .OnStart(() =>
                {
                    OnPressedSound.OnNext(_buttonSound);
                    _isPressed = true;
                    OnPressed.OnNext(Unit.Default);
                });
        }

        public Tween Release()
        {
            if (IsActive == false || _isPressed == false)
                return null;

            return AnimateRelease()
                .OnStart(() => _isPressed = false);
        }

        public void Deactivate()
        {
            _button.interactable = false;
            if (_isPressed == false)
                AnimatePress().OnComplete(() => _isPressed = true);
        }

        public void Activate()
        {
            _button.interactable = true;
            if (_isPressed)
                AnimateRelease().OnComplete(() => _isPressed = false);
        }

        private Tween AnimatePress(float duration = 0.2f)
        {
            var targetY = _buttonTop.transform.position.y - _pressMoveValue;

            (this as LetterButtonView)?.DisableLetterView();
            return _buttonTop.transform.DOMoveY(targetY, duration);
        }

        private Tween AnimateRelease(float duration = 0.2f)
        {
            var targetY = _buttonTop.transform.position.y + _pressMoveValue;

            (this as LetterButtonView)?.EnableLetterView();
            return _buttonTop.transform.DOMoveY(targetY, duration);
        }
    }
}