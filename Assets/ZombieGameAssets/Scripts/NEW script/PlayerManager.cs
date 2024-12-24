using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour, IDamageable
{
    [Header("Настройки здоровья")]
    [SerializeField] private float _hp = 100f;
    [SerializeField] private float _maxHp = 100f;

    [Header("Ссылки на UI-элементы для HP")]
    [SerializeField] private Image _hpFillImage;      
    [SerializeField] private TextMeshProUGUI _hpText; 

    private void OnEnable()
    {
        Actions.GetEnemyHit += TakeDamage;
    }
    private void Start()
    {
        UpdateHPUI();
    }
    private void OnDisable()
    {
        Actions.GetEnemyHit -= TakeDamage;
    }


    public void TakeDamage(float damage)
    {
        _hp -= damage;
        if (_hp <= 0f)
        {
            _hp = 0f;
            Die();
        }
        
        UpdateHPUI();
    }


    public void TakeHeal(float heal)
    {
        _hp += heal;
        if (_hp > _maxHp) 
            _hp = _maxHp;

        UpdateHPUI();
    }


    private void UpdateHPUI()
    {
        if (_hpFillImage != null)
            _hpFillImage.fillAmount = _hp / _maxHp;

        if (_hpText != null)
            _hpText.text = Mathf.RoundToInt(_hp).ToString();
    }

    private void Die()
    {
        Debug.Log("Player is Dead!");
    }
}
