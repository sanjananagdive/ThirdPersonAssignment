using System.Collections;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public float activeDuration = 2f; // Time in seconds for which the child objects are active
    public float inactiveDuration = 2f; // Time in seconds for which the child objects are inactive
    private bool isActive;

    private void Start()
    {
        // Initialize the state and start the coroutine
        isActive = true; // Assuming we start with child objects active
        StartCoroutine(ToggleChildrenCoroutine());
    }

    private IEnumerator ToggleChildrenCoroutine()
    {
        while (true) // Continuously loop
        {
            if (isActive)
            {
                // Disable all child objects and wait for the inactive duration
                SetChildrenActive(false);
                yield return new WaitForSeconds(inactiveDuration);
                isActive = false;
            }
            else
            {
                // Enable all child objects and wait for the active duration
                SetChildrenActive(true);
                yield return new WaitForSeconds(activeDuration);
                isActive = true;
            }
        }
    }

    private void SetChildrenActive(bool state)
    {
        // Loop through each child and set its active state
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(state);
        }
    }
}
