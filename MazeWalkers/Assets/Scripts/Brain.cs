using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    int dnaLength = 2;
    public float distanceTraveled;
    public DNA dna;
    public GameObject eyes;
    Vector3 startPosition;
    bool alive = true;
    bool wallVisible = true;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("dead")) {
            distanceTraveled = 0;
            alive = false;
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    public void Init () {
        startPosition = transform.position;
        distanceTraveled = 0;
        dna = new DNA(dnaLength, 360);
        alive = true;
        this.gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

	void Update () {
        if (!alive) return;
        //Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 0.5f, Color.red, 10);
        RaycastHit hit;
        if (Physics.SphereCast(eyes.transform.position, 0.1f, eyes.transform.forward, out hit, 0.5f))
            wallVisible = hit.collider.gameObject.CompareTag("wall") ? true : false; 

	}

    void FixedUpdate() {
        if (!alive) return;
        float h = 0;
        float v = dna.GetGene(0);
        if (wallVisible)
            h = dna.GetGene(1);
        transform.Translate(0, 0, v * 0.0001f);
        transform.Rotate(0, h, 0);
        distanceTraveled = Vector2.Distance(startPosition, transform.position);
    }

}
