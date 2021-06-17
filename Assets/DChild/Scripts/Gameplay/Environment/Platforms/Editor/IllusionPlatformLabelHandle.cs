using DChild.Gameplay.Environment;
using System.Collections.Generic;
using UnityEngine;

namespace DChildEditor.Gameplay.Environment
{
    public class IllusionPlatformLabelHandle : ObjectLabelHandle<IllusionPlatform>
    {
        public IllusionPlatformLabelHandle(Dictionary<IllusionPlatform, string> labelPair, GUIStyle labelStyle) : base(labelPair, labelStyle)
        {
        }

        public override void SetObjectsToLabel(params IllusionPlatform[] objects)
        {
            m_labelPair.Clear();
            int index = 0;
            for (int i = 0; i < objects.Length; i++)
            {
                var platform = objects[i];
                if (platform != null)
                {
                    if (platform is CompositeIllusionPlatform)
                    {
                        LabelCompositeIllusionPlatform((CompositeIllusionPlatform)platform,ref index);
                    }
                    else
                    {
                        UpdateLabel(platform, index);
                    }
                }
                index++;
            }
        }

        private void UpdateLabel(IllusionPlatform platform, int index)
        {
            if (m_labelPair.ContainsKey(platform))
            {
                m_labelPair[platform] += $"\n{index + 1}";
            }
            else
            {
                m_labelPair.Add(platform, $"{index + 1}");
            }
        }

        private void LabelCompositeIllusionPlatform(CompositeIllusionPlatform compositeIllusionPlatform, ref int index)
        {
            foreach (var platform in compositeIllusionPlatform.list)
            {
                UpdateLabel(platform, index);
                index++;
            }
        }
    }

}