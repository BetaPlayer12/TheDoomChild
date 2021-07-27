using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
	public interface IPlayerWallStickPlatformReaction
	{
		void ReactToPlayerWallStick(Character player);
		void ReactToPlayerWallUnstick(Character player);
	}
}