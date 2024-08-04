using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "New Stat", menuName = "Gameplay/Stat", order = 1)]
    public class StatType : ScriptableObject
    {
        [field: SerializeField] public float BaseValue { get; private set; }
        [field: SerializeField] public string StatName { get; private set; }
    }
}