using UnityEngine;

public class Catcher : MonoBehaviour {
    private GameManager manager;

    public void Start() {
        manager = GameObject.Find("[MANAGER]").GetComponent<GameManager>();
    }

    public void Update() {
        Vector3 mousePos2D = Input.mousePosition;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 pos = transform.position;
        pos.x = mousePos3D.x;
        transform.position = pos;
        /*
        if (mousePos3D.x >= -6.9f && mousePos3D.x <= 6.9f) {
            Vector3 pos = transform.position;
            pos.x = mousePos3D.x;
            transform.position = pos;
        }*/
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        GameObject collidedWith = collision.gameObject;
        manager.caughtObject(collidedWith.tag);
        Destroy(collidedWith);
    }
}
