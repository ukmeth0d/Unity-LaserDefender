using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float padding = 1;
    public GameObject projectile;
    public float projectileSpeed = 0f;
    public float firingRate = 0.2f;
    public float health = 500f;
    public AudioClip fireSound;

	private float xMin;
	private float xMax;

	// Use this for initialization
	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distance));

		xMin = leftMost.x + padding;
		xMax = rightMost.x - padding;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.position += Vector3.left * (speed * Time.deltaTime);
		} else if(Input.GetKey(KeyCode.RightArrow)){
			transform.position += Vector3.right * (speed * Time.deltaTime);
		}

        if (Input.GetKeyDown(KeyCode.Space)) {
            InvokeRepeating("Fire", 0.000001f, firingRate);
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            CancelInvoke();
        }

		// Restrict the player to the gamespace
		float newX = Mathf.Clamp (transform.position.x, xMin, xMax);
		transform.position = new Vector3 (newX, transform.position.y, transform.position.z);
	}

    void OnTriggerEnter2D(Collider2D col) {
        Projectile missile = col.gameObject.GetComponent<Projectile>();

        if (missile && missile.enemyLaser) {
            health -= missile.GetDamage();
            missile.Hit();
            if (health <= 0) {
                Die();
            }
        }
    }

    void Fire () {
        GameObject beam = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        Rigidbody2D beamRigidbody = beam.GetComponent<Rigidbody2D>();
        beamRigidbody.velocity = new Vector3(0, projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
        //beam.transform.parent = transform;
    }

    void Die() {
        Destroy(gameObject);
        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelManager.LoadLevel("Win Screen");
    }
}
