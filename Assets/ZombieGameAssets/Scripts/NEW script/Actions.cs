using System;
using UnityEngine;

public static class Actions
{
    public static Action<float, float> OnMove;
    public static Action GunShoot;
    public static Action<bool> AxeShoot;

    public static Action<float, float, bool> OnMoveSound;

    public static Action<int> GetEnemyHit;
}