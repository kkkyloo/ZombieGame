using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class trig : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("work");
        SceneManager.LoadScene(0);
    }
}
