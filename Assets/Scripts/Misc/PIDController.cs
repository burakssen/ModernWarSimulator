using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class PIDController
{
    private float p, i, d;
    public float kp, ki, kd;
    private float prevError;

    public PIDController(float p, float i, float d)
    {
        kp = p;
        ki = i;
        kd = d;
    }

    public void SetValues(float _p, float _i, float _d)
    {
        kp = _p;
        ki = _i;
        kd = _d;
    }

    public float GetOutput(float currentError, float deltaTime)
    {
        p = currentError;
        i += p * deltaTime;
        d = (p - prevError) / deltaTime;
        prevError = currentError;
        return p * kp + i * ki + d * kd;
    }
}