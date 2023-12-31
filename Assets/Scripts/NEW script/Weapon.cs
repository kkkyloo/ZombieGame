using UnityEngine;

public abstract class Weapon 
{
    protected int damage;
    protected int range;
    protected float attackDelay = 0.1f;

    protected GameObject player;
    protected Camera fpsCam;

    protected static Animator animator;
    protected string currentAnimaton;
    protected static string IDLE;
    protected string FIRE;

    protected bool isAttackPressed;
    protected bool isAttacking;
    protected bool isRunning;
    protected float secundomer;
    protected static bool animComplite;

    protected AudioClip impact;
    protected AudioSource akSound;

    protected void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }


    // private float startPosX;
    // private float startPosY;
    //  private string enableRUN;
    //  private string disableRUN;
    //  private string reloadAnim;
    //  [SerializeField] private GameObject _bulletHolePrefab;
    //  [SerializeField] private GameObject _prefab;

    //  [SerializeField] public static TextMeshProUGUI ammoText;

    //   public static bool isReloading = false;
    // protected ParticleSystem flash;

    //   protected AudioClip reload;
    //   protected AudioClip noAmmo;
    //  [SerializeField] public static int MaxAmmo = 60;
    //  [SerializeField] public static int currentAmmo = 30;










}
