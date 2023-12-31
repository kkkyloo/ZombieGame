using System.Collections;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
   // [SerializeField] private float sensXJoy;
  //  [SerializeField] private float sensYJoy;

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

    public float sensitivity = 2.0f; // Чувствительность движения мыши
    public float smoothing = 2.0f;   // Сглаживание движения

    private float rotationX = 0.0f;  // Переменная для хранения поворота по оси X
    private float rotationY = 0.0f;  // Переменная для хранения поворота по оси Y
    private Vector2 smoothV;






    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

        // mouseX = SimpleInput.GetAxis("Panel X") * Time.deltaTime * sensXJoy * sens;
        //  mouseY = SimpleInput.GetAxis("Panel Y") * Time.deltaTime * sensYJoy * sens;

        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // Применяем чувствительность и сглаживание
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothing);


    //    if (fireJoy)
     //   {
    //        mouseX = SimpleInput.GetAxis("fire X") * Time.deltaTime * sensXJoy * sens;
    //        mouseY = SimpleInput.GetAxis("fire Y") * Time.deltaTime * sensYJoy * sens;
   //     }        
   //     if(jumpJoy)
   //     {
   //         mouseX = SimpleInput.GetAxis("jump X") * Time.deltaTime * sensXJoy * sens;
   //         mouseY = SimpleInput.GetAxis("jump Y") * Time.deltaTime * sensYJoy * sens;
   //     }



        yRotation += smoothV.x;

        xRotation -= smoothV.y; 
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
        if (PlayerMovement.horizontalInput < 0) // сделать поле для изменения и починить баг при стрельбе не работает и сделать плавное смещение камеры
        {
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 1f);
        }
        if (PlayerMovement.horizontalInput > 0)
        {
            transform.rotation = Quaternion.Euler(xRotation, yRotation, -1f);
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