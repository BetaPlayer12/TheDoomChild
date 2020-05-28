﻿/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using DChild.Gameplay.Environment.Interractables;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class WhipColliderDamage : ColliderDamage
    {
        private const string ENVIRONMENT_LAYER = "Environment";

        private static List<Collider2D> m_canPassthroughList;

        public static void InitializePassables(IEnumerable<Collider2D> list)
        {
            if (m_canPassthroughList == null)
            {
                m_canPassthroughList = new List<Collider2D>();
            }
            else
            {
                m_canPassthroughList.Clear();
            }

            m_canPassthroughList.AddRange(list);
        }

        protected override bool IsValidToHit(Collider2D collision)
        {
            // Cannot Hit Enemies on the other side of grills

            if (m_canDetectInteractables)
            {
                if (collision.TryGetComponentInParent(out IHitToInteract interactable))
                {
                    var position = transform.position;
                    var direction = collision.bounds.center - position;
                    var castedAll = Raycaster.CastAll(position, direction.normalized, direction.magnitude);
                    for (int i = 0; i < castedAll.Length; i++)
                    {
                        if (castedAll[i].collider == collision)
                        {
                            break;
                        }
                        else if (m_canPassthroughList.Contains(castedAll[i].collider) == false)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            }
            else
            {
                return Raycaster.SearchCast(transform.position, collision.bounds.center, LayerMask.GetMask(ENVIRONMENT_LAYER), out RaycastHit2D[] buffer);
            }
        }
    }
}
