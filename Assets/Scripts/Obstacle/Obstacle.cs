using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerStateMachine>())
        {
            other.GetComponent<PlayerStateMachine>().OnDied();
        }
    }
}
