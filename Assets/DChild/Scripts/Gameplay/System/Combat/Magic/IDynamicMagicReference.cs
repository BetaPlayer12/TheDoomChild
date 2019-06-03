using System;

namespace DChild.Gameplay.Combat
{
    public interface IDynamicMagicReference
    {
        void SetMagicReference(Func<ICappedStat> handle);
    }
}