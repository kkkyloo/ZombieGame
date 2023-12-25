using UnityEngine;
public class ammoplus : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {


        if (other.gameObject.tag == "Player")
        {
            if (Ak47.MaxAmmo <= 60 && Ak47.MaxAmmo >= 30 )
            {
                Ak47.MaxAmmo = 60;
                
            }
            else if (Ak47.MaxAmmo < 30)
            {
                Ak47.MaxAmmo += 30;
            }

            if(shotgun.MaxAmmo <= 12 && shotgun.MaxAmmo >= 6)
            {
                shotgun.MaxAmmo = 12;
            }
            else if (shotgun.MaxAmmo < 6)
            {
                Ak47.MaxAmmo += 6;
            }


            Ak47.ammoText.text = Ak47.currentAmmo + " / " + Ak47.MaxAmmo;

            if (GameObject.FindGameObjectsWithTag("sgUI") != null)
                shotgun.ammoText.text = shotgun.currentAmmo + " / " + shotgun.MaxAmmo;

        }


        Destroy(gameObject);

    }
}
