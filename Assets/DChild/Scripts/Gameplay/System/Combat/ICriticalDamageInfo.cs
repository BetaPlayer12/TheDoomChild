using System.Collections.Generic;

namespace DChild.Gameplay.Combat
{
    public interface ICriticalDamageInfo
    {
        int physicalDamage { get; set; }
        int magicDamage { get; set; }
        int totalDamage { get; set; }
        List<AttackDamage> damageDealt { get; }
        int heals { get; set; }
        bool isCrit { set; }
    }
}