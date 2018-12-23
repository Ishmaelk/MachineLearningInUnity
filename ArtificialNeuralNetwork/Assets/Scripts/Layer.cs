using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer { // holds the neurons for this particular layer

    public int numNeurons;
    public List<Neuron> neurons = new List<Neuron>();

    public Layer (int num_neurons, int num_neuron_inputs) { // number of inputs is the number of neurons in previous layer
        numNeurons = num_neurons;
        for (int i = 0; i < num_neurons; i++)
            neurons.Add(new Neuron(num_neuron_inputs));
    }

	
}
