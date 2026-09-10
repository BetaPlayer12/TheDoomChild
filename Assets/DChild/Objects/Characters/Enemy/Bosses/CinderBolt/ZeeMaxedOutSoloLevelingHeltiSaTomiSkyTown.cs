using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ZeeMaxedOutSoloLevelingHeltiSaTomiSkyTown : MonoBehaviour
{
    private BasicHealth m_zeeHealth;
    private Magic m_shadowGauge;
    private PlayerSkills m_skills;
    private CombatArts m_combatSkills;
    private List<CombatArt> m_allCombatSkills = new List<CombatArt>();
    private PrimarySkill m_listOfSkills = PrimarySkill.All;

    private enum ListOfCombatArt
    {
        ReaperHarvest,
        SovereignImpale,
        HellTrident,
        FoolsVerdict,
        SoulfireBlast,
        EdgedFury,
        BackDiver,
        Barrier,
        DiagonalSwordDash,
        ChampionsUprising,
        LightningSpear,
        IcarusWings,
        TeleportingSkull,
        AirSlashRange,
    }
    private void Awake()
    {
        var playerSkills = GameObject.Find("Progression");
        m_skills = playerSkills.GetComponent<PlayerSkills>();
        m_combatSkills = playerSkills.GetComponentInChildren<CombatArts>();
        var zeeHealth = GameObject.Find("Zee").GetComponentInChildren<BasicHealth>();
        var zeeGauge = GameObject.Find("Zee").GetComponentInChildren<Magic>();
        m_zeeHealth = zeeHealth;
        m_shadowGauge = zeeGauge;
    }
    private void Start()
    {
        m_skills.SetSkillStatus(m_listOfSkills, true);
        InitializeCombatArts();
        for (int i = 0; i < m_allCombatSkills.Count; i++)
        {
            m_combatSkills.SetAbilityLevel(m_allCombatSkills[i], 200);
        }
        StartCoroutine(SetZeeHealthWithDelay());
        StartCoroutine(SetZeeGaugeWithDelay());
    }
    private void InitializeCombatArts()
    {
        foreach (CombatArt combatArt in System.Enum.GetValues(typeof(ListOfCombatArt)))
            m_allCombatSkills.Add(combatArt);
    }
    private IEnumerator SetZeeHealthWithDelay()
    {
        yield return new WaitForEndOfFrame();
        m_zeeHealth.SetMaxValue(2147483647);
        m_zeeHealth.ResetValueToMax();
    }
    private IEnumerator SetZeeGaugeWithDelay()
    {
        yield return new WaitForEndOfFrame();
        m_shadowGauge.SetMaxValue(2147483647);
        m_shadowGauge.ResetValueToMax();
    }
}
