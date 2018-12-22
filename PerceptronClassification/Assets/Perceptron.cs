using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrainingSet {
	public double[] input;
	public double output;

    public TrainingSet (double[] i, double o) {
        input = i;
        output = o;
    }

}

public class Perceptron : MonoBehaviour {

	public List<TrainingSet> ts;
	double[] weights = {0,0};
	double bias = 0;

	public SimpleGrapher sg;

    void Start() {
        CreateTrainingSet();
        DrawAllPoints();
        Train(200);
        // Drawing the decision boundary //
        //                  takes slope = deltaY / deltaX               and y-intercept
        sg.DrawRay((float)(-(bias / weights[1]) / (bias / weights[0])), (float)(-bias / weights[1]), Color.red);

        if (CalcOutput(0.3, 0.9) == 0)
            sg.DrawPoint(0.3f, 0.9f, Color.red);
        else
            sg.DrawPoint(0.3f, 0.9f, Color.yellow);

        if (CalcOutput(0.8, 0.1) == 0)
            sg.DrawPoint(0.8f, 0.1f, Color.red);
        else
            sg.DrawPoint(0.8f, 0.1f, Color.yellow);

    }

    void CreateTrainingSet () {
        double[] sharpness = { 0.5, 0, 0.2, 0.1, 0.3, 0.3, 0.5, 0.8, 0.1, 0.5, 0.6, 0.6, 0.7, 0.9 };
        double[] edible = { 1, 0.8, 1, 0.5, 0.5, 0.8, 0.9, 1, 0, 0.3, 0.3, 0.1, 0, 0 };
        double[] food = { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < food.Length; i++) {
            double[] input = { sharpness[i], edible[i] };
            TrainingSet s = new TrainingSet(input, food[i]);
            ts.Add(s);
        }
    }

    double DotProductBias(double[] v1, double[] v2) {
		if (v1 == null || v2 == null)
			return -1;
	 
		if (v1.Length != v2.Length)
			return -1;
	 
		double d = 0;
		for (int x = 0; x < v1.Length; x++) {
			d += v1[x] * v2[x];
		}

		d += bias;
	 
		return d;
	}

	double CalcOutput(int i) {
		double dp = DotProductBias(weights,ts[i].input);
		if(dp >= 0) return(1);
		return (0);
	}

	double CalcOutput(double i1, double i2) {
		double[] inp = new double[] {i1, i2};
		double dp = DotProductBias(weights,inp);
		if(dp >= 0) return(1);
		return (0);
	}

	void UpdateWeights(int j) {
		double error = ts[j].output - CalcOutput(j);
		for(int i = 0; i < weights.Length; i++) {			
			weights[i] = weights[i] + error*ts[j].input[i]; 
		}
		bias += error;
	}

	void InitialiseWeights() {
		for(int i = 0; i < weights.Length; i++) {
			weights[i] = Random.Range(-1.0f,1.0f);
		}
		bias = Random.Range(-1.0f,1.0f);
	}

	void Train(int epochs) {
		InitialiseWeights();
		for(int e = 0; e < epochs; e++) {
			for(int t = 0; t < ts.Count; t++) {
				UpdateWeights(t);
				Debug.Log("W1: " + (weights[0]) + "W2: " + (weights[1]) + "B: " + bias);
			}
		}

	}

	void DrawAllPoints() {
		for(int t = 0; t < ts.Count; t++) {
			if(ts[t].output == 0)
				sg.DrawPoint((float)ts[t].input[0], (float)ts[t].input[1], Color.magenta);
			else
				sg.DrawPoint((float)ts[t].input[0], (float)ts[t].input[1], Color.green);
		}
	}

}
