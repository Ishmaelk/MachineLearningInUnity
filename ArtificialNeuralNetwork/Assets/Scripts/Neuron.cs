using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron { // same functionality as perceptron before

    public int numInputs; // use this to create number of weights
    public double bias;
    public double output;
    public double errorGradient; // //
    public List<double> weights = new List<double>();
    public List<double> inputs = new List<double>();

    public Neuron (int ninput) {
        bias = Random.Range(-1.0f, 1.0f);
        numInputs = ninput;
        for (int i = 0; i < numInputs; i++)
            weights.Add(Random.Range(-1.0f, 1.0f));
    }

}
