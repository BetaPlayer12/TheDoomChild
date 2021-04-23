using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class AOETargetHandler
    {
        private List<HitboxInfo> m_processingInfo;
        private Dictionary<int, Hitbox> m_targetList;
        private TargetValidator m_targetValidator;
        private List<Hitbox> m_hitboxes;
        private List<TargetValidator.Result> m_validationResult;

        private Hitbox m_cacheHitbox;

        public AOETargetHandler()
        {
            m_processingInfo = new List<HitboxInfo>();
            m_targetList = new Dictionary<int, Hitbox>();
            m_targetValidator = new TargetValidator();
            m_hitboxes = new List<Hitbox>();

        }

        public List<Hitbox> ValidateTargets(Vector2 source, Invulnerability ignoresLevel, List<Hitbox> hitboxes)
        {
            m_processingInfo.Clear();
            m_targetList.Clear();
            for (int i = 0; i < hitboxes.Count; i++)
            {
                m_cacheHitbox = hitboxes[i];
                var hitboxID = m_cacheHitbox.GetInstanceID();
                if (m_targetList.ContainsKey(hitboxID) == false)
                {
                    m_processingInfo.Add(new HitboxInfo(m_cacheHitbox));
                    m_targetList.Add(hitboxID, m_cacheHitbox);
                }
            }
            ProcessesValidTargets(source, m_processingInfo.ToArray(), ignoresLevel);

            return m_hitboxes;
        }

       
        public List<Hitbox> GetValidTargetsOfCircleAOE(Vector2 source, float radius, int layer, Invulnerability ignoresLevel)
        {
            m_processingInfo.Clear();
            m_targetList.Clear();
            var affectedColliders = Physics2D.OverlapCircleAll(source, radius, layer);
            for (int i = 0; i < affectedColliders.Length; i++)
            {
                if (affectedColliders[i].CompareTag("DamageCollider") == false)
                {
                    var hitbox = affectedColliders[i].GetComponentInParent<Hitbox>();
                    if (hitbox != null)
                    {
                        var hitboxID = hitbox.GetInstanceID();
                        if (m_targetList.ContainsKey(hitboxID) == false)
                        {
                            m_processingInfo.Add(new HitboxInfo(hitbox));
                            m_targetList.Add(hitboxID, hitbox);
                        }
                    }
                }
            }

            ProcessesValidTargets(source, m_processingInfo.ToArray(), ignoresLevel);
            return m_hitboxes;
        }

        private void ProcessesValidTargets(Vector2 source, HitboxInfo[] infos, Invulnerability ignoresLevel)
        {
            m_validationResult = m_targetValidator.ValidateToDamage(source, infos, ignoresLevel);
            m_hitboxes.Clear();
            for (int i = 0; i < m_validationResult.Count; i++)
            {
                m_hitboxes.Add(m_targetList[m_validationResult[i].instanceID]);
            }
        }
    }
}