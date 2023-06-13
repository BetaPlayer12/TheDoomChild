using DChild.Gameplay;
using DChild.Gameplay.SoulSkills;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSkillNotifier : MonoBehaviour
{
    private void UpdateNotification(object sender, SoulSkillAcquiredEventArgs eventArgs)
    {
        GameplaySystem.gamplayUIHandle.notificationManager.QueueNotification(eventArgs.SoulSKill);
    }
    private void Start()
    {
        GameplaySystem.playerManager.player.inventory.SoulSkillItemAcquired += UpdateNotification;
    }
}
