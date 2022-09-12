using DChild.Gameplay;
using DChild.Gameplay.Combat.StatusAilment;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class StatusEffectSpreaderHandler : MonoBehaviour
{

    public Dictionary<StatusEffectData, float> statusEffectList = new Dictionary<StatusEffectData, float>();




    public void SetChanceToSpreadStatus(StatusEffectData statusEffect, float chance)
    {

        if (statusEffectList.ContainsKey(statusEffect))
        {
            statusEffectList[key: statusEffect] = chance;
        }

        else
        {

            statusEffectList.Add(statusEffect, chance);
        }

    }

    public void AddChanceToSpreadStatus(StatusEffectData statusEffect, float chance)
    {

        if (statusEffectList.ContainsKey(statusEffect))
        {
            statusEffectList[key: statusEffect] += chance;

        }
        else
        {
            statusEffectList.Add(statusEffect, chance);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var otherGameObject = collision.gameObject;

            if (otherGameObject.tag == "Hitbox")
            {
                var characterStatusEffectReciever = otherGameObject.GetComponentInParent<StatusEffectReciever>();

                if (characterStatusEffectReciever)
                {
                    foreach (var i in statusEffectList)
                    {
                        if (characterStatusEffectReciever.IsInflictedWith(i.Key.type))
                        {
                            characterStatusEffectReciever.ResetDuration(i.Key.type);

                        }
                        else
                        {
                            GameplaySystem.combatManager.Inflict(characterStatusEffectReciever, i.Key.type);
                        }
                    }


                }
            }
        } else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var otherGameObject = collision.gameObject;

            if (otherGameObject.tag == "Hitbox")
            {
                var characterStatusEffectReciever = otherGameObject.GetComponentInParent<StatusEffectReciever>();

                if (characterStatusEffectReciever)
                {
                    foreach (var i in statusEffectList)
                    {
                        if (characterStatusEffectReciever.IsInflictedWith(i.Key.type))
                        {
                            characterStatusEffectReciever.ResetDuration(i.Key.type);

                        }
                        else
                        {
                            GameplaySystem.combatManager.Inflict(characterStatusEffectReciever, i.Key.type);
                        }
                    }


                }
            }
        }


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var otherGameObject = collision.gameObject;

            if (otherGameObject.tag == "Hitbox")
            {
                var characterStatusEffectReciever = otherGameObject.GetComponentInParent<StatusEffectReciever>();

                if (characterStatusEffectReciever)
                {
                    foreach (var i in statusEffectList)
                    {
                        if (characterStatusEffectReciever.IsInflictedWith(i.Key.type))
                        {
                            characterStatusEffectReciever.ResetDuration(i.Key.type);

                        }
                        else
                        {
                            GameplaySystem.combatManager.Inflict(characterStatusEffectReciever, i.Key.type);
                        }
                    }


                }
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var otherGameObject = collision.gameObject;

            if (otherGameObject.tag == "Hitbox")
            {
                var characterStatusEffectReciever = otherGameObject.GetComponentInParent<StatusEffectReciever>();

                if (characterStatusEffectReciever)
                {
                    foreach (var i in statusEffectList)
                    {
                        if (characterStatusEffectReciever.IsInflictedWith(i.Key.type))
                        {
                            characterStatusEffectReciever.ResetDuration(i.Key.type);

                        }
                        else
                        {
                            GameplaySystem.combatManager.Inflict(characterStatusEffectReciever, i.Key.type);
                        }
                    }


                }
            }
        }
    }


}


