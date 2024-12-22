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
    public void TakeHeal(float heal)
    {
        if (_hp + heal > 100) _hp = 100;
        else _hp += heal;
    }

    private void Die()
    {
        Debug.Log("Die");
    }
}
