using UnityEngine;

public class Catcher : MonoBehaviour {
    private const float speed = 300.0f;
    private const float stopRange = 0.2f;
    private const float speedAdjustRange = 1.0f;
    private const float speedAdjustCoeff = 0.5f;

    private GameManager manager;
    private Rigidbody2D catcherRigidbody;


    public void Awake() {
        manager = GameObject.Find("[MANAGER]").GetComponent<GameManager>();
        catcherRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Update() {
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float xDiff = mousePos3D.x - transform.position.x;
        float xDiffAbs = Mathf.Abs(xDiff); 

        if (xDiffAbs > stopRange) {
            Vector2 direction = Vector2.right * Mathf.Sign(xDiff);
            catcherRigidbody.AddForce(direction * speed * (xDiffAbs < speedAdjustRange ? speedAdjustCoeff : 1.0f));
        } else {
            catcherRigidbody.velocity = Vector2.zero;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        GameObject collidedWith = collision.gameObject;
        manager.caughtObject(collidedWith.tag);
        if (collidedWith.tag != "GameController") {
            Destroy(collidedWith);
        }
    }
}
