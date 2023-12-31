using UnityEngine;

public class AxeScript : Weapon
{
    [SerializeField] private int damage;
    [SerializeField] private int range;
    [SerializeField] private float attackDelay = 0.1f;

    [SerializeField] private GameObject player;
    [SerializeField] private Camera fpsCam;

    [SerializeField] private static Animator animator;
    [SerializeField] private string currentAnimaton;
    [SerializeField] private static string IDLE;
    [SerializeField] private string FIRE;

    [SerializeField] private bool isAttackPressed;
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool isRunning;
    [SerializeField] private float secundomer;
    [SerializeField] private static bool animComplite;

    [SerializeField] private AudioClip impact;
    [SerializeField] private AudioSource akSound;

    [SerializeField] private void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

}
