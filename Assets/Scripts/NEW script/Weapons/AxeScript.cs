using System.Collections;
using UnityEngine;
public class AxeScript : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private int damage = 65;
    [SerializeField] private float attackDelay = 1.8f;
    [Header("Audio")]
    [SerializeField] private float InitialSoundVolume = 0.7f;
    [SerializeField] private float ImpactDuration = 0.8f;
    [Header("GameObjects")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private AudioClip impact;

    private GameObject _axe;
    private Collider _axeCollider;
    private AudioSource sound;
    private Animator animator;

    private string currentAnimation;
    private string idleAnimation;
    private string fireAnimation;

    private bool isAttacking;
    private bool isSoundEnabled = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sound = gameObject.AddComponent<AudioSource>();
        sound.clip = impact;

        _axe = GameObject.FindGameObjectWithTag("axeArms");
        _axeCollider = GetComponent<Collider>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        idleAnimation = clips[0].name;
        fireAnimation = clips[1].name;
    }
    private void OnEnable() => ChangeAnimationState(idleAnimation);
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && _axe.activeSelf && animator.GetCurrentAnimatorStateInfo(0).IsName(idleAnimation))
            Attack();
    }
    private void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(PerformAttack());
        }
    }
    private IEnumerator PerformAttack()
    {
        if (isSoundEnabled)
        {
            sound.PlayOneShot(impact, InitialSoundVolume);
            isSoundEnabled = false;
        }

        ChangeAnimationState(fireAnimation);
        _axeCollider.enabled = true;

        yield return new WaitForSeconds(ImpactDuration);

        _axeCollider.enabled = false;
        yield return new WaitForSeconds(attackDelay - ImpactDuration);

        AttackComplete();
    }
    private void AttackComplete()
    {
        isSoundEnabled = true;
        isAttacking = false;
        ChangeAnimationState(idleAnimation);
    }
    private void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            Destroy(Instantiate(prefab, other.transform.position, Quaternion.identity), 0.5f);
            other.gameObject.GetComponent<Targets>().TakeDamage(damage);
        }
    }
}