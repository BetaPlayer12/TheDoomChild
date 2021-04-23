using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{

    public class TargetValidator
    {
        private class GroupedHitboxInfo
        {
            public int targetInstanceID { get; private set; }
            public List<HitboxInfo> vulnerableHitbox { get; private set; }
            public List<int> invulnerableHitboxIDs { get; private set; }

            public GroupedHitboxInfo()
            {
                vulnerableHitbox = new List<HitboxInfo>();
                invulnerableHitboxIDs = new List<int>();
            }

            public void Clear()
            {
                vulnerableHitbox.Clear();
                invulnerableHitboxIDs.Clear();
            }

            public void SetID(int id) => targetInstanceID = id;
            public void Add(HitboxInfo info, Invulnerability ignoresLevel)
            {
                if (info.invulnerabilityLevel > ignoresLevel)
                {
                    invulnerableHitboxIDs.Add(info.targetID);
                }
                else
                {
                    vulnerableHitbox.Add(info);
                }
            }
        }

        public struct Result
        {
            public Result(int m_instanceID, bool isInvulnerable) : this()
            {
                this.instanceID = m_instanceID;
                this.isInvulnerable = isInvulnerable;
            }

            public int instanceID { get; }
            public bool isInvulnerable { get; }
        }

        private List<Result> m_results;
        private Dictionary<int, GroupedHitboxInfo> m_categorizedInfo;
        private RaycastHit2D[] m_hitbuffer;

        public TargetValidator()
        {
            m_results = new List<Result>();
            m_categorizedInfo = new Dictionary<int, GroupedHitboxInfo>();
        }

        public List<Result> ValidateToDamage(Vector2 source, HitboxInfo[] infos, Invulnerability ignoresLevel)
        {
            m_results.Clear();
            CategorizeInfos(infos, ignoresLevel);

            foreach (var info in m_categorizedInfo.Values)
            {
                if (info.invulnerableHitboxIDs.Count == 0)
                {
                    //Validate targets with only 1 hitboxinfo and it should be vulnerable
                    for (int i = 0; i < info.vulnerableHitbox.Count; i++)
                    {
                        m_results.Add(new Result(info.vulnerableHitbox[i].targetID, false));
                    }
                }
                else
                {
                    //Check if others is valid by raycasting from source to a vulnerable 
                    //then making sure that there is no other invulnerable colliders
                    for (int i = 0; i < info.vulnerableHitbox.Count; i++)
                    {
                        bool isInvulnerable = false;
                        var hitboxInfo = info.vulnerableHitbox[i];
                        var directionToHitbox = hitboxInfo.position - source;
                        m_hitbuffer = Raycaster.CastAll(source, directionToHitbox.normalized, directionToHitbox.magnitude);
                        for (int j = 0; j < m_hitbuffer.Length; j++)
                        {
                            if (info.invulnerableHitboxIDs.Contains(m_hitbuffer[j].collider.GetInstanceID()))
                            {
                                isInvulnerable = true;
                                break;
                            }
                        }
                        m_results.Add(new Result(info.vulnerableHitbox[i].targetID, isInvulnerable));
                    }
                }
            }
            return m_results;
        }

        private void CategorizeInfos(HitboxInfo[] infos, Invulnerability ignoresLevel)
        {
            m_categorizedInfo.Clear();
            //Categorize Hitboxes to targetInstances
            for (int i = 0; i < infos.Length; i++)
            {
                var info = infos[i];

                if (m_categorizedInfo.ContainsKey(info.targetID))
                {
                    m_categorizedInfo[info.targetID].Add(info, ignoresLevel);
                }
                else
                {
                    var groupedInfo = new GroupedHitboxInfo();
                    groupedInfo.SetID(info.targetID);
                    groupedInfo.Add(info, ignoresLevel);
                    m_categorizedInfo.Add(info.targetID, groupedInfo);
                }
            }
        }
    }
}