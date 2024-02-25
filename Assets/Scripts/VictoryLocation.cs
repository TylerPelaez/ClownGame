using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VictoryLocation : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    public UnityEvent OnVictoryLocationReached;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (layerMask == (layerMask | (1 << other.gameObject.layer)))
        {
            OnVictoryLocationReached?.Invoke();
        }
    }
}
