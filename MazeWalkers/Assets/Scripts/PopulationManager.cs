using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour {

    public static float elapsed = 0;
    public float trialTime = 5;
    int generation = 1;

    public GameObject spawnPoint;
    public GameObject BOTPREFAB;
    public int populationSize = 50;
    List<Brain> population = new List<Brain>();

    GUIStyle guiStyle = new GUIStyle();

    void OnGUI() {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), "Time: " + elapsed, guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population Size: " + population.Count, guiStyle);
        GUI.EndGroup();
    }

   
    void Start () { // create starting population
        population = new List<Brain>();
        BOTPREFAB = Resources.Load<GameObject>("Agent");
        for (int i = 0; i < populationSize; i++) {
            
            GameObject agent = Instantiate(BOTPREFAB, spawnPoint.transform.position, transform.rotation);
            Brain brain = agent.GetComponent<Brain>();
            brain.Init();
            population.Add(brain);
        }
	}

    void Update() {
        elapsed += Time.deltaTime;
        if (elapsed >= trialTime) {
            BreedNewPopulation();
            elapsed = 0;
        }
    }

    void BreedNewPopulation () {
        List<Brain> sorted = population.OrderBy(o => o.distanceTraveled).ToList();
        population.Clear();
        for (int i = (int) (sorted.Count/2.0f) -1; i < sorted.Count-1; i++) { // breed best half of population
            population.Add(Breed(sorted[i], sorted[i + 1]));
            population.Add(Breed(sorted[i+1], sorted[i]));
        }

        foreach(var a in sorted)
            Destroy(a.gameObject);
        generation++;

    }

    Brain Breed (Brain parent1, Brain parent2) {
        Vector3 start_pos = new Vector3(this.transform.position.x + Random.Range(-2, 2),
                                            this.transform.position.y + Random.Range(-2, 2),
                                            this.transform.position.z + 0.5f);
        GameObject offspring = Instantiate(BOTPREFAB, start_pos, this.transform.rotation);
        Brain brain = offspring.GetComponent<Brain>();
        brain.Init();
        if (Random.Range(0, 100) == 1)
            brain.dna.Mutate();
        else
            brain.dna.Combine(parent1.dna, parent2.dna);
        return brain;
    }
	
}
