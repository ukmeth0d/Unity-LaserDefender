﻿using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
    public float formationWidth = 10f;
    public float formationHeight = 5f;
    public float speed = 5f;
    public float spawnDelay = 0.5f;

    private bool movingRight = false;
    private float xMin;
    private float xMax;

	// Use this for initialization
	void Start () {

        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
        Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));

        xMin = leftBoundary.x;
        xMax = rightBoundary.x;

        SpawnUntilFull();

    }

    public void OnDrawGizmos () {
        Gizmos.DrawWireCube(transform.position, new Vector3(formationWidth, formationHeight));
    }
	
	// Update is called once per frame
	void Update () {
        if (movingRight) {
            transform.position += Vector3.right * speed * Time.deltaTime;
        } else {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        float rightEdgeOfFormation = transform.position.x + (0.5f * formationWidth);
        float leftEdgeOfFormation = transform.position.x - (0.5f * formationWidth);
        if (leftEdgeOfFormation <= xMin) {
            movingRight = true;
        } else if (rightEdgeOfFormation >= xMax) {
            movingRight = false;
        }

        if (AllMembersDead()) {
            SpawnUntilFull();
        }
	}

    void SpawnEnemies() {
        foreach (Transform child in transform) {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }
    }

    void SpawnUntilFull() {
        Transform freePosition = NextFreePosition();
        if (freePosition) {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
        }
        if (NextFreePosition()) {
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }

    bool AllMembersDead () {
        foreach(Transform childPositionGameObject in transform) {
            if(childPositionGameObject.childCount > 0) {
                return false;
            }
        }

        return true;
    }

    Transform NextFreePosition() {
        foreach (Transform childPositionGameObject in transform) {
            if (childPositionGameObject.childCount <= 0) {
                return childPositionGameObject;
            }
        }

        return null;
    }
}
