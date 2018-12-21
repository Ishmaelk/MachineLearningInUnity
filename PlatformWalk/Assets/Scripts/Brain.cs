using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    int dnaLength = 2;
    public float timeAlive;
    public float timeWalking;
    public DNA dna;
    public GameObject eyes;
    bool alive = true;
    bool groundVisible = true;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("dead") && PopulationManager.elapsed >= 1f) {
            alive = false;
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    public void Init () {
        timeAlive = 0;
        timeWalking = 0;
        // forward: 0, left: 1, right: 2
        dna = new DNA(dnaLength, 3);
        timeAlive = 0;
        alive = true;
    }

	void Update () {
        if (!alive) return;
        Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 10, Color.red, 10);
        
        RaycastHit hit;
        if (Physics.Raycast(eyes.transform.position, eyes.transform.forward*10, out hit))
            groundVisible = hit.collider.gameObject.CompareTag("platform") ? true : false;
        timeAlive = PopulationManager.elapsed;
        float turn = 0;
        float move = 0;
        if (groundVisible) {
            int zeroGene = dna.GetGene(0);
            if (zeroGene == 0) { move = 1; timeWalking += Time.deltaTime; }
            else if (zeroGene == 1) turn = -90;
            else if (zeroGene == 2) turn = 90;
        } else { // if the ground is not visible
            int oneGene = dna.GetGene(1);
            if (oneGene == 0) { move = 1; timeWalking += Time.deltaTime; }
            else if (oneGene == 1) turn = -90;
            else if (oneGene == 2) turn = 90;
        }

        this.transform.Translate(0, 0, move * 0.1f);
        this.transform.Rotate(0, turn, 0);

	}
}
