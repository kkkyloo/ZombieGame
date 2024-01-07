using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int _hp = 100;
    private void OnEnable()
    {
        Actions.GetEnemyHit += Damage;
    }
    private void OnDisable()
    {
        Actions.GetEnemyHit -= Damage;
    }
    private void Damage(int damage)
    {
        if (_hp - damage > 0) _hp -= damage;
        else Die();
    }
    private void Die()
    {
        Debug.Log("Die");
    }
}
