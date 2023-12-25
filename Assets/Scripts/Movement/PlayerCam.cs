using System.Collections;
using UnityEngine;
public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [SerializeField] private Transform orientation;

    private float xRotation;
    private float yRotation;

    private static float sens = 1;

    private string nameWeapon;

    private float mouseX = 0;
    private float mouseY = 0;

    private bool fireJoy = false;
    private bool jumpJoy = false;

    public static bool fire = false;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void fireJoyDown()
    {
        fireJoy = true;

    }
    public void fireJoyUp()
    {
        fireJoy = false;
    }
    public void jumpJoyDown()
    {
        jumpJoy = true;

    }
    public void jumpJoyUp()
    {
        jumpJoy = false;
    }


    public void FireDown()
    {
        fire = true;

    }

    public void FireUp()
    {
        fire = false;
    }



    private void Update()
    {

         mouseX = SimpleInput.GetAxis("Panel X") * Time.deltaTime * sensX * sens;
         mouseY = SimpleInput.GetAxis("Panel Y") * Time.deltaTime * sensY * sens;         
        

        
        if(fireJoy)
        {
            mouseX = SimpleInput.GetAxis("fire X") * Time.deltaTime * sensX * sens;
            mouseY = SimpleInput.GetAxis("fire Y") * Time.deltaTime * sensY * sens;
        }        
        if(jumpJoy)
        {
            mouseX = SimpleInput.GetAxis("jump X") * Time.deltaTime * sensX * sens;
            mouseY = SimpleInput.GetAxis("jump Y") * Time.deltaTime * sensY * sens;
        }



        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        //nameWeapon = playermanager.nameWeapon;
        
        if (fire && WeaponSwitching.selectedWeapon == 1 && !Ak47.isReloading && Ak47.currentAmmo != 0)  //&& nameWeapon == "ak47"
        {
            StartCoroutine(TestCoroutine());

        }
        if (fire && WeaponSwitching.selectedWeapon == 2 && !shotgun.isReloading && shotgun.currentAmmo != 0) //&& nameWeapon == "ak47"
        {

            StartCoroutine(TestCoroutine2());

        }
        if (PlayerMovement.horizontalInput < 0) // сделать поле для изменения и починить баг при стрельбе не работает
        {
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 2f);
        }
        if (PlayerMovement.horizontalInput > 0)
        {
            transform.rotation = Quaternion.Euler(xRotation, yRotation, -2f);
        }



    }
    IEnumerator TestCoroutine2()
    {
        while (fire && !shotgun.isReloading && shotgun.currentAmmo != 0)
        {
            yield return null;
            
            transform.rotation = Quaternion.Euler(new Vector3(xRotation + Random.Range(-1f, 1f), yRotation + Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
        }
    }




    IEnumerator TestCoroutine()
    {
        while (fire && !Ak47.isReloading && Ak47.currentAmmo != 0)
        {
            yield return null;
            transform.rotation = Quaternion.Euler(new Vector3(xRotation + Random.Range(-0.3f, 0.3f), yRotation + Random.Range(-0.3f, 0.3f), Random.Range(-0.5f, 0.5f)));
        } 
    }



    public static void Damage(float Count)
    {
        sens = Count;
    }
}