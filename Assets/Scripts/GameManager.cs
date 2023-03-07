using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private int currentCount;
    private int highestCount;

    private GameObject helicopterPrefab;
    private GameObject catcherPrefab;
    private GameObject activeHelicopter;
    private List<GameObject> activeCatchers;
    private AudioSource oneshotAudioSource;
    private AudioClip missClip;
    private AudioClip splashClip;
    private AudioClip catchClip;
    private AudioClip crashClip;
    private AudioClip dropClip;
    private TextMeshProUGUI score;
    private TextMeshProUGUI bestScore;


    public void Start() {
        currentCount = 0;
        highestCount = PlayerPrefs.GetInt("highestCount", 0);
        helicopterPrefab = Resources.Load<GameObject>("Prefabs/Prefab_Helicopter");
        catcherPrefab = Resources.Load<GameObject>("Prefabs/Prefab_Catcher");
        oneshotAudioSource = gameObject.AddComponent<AudioSource>();
        missClip = Resources.Load<AudioClip>("Audio/miss");
        splashClip = Resources.Load<AudioClip>("Audio/splash");
        catchClip = Resources.Load<AudioClip>("Audio/catch");
        crashClip = Resources.Load<AudioClip>("Audio/crash");
        dropClip = Resources.Load<AudioClip>("Audio/dropSound");
        score = GameObject.Find("/Canvas/text_score").GetComponent<TextMeshProUGUI>();
        bestScore = GameObject.Find("/Canvas/text_bestScore").GetComponent<TextMeshProUGUI>();
        score.text = "Current Score: " + currentCount;
        bestScore.text = "Best Score: " + highestCount;

        activeCatchers = new List<GameObject>();
        for (int i = 0; i < 3; i++) {
            GameObject newCatcher = Instantiate<GameObject>(catcherPrefab);
            Vector3 pos = Vector3.zero;

            pos.y = -4.4f + (i * 0.6f);
            newCatcher.transform.position = pos;

            activeCatchers.Add(newCatcher);
        }

        activeHelicopter = Instantiate<GameObject>(helicopterPrefab);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        string collisionTag = collision.gameObject.tag;
        if (collisionTag == "Parachuter") {
            GameObject[] dParachuterArray = GameObject.FindGameObjectsWithTag("Parachuter");
            foreach (GameObject dParachuter in dParachuterArray) {
                Destroy(dParachuter);
            }
            destroyCatcher();
            oneshotAudioSource.PlayOneShot(missClip);
        }
        else if (collisionTag == "Barrel") {
            Destroy(collision.gameObject);
            oneshotAudioSource.PlayOneShot(splashClip);
        }
    }

    private void destroyCatcher() {
        int index = activeCatchers.Count - 1;
        GameObject dCatcher = activeCatchers[index];

        activeCatchers.RemoveAt(index);
        Destroy(dCatcher);

        if (activeCatchers.Count == 0) {
            SceneManager.LoadScene("Game");
        }
    }

    public void caughtObject(string objectTag) {
        if (objectTag == "Parachuter") {
            oneshotAudioSource.PlayOneShot(catchClip);
            currentCount++;
        }
        else if (objectTag == "Barrel") {
            oneshotAudioSource.PlayOneShot(crashClip);
            currentCount -= 2;
        }

        score.text = "Current Score: " + currentCount;
        if (currentCount > highestCount) {
            highestCount = currentCount;
            bestScore.text = "Best Score: " + highestCount;
            PlayerPrefs.SetInt("highestCount", highestCount);
        }
    }

    public void dropOneShotAudio() {
        oneshotAudioSource.PlayOneShot(dropClip);
    }
}
