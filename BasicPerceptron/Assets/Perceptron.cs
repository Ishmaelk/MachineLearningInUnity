using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrainingSet {
    public double[] input;
    public double output;
}


public class Perceptron : MonoBehaviour {

    public TrainingSet[] ts; // each line in the array is one line in the training set
    double[] weights = { 0, 0 }; // number of weights match number of inputs
    double bias = 0;
    double totalError = 0; // keeps track of each epochs errors

    void Start() {
        Train(numEpochs: 8);
        Debug.Log("Test 0 0: " + CalculateOutput(0, 0));
        Debug.Log("Test 0 1: " + CalculateOutput(0, 1));
        Debug.Log("Test 1 0: " + CalculateOutput(1, 0));
        Debug.Log("Test 1 1: " + CalculateOutput(1, 1));
    }

    void Train(int numEpochs) {
        InitializeWeights();
        for (int i = 0; i < numEpochs; i++) {
            totalError = 0;
            for (int t = 0; t < ts.Length; t++) {
                UpdateWeights(t);
                Debug.Log("W1: " + weights[0] + " W2: " + weights[1] + " B: " + bias);
            } Debug.Log("Total Error: " + totalError);
        }
    }

    void UpdateWeights (int t) { // updates weights based on error 
        double error = ts[t].output - CalculateOutput(t); // difference between output we want and our answer
        totalError += Mathf.Abs((float)error);
        for (int i = 0; i < weights.Length; i++)
            weights[i] = weights[i] + error * ts[t].input[i]; // update weight by error * this training set's input 
        bias += error;
    }

    double CalculateOutput (int i) { // returns a 0 or 1 decision based on weights and bias
        double dp = DotProductBias(ts[i].input, weights);
        return dp > 0 ? 1 : 0;
    }

    double CalculateOutput (double i1, double i2) {
        double[] input = { i1, i2 };
        double dp = DotProductBias(weights, input);
        return dp > 0 ? 1 : 0;
    }

    double DotProductBias (double[] inputs, double[] weights) {
        if (inputs == null || weights == null)
            return -1;
        if (inputs.Length != weights.Length) // cant calculate if not equal length
            return -1;
        double d = 0;
        for (int i = 0; i < inputs.Length; i++)
            d += inputs[i] * weights[i];
        d += bias;
        return d;
    }

    void InitializeWeights () { // initializes weights and bias with random values bet -1 and 1
        for (int i = 0; i < weights.Length; i++)
            weights[i] = Random.Range(-1.0f, 1.0f);
        bias = Random.Range(-1.0f, 1.0f);
    }

}
