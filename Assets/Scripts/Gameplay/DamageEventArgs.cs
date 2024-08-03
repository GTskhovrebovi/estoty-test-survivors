using System;

namespace Gameplay
{
    public class DamageEventArgs : EventArgs
    {
        public DamageEventArgs(Character source, Character target, float amount)
        {
            Source = source;
            Target = target;
            Amount = amount;
        }

        public Character Source { get; set; }
        public Character Target { get; set; }
        public float Amount { get; set; }
    }
}