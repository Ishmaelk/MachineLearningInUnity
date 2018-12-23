using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    ANN ann;
    double sumSquareError;
	
	void Start () {
        sumSquareError = 0;
        ann = new ANN(nI: 2, nO: 1, nH: 1, nPH: 2, a: 0.8);
        List<double> result;
        for (int i = 0; i < 1000; i++) { // 1000 epochs
            sumSquareError = 0;
            result = Train(1, 1, 0);
            sumSquareError += Mathf.Pow((float)result[0] - 0, 2);
            result = Train(1, 0, 1);
            sumSquareError += Mathf.Pow((float)result[0] - 1, 2);
            result = Train(0, 1, 1);
            sumSquareError += Mathf.Pow((float)result[0] - 1, 2);
            result = Train(0, 0, 0);
            sumSquareError += Mathf.Pow((float)result[0] - 0, 2);
        }
        Debug.Log("---------------------");
        Debug.Log("Training Complete!");
        Debug.Log("Sum of squared error: " + sumSquareError);
        Debug.Log("---------------------");
        result = Train(1, 1, 0);
        Debug.Log("XOR(1, 1) = " + Mathf.RoundToInt((float)result[0]));
        result = Train(0, 1, 1);
        Debug.Log("XOR(0, 1) = " + Mathf.RoundToInt((float)result[0]));
        result = Train(1, 0, 1);
        Debug.Log("XOR(1, 0) = " + Mathf.RoundToInt((float)result[0]));
        result = Train(0, 0, 0);
        Debug.Log("XOR(0, 0) = " + Mathf.RoundToInt((float)result[0]));
    }
	
    List<double> Train (double i1, double i2, double o) {
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();
        inputs.Add(i1);
        inputs.Add(i2);
        outputs.Add(o);
        return ann.Go(inputs, outputs);
    }
}
