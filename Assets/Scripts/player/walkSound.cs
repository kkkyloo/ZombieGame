using UnityEngine;

public class walkSound : MonoBehaviour
{
    private AudioSource sound;
    private bool isgrounded;

    void Start()
    {
        sound = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        if (Mathf.Abs(PlayerMovement.horizontalInput) > 0 || Mathf.Abs(PlayerMovement.verticalInput) > 0 && !PlayerMovement.jumpPress)
        {
            sound.pitch = Random.Range(0.8f, 1.1f);
            sound.enabled = true;
        }
        else
            sound.enabled = false;
    }

  /*  void OnCollisionEnter(Collision theCollision)
    {
        if (theCollision.gameObject.tag == "floor")
        {
            isgrounded = true;
        }
    }

    void OnCollisionExit(Collision theCollision)
    {
        if (theCollision.gameObject.tag == "floor")
        {
            isgrounded = false;
        }
    }
  */
}
