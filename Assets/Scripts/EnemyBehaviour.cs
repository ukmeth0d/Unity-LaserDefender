using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    public float health = 200f;
    public GameObject projectile;
    public float projectileSpeed = 0f;
    public float shotsPerSecond = 0.5f;
    public int scoreValue = 10;
    public AudioClip fireSound;
    public AudioClip deathSound;

    private ScoreKeeper scoreKeeper;

    void Start() {
        scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
    }

    void Update() {
        float probability = Time.deltaTime * shotsPerSecond;
        if (Random.value < probability) {
            Fire();
        }
    }

    void Fire() {
        GameObject beam = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        Rigidbody2D beamRigidbody = beam.GetComponent<Rigidbody2D>();
        beamRigidbody.velocity = new Vector3(0, -projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }

    void OnTriggerEnter2D(Collider2D col) {
        Projectile missile = col.gameObject.GetComponent<Projectile>();

        if (missile && !missile.enemyLaser) {
            health -= missile.GetDamage();
            missile.Hit();
            if(health <= 0) {
                Die();
            }
        }
    }

    void Die() {
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        scoreKeeper.Score(scoreValue);
        Destroy(gameObject);
    }
}
