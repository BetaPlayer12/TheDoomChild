using DChild.Gameplay;
using DChild.Gameplay.SoulSkills;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptSoulSkillNotificationHandle : MonoBehaviour
{
    [Button]
    private void NotifySkill()
    {
        GameplaySystem.gamplayUIHandle.PromptSoulSkillNotification();
    }
    private void UpdateNotification(object sender, SoulSkillAcquiredEventArgs eventArgs)
    {
        NotifySkill();
    }
    private void Start()
    {
        GameplaySystem.playerManager.player.inventory.SoulSkillItemAcquired += UpdateNotification;
    }

   
}
