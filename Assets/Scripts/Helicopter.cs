using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour {
    private const float height = 3.0f;
    private const float speedIncrement = 0.01f;
    private const float dropIncrement = 0.25f;
    private const float maxPercent = 1.0f;

    private float speed;
    private float dropMagnitude;
    private Vector3 direction;
    private GameManager manager;
    private GameObject parachuterPrefab;
    private GameObject barrelPrefab;
    private SpriteRenderer heliSpriteRenderer;
    private Rigidbody2D heliRigidBody;
    private List<GameObject> activeParachuters = new List<GameObject>();
    private List<GameObject> activeBarrels = new List<GameObject>();


    public void Awake() {
        speed = 2.0f;
        dropMagnitude = 0.0f;
        direction = Vector3.right;
        Vector3 startPos = transform.position;
        startPos.y = height;
        transform.position = startPos;
        manager = GameObject.Find("[MANAGER]").GetComponent<GameManager>();
        parachuterPrefab = Resources.Load<GameObject>("Prefabs/Prefab_Parachuter");
        barrelPrefab = Resources.Load<GameObject>("Prefabs/Prefab_Barrel");
        heliSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        heliRigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate() {
        //Incrementally raise speed/difficulty and transform position
        speed += speedIncrement;
        dropMagnitude += dropIncrement;
        heliRigidBody.AddForce(direction * speed);

        //Flip helicopter direction according to random chance(5%)
        if (randomPercentChanceLTE(0.05f))  {
            heliSpriteRenderer.flipX = !heliSpriteRenderer.flipX;
            direction = Vector3.right * (heliSpriteRenderer.flipX ? -1 : 1);
        }
        
        //Random chance to drop, 70/30 chance for parachuter/barrel respectively
        if (randomPercentChanceLTE(0.015f)) {
            GameObject randDropObject = (randomPercentChanceLTE(0.7f)) ?
                    Instantiate<GameObject>(parachuterPrefab) :
                    Instantiate<GameObject>(barrelPrefab);

            Vector3 pos = transform.position;
            pos.z = 0.0f;
            Rigidbody2D doRigidbody = randDropObject.GetComponent<Rigidbody2D>();
            randDropObject.transform.position = pos;
            doRigidbody.AddForce(-transform.up * dropMagnitude);
            manager.dropOneShotAudio();


            if (randDropObject.tag == "Barrel") {
                activeBarrels.Add(randDropObject);
            } else if (randDropObject.tag == "Parachuter") {
                activeParachuters.Add(randDropObject);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        GameObject collidedWith = collision.gameObject;
        
        if (collidedWith.tag == "GameController") {
            heliSpriteRenderer.flipX = !heliSpriteRenderer.flipX;
            direction = Vector3.right * (heliSpriteRenderer.flipX ? -1 : 1);
        }
    }

    public void clearActiveParachuters() {
        foreach (GameObject parachuter in activeParachuters) {
            Destroy(parachuter);
        }
        activeParachuters.Clear();
    }

    public bool randomPercentChanceLTE(float ltValue) {
        if (ltValue > maxPercent) {
            Debug.LogError("randomPercentChanceLT: LessThanValue greater than maxPercent");
        }

        return Random.Range(0.0f, maxPercent) <= ltValue;
    }
}
