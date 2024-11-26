using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamageable
{
    [SerializeField] private float _hp = 100;
    private void OnEnable()
    {
        Actions.GetEnemyHit += TakeDamage;
    }
    private void OnDisable()
    {
        Actions.GetEnemyHit -= TakeDamage;
    }
    public void TakeDamage(float damage)
    {
        if (_hp - damage > 0) _hp -= damage;
        else Die();
    }
    private void Die()
    {
        Debug.Log("Die");
    }
}
