
using R3;

namespace ArcaneWords.Scripts.Game.Gameplay.Root.Level
{
    public interface ILevelReadyNotifier
    {
        Subject<Unit> OnReadyForLevelTransition { get; }
    }
}