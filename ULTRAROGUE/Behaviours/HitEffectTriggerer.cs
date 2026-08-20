using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class HitEffectTriggerer : MonoBehaviour
{
    public UnityEvent<float> OnHit;

    public void OnGottenHit(float dmg)
    {
        OnHit?.Invoke(dmg);
    }
}
