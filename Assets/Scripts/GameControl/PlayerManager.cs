using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    void Awake() {
        instance = this;
    }

    public GameObject player;

    public void KillPlayer() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
