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
   
    private void Movement(bool active)
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
    private void Jump(bool active)
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
    private void Highjump(bool active)
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
    private void Crouch(bool active)
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
    private void Dropdown(bool active)
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
    private void Attack(bool active)
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
