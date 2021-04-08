using UnityEngine;
using System.Collections;

public class BombSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public Vector2 fireDirection;
    public float power;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject Fire() {
        var spwanPosition = new Vector3(transform.position.x, transform.position.y + 100, -5);
        var id_ = Instantiate(ballPrefab);
        id_.GetComponent<Transform>().position = spwanPosition;
        id_.GetComponent<Rigidbody2D>().AddForce(fireDirection * power);
        return id_;
    }

}
