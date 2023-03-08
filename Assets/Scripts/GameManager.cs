using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    //TODO: set consts for particular values
    private const float catcherYPos = -4.4f;
    private const int pointIncrement = 1;
    private const int pointDecrement = 2;

    private int playerLives;
    private int currentCount;
    private int highestCount;
    private bool playerDead {
        get {
            return playerLives == 0;
        }
    }
    private GameObject helicopterPrefab;
    private GameObject catcherPrefab;
    private GameObject activeHelicopter;
    private AudioSource oneshotAudioSource;
    private AudioClip missClip;
    private AudioClip splashClip;
    private AudioClip catchClip;
    private AudioClip crashClip;
    private AudioClip dropClip;
    private TextMeshProUGUI score;
    private TextMeshProUGUI bestScore;
    private TextMeshProUGUI lives;


    public void Start() {
        playerLives = 3;
        currentCount = 0;
        highestCount = PlayerPrefs.GetInt("highestCount", 0);
        helicopterPrefab = Resources.Load<GameObject>("Prefabs/Prefab_Helicopter");
        catcherPrefab = Resources.Load<GameObject>("Prefabs/Prefab_Catcher");
        oneshotAudioSource = gameObject.AddComponent<AudioSource>();
        missClip = Resources.Load<AudioClip>("Audio/miss");
        splashClip = Resources.Load<AudioClip>("Audio/splash");
        catchClip = Resources.Load<AudioClip>("Audio/catch");
        crashClip = Resources.Load<AudioClip>("Audio/crash");
        dropClip = Resources.Load<AudioClip>("Audio/drop");
        score = GameObject.Find("/Canvas/text_score").GetComponent<TextMeshProUGUI>();
        bestScore = GameObject.Find("/Canvas/text_bestScore").GetComponent<TextMeshProUGUI>();
        lives = GameObject.Find("/Canvas/LifeTally/text_lives").GetComponent<TextMeshProUGUI>();

        score.text = "Current Score: " + currentCount;
        bestScore.text = "Best Score: " + highestCount;
        lives.text = "x " + playerLives;

        GameObject newCatcher = Instantiate<GameObject>(catcherPrefab);
        newCatcher.transform.position = new Vector3(0.0f, catcherYPos, 0.0f);
        activeHelicopter = Instantiate<GameObject>(helicopterPrefab);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        string collisionTag = collision.gameObject.tag;
        if (collisionTag == "Parachuter") {
            playerLives -= 1;
            if (playerDead) {
                SceneManager.LoadScene("Game");
            }
            activeHelicopter.GetComponent<Helicopter>().clearActiveParachuters();
            lives.text = "x " + playerLives;
            oneshotAudioSource.PlayOneShot(missClip);
        }
        else if (collisionTag == "Barrel") {
            Destroy(collision.gameObject);
            oneshotAudioSource.PlayOneShot(splashClip);
        }
    }

    public void caughtObject(string objectTag) {
        if (objectTag == "Parachuter") {
            currentCount += pointIncrement;
            oneshotAudioSource.PlayOneShot(catchClip, 0.15f);
        }
        else if (objectTag == "Barrel") {
            currentCount -= pointDecrement;
            oneshotAudioSource.PlayOneShot(crashClip, 0.8f);
        }

        score.text = "Current Score: " + currentCount;
        if (currentCount > highestCount) {
            highestCount = currentCount;
            bestScore.text = "Best Score: " + highestCount;
            PlayerPrefs.SetInt("highestCount", highestCount);
        }
    }

    public void dropOneShotAudio() {
        oneshotAudioSource.PlayOneShot(dropClip, 0.5f);
    }
}
