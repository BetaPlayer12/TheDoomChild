using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorialHandle : MonoBehaviour
{
    public UIView m_movement;
    public UIView m_jump;
    public UIView m_highjump;
    public UIView m_crouch;
    public UIView m_dropdown;
    public UIView m_attack;
    public bool m_active;

    public void SetMovementTutorialVisibility(bool active)
    {
        if (active)
        {
            m_movement.Show();
        }
        else
        {
            m_movement.Hide();
        }
    }
    public void SetJumpTutorialVisibility(bool active)
    {
        if (active)
        {
            m_jump.Show();
        }
        else
        {
            m_jump.Hide();
        }
    }
    public void SetHighJumpTutorialVisibility(bool active)
    {
        if (active)
        {
            m_highjump.Show();
        }
        else
        {
            m_highjump.Hide();
        }
    }
    public void SetCrouchTutorialVisibility(bool active)
    {
        if (active)
        {
            m_crouch.Show();
        }
        else
        {
            m_crouch.Hide();
        }
    }
    public void SetDropdownTutorialVisibility(bool active)
    {
        if (active)
        {
            m_dropdown.Show();
        }
        else
        {
            m_dropdown.Hide();
        }
    }
    public void SetAttackTutorialVisibility(bool active)
    {
        if (active)
        {
            m_attack.Show();
        }
        else
        {
            m_attack.Hide();
        }
    }
}
