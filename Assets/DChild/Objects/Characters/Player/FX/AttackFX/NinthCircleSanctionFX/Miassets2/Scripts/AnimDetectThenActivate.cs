using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class AnimDetectThenActivate : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation _skeletonAnimation; // Reference to the SkeletonAnimation component
    [SerializeField] private string _animationName; // Name of the Spine animation to detect
    [SerializeField] private GameObject[] _gameObjectsToActivate; // Array of game objects to activate when animation is detected
    [SerializeField] private float _activationDelayTime; // Delay time before activating the game objects
    [SerializeField] private float _deactivationDelayTime; // Delay time before deactivating the game objects again

    private bool _animationDetected = false; // Flag to indicate if the Spine animation has been detected

    public bool AnimationDetected { get; internal set; }
    public IEnumerable<GameObject> GameObjectsToActivate { get; internal set; }

    private IEnumerator Start()
    {
        while (true) // Infinite loop to keep detecting the Spine animation
        {
            if (_skeletonAnimation.AnimationName == _animationName && !_animationDetected) // If the Spine animation is detected and the flag is false
            {
                _animationDetected = true; // Set the flag to true

                yield return new WaitForSeconds(_activationDelayTime); // Wait for the activation delay time

                ActivateGameObjects(); // Activate the game objects

                yield return new WaitForSeconds(_deactivationDelayTime); // Wait for the deactivation delay time

                DeactivateGameObjects(); // Deactivate the game objects

                _animationDetected = false; // Reset the flag
            }

            yield return null; // Yield once per frame
        }
    }

    public void ActivateGameObjects()
    {
        foreach (GameObject gameObject in _gameObjectsToActivate) // Loop through the game objects to activate
        {
            gameObject.SetActive(true); // Activate each game object
        }
    }

    public void DeactivateGameObjects()
    {
        foreach (GameObject gameObject in _gameObjectsToActivate) // Loop through the game objects to deactivate
        {
            gameObject.SetActive(false); // Deactivate each game object
        }
    }
}
