using UnityEngine;
using System.Collections;

public class TankPart : MonoBehaviour {

    Tank tank;

    void Awake()
    {
        tank = GetComponentInParent<Tank>();
    }

    internal void TakeDamage(float damage)
    {
        if (tank)
        {
            tank.TakeDamage(damage);
        }
    }
}
