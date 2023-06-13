using UnityEngine;
using Sirenix.OdinInspector;
using System.Text;
using System.Collections;
using System.Linq;

namespace DChildEditor.DesignTool.LevelMap.HackNPlan
{

    [RequireComponent(typeof(LevelMapElementGroup))]
    public class LevelElementListLogger : HackNPlanLogger
    {
        [SerializeField, ValueDropdown("GetDesignID")]
        private string m_designElementID;
        [SerializeField]
        private bool m_shortenIDListing;


        private static StringBuilder LogBuilder = new StringBuilder();

        public string designElementID => m_designElementID;

        private IEnumerable GetDesignID() => HackNPlanManager.instance.GetDesignElements();
        public override void GenerateLog(StringBuilder stringBuilder)
        {
            LogBuilder.Clear();
            var elementGroup = GetComponent<LevelMapElementGroup>();
            var elements = GetComponentsInChildren<LevelMapElement>();

            if (elements.Length <= 0)
                return;

            ListElementIDs(elements);

            if (elements.Length > 1)
            {
                LogBuilder.Insert(0, $"- Have {elements.Length} {m_designElementID} ");
                LogBuilder.Append(" scattered at certain areas.\n");
            }
            else
            {
                LogBuilder.Insert(0, $"- Has a {m_designElementID} ");
                LogBuilder.Append(" at a specific place.\n");
            }


            stringBuilder.Append(LogBuilder);

            Debug.Log("Log Generated:\n" + LogBuilder.ToString());
        }

        private void ListElementIDs(LevelMapElement[] elements)
        {
            if (m_shortenIDListing)
            {
                elements.OrderBy(x => x.index);
                var elementIndexesList = elements.Select(x => x.index).ToList();
                var elementIndexes = elementIndexesList.ToArray();

                var startingElementIndex = 0;
                var currentIndex = elements[startingElementIndex].index;
                currentIndex++;

                for (int i = 1; i < elements.Length; i++)
                {
                    var element = elements[i];
                    if (i == elements.Length - 1)
                    {
                        if (currentIndex != element.index)
                        {
                            CreateShortenedIDList(elements[startingElementIndex], elements[i - 1]);
                            LogBuilder.Append(" and ");
                            LogBuilder.Append(HackNPlanUtils.EmphasizeGuideIDs($"{elements[i].elementName}"));
                        }
                        else
                        {
                            CreateShortenedIDList(elements[startingElementIndex], elements[i]);
                        }
                    }
                    else
                    {
                        if (currentIndex != element.index)
                        {
                            CreateShortenedIDList(elements[startingElementIndex], elements[i - 1]);
                            LogBuilder.Append(" and ");

                            startingElementIndex = i;
                            currentIndex = element.index;
                        }
                    }

                    currentIndex++;
                }

                //var smallest = Mathf.Min(elementIndexes);
                //var smallestIndex = elementIndexesList.FindIndex(x => x == smallest);
                //LogBuilder.Append($"{elements[smallestIndex].elementName} to ");

                //var largest = Mathf.Max(elementIndexes);
                //var largesttIndex = elementIndexesList.FindIndex(x => x == largest);
                //LogBuilder.Append(elements[largesttIndex].elementName);
                //HackNPlanUtils.EmphasizeGuideIDs(LogBuilder);
            }
            else
            {
                HackNPlanUtils.EmphasizeGuideIDs(LogBuilder, elements);
            }

            void CreateShortenedIDList(LevelMapElement startingElement, LevelMapElement endElement)
            {
                LogBuilder.Append(HackNPlanUtils.EmphasizeGuideIDs($"{startingElement.elementName} to {endElement.elementName}"));
            }
        }
    }
}