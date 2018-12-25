using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    public GameObject paddle;
    public GameObject ball;
    Rigidbody2D brb;
    float yvel;
    float paddleMinY;
    float paddleMaxY;
    float paddleMaxSpeed;
    public float numSaved;
    public float numMissed;
    ANN ann;

	void Start () {
        paddleMinY = 8.8f;
        paddleMaxY = 17.4f;
        paddleMaxSpeed = 15;
        numSaved = 0;
        numMissed = 0;
        brb = ball.GetComponent<Rigidbody2D>();
        ann = new ANN(6, 1, 1, 4, 0.11); // 6 inputs, 1 output, 1 hidden layer, 4 neurons, 0.11 alpha
    }
	
	void Update () {
        float posy = Mathf.Clamp(paddle.transform.position.y + (yvel * Time.deltaTime * paddleMaxSpeed),
            paddleMinY, paddleMaxY);
        Vector3 position = new Vector3 ( paddle.transform.position.x, posy, paddle.transform.position.z );
        paddle.transform.position = position;
        List<double> output = new List<double>();
        int layerMask = 1 << 9;
        RaycastHit2D hit = Physics2D.Raycast(ball.transform.position, brb.velocity, 1000, layerMask);
        if (hit.collider != null) {
            if (hit.collider.gameObject.CompareTag("tops")) { // raycast from reflection to backwall
                Vector3 reflection = Vector3.Reflect(brb.velocity, hit.normal);
                hit = Physics2D.Raycast(hit.point, reflection, 1000, layerMask);
            }
            if (hit.collider == null) {
                yvel = 0;
                return;
            }
            if (gameObject.name == "RightPlayerBrain" &&
                hit.collider.gameObject.CompareTag("backwall")) { // raycast from ball -> ball direction
                float dy = hit.point.y - paddle.transform.position.y; // get difference in y from paddle to backwall hit
                output = Run(ball.transform.position.x,
                            ball.transform.position.y,
                            brb.velocity.x, brb.velocity.y,
                            paddle.transform.position.x,
                            paddle.transform.position.y,
                            dy, true); // train the ANN
                yvel = (float)output[0]; // set the y to the output
            } else if (gameObject.name == "LeftPlayerBrain"
                && hit.collider.gameObject.CompareTag("frontwall")) {
                float dy = hit.point.y - paddle.transform.position.y; // get difference in y from paddle to backwall hit
                output = Run(ball.transform.position.x,
                            ball.transform.position.y,
                            brb.velocity.x, brb.velocity.y,
                            paddle.transform.position.x,
                            paddle.transform.position.y,
                            dy, true); // train the ANN
                yvel = (float)output[0]; // set the y to the output
            }
        } else // otherwise paddle doesnt move
            yvel = 0;
	}

    // Wrapper method for train and calculate output //
    List<double> Run (double bx, double by, double bvx, double bvy, double px, double py, double pv, bool train) {
        List<double> inputs = new List<double>() { bx, by, bvx, bvy, px, py };
        List<double> outputs = new List<double>() { pv };
        return train ? ann.Train(inputs, outputs) : ann.CalcOutput(inputs, outputs);  
    }

}
