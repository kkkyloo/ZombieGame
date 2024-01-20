using System;
public class Actions
{
    public static Action<float, float> OnMove;
    public static Action GunShoot;
    public static Action AxeShoot;
    public static Action<float, float, bool> OnMoveSound;
    public static Action OnMoveSound2;
    public static Action<int> GetEnemyHit;
}