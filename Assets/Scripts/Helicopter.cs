using UnityEngine;

public class Helicopter : MonoBehaviour {
    private float speed;
    private float downForce;
    private float height;
    private Vector3 direction;

    private GameManager manager;
    private GameObject parachuterPrefab;
    private GameObject barrelPrefab;
    private SpriteRenderer heliSpriteRenderer;
    

    public void Start() {
        speed = 2.0f;
        downForce = 0.0f;
        height = 3.0f;
        direction = Vector3.right;
        Vector3 startPos = transform.position;
        startPos.y = height;
        transform.position = startPos;
        manager = GameObject.Find("[MANAGER]").GetComponent<GameManager>();
        parachuterPrefab = Resources.Load<GameObject>("Prefabs/Prefab_Parachuter");
        barrelPrefab = Resources.Load<GameObject>("Prefabs/Prefab_Barrel");
        heliSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void FixedUpdate() {
        //Incrementally raise speed/difficulty and transform position
        speed += 0.01f;
        downForce += 0.25f;
        transform.Translate(direction * speed * Time.fixedDeltaTime);

        //Change helicopter direction according to random chance or if leaving game boundaries
        float randDirectionChange = Random.Range(0.0f, 10.0f);
        if (randDirectionChange <= 0.25f || transform.position.x >= 6.60f)  {
            direction = Vector3.left;
            heliSpriteRenderer.flipX =  true;
        }
        else if (randDirectionChange >= 9.75f || transform.position.x <= -6.75f) {
            direction = Vector3.right;
            heliSpriteRenderer.flipX = false;
        }
        
        //Random chance to drop, 70/30 chance for parachuter/barrel respectively
        if (Random.Range(0.0f, 10.0f) < 0.15f) {
            GameObject randDropObject = (Random.Range(0.0f, 10.0f) < 7.0) ?
                    Instantiate<GameObject>(parachuterPrefab) :
                    Instantiate<GameObject>(barrelPrefab);
            Vector3 pos = transform.position;
            pos.z = 0.0f;
            Rigidbody2D doRigidbody = randDropObject.GetComponent<Rigidbody2D>();
            randDropObject.transform.position = pos;
            doRigidbody.AddForce(-transform.up * downForce);
            manager.dropOneShotAudio();
        }
    }
}
