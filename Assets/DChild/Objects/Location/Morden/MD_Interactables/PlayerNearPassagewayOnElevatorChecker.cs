using DChild.Gameplay;
using DChild.Gameplay.Environment;
using DChild.Gameplay.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerNearPassagewayOnElevatorChecker : MonoBehaviour
{
    public RaySensor m_sensor;
    private bool m_passagewayfound = false;
    private bool m_isdetecting = false;
    [SerializeField]
    private MovingPlatform m_elevator;
    [SerializeField]
    private UnityEvent m_passagewaydetected;
    // Start is called before the first frame update
    void Start()
    {
        m_elevator.DestinationReached += destination;
        m_elevator.DestinationChanged += change;
    }
    private void change(object sender, MovingPlatform.UpdateEventArgs eventArgs)
    {

        m_isdetecting = true;
    }

    private void destination(object sender, MovingPlatform.UpdateEventArgs eventArgs)
    {
        m_isdetecting = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (m_isdetecting == true)
        {
            m_sensor.Cast();
            RaycastHit2D[] RayGameObject = m_sensor.GetValidHits();
            for (int i = 0; i < RayGameObject.Length; i++)
            {
                if (RayGameObject[i].collider.GetComponentInParent<LocationPoster>() != null)
                {
                    m_passagewaydetected?.Invoke();
                }
               
            }
        }
    }
}
