using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Animals {
    BUNNY,
    BEAR,
    EAGLE,
}

public class GameManager : MonoBehaviour {
    public Text connectionInfo;

    public Object[] animalPrefabs;

    public Object shadowPrefab;
    public Transform shadowHolder;
    public Transform spawnCenter;


    public static GameManager instance = null;
    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public static readonly string[] animals = {
        "Rabbit",
        "Bear",
        //"Squirrel",
        //"Eagle",
        //"Snake",
        //"Vulture",
        //"Spider" // to kill birds??
        //"Human",
    };

    public static readonly string[] animalInstructions = {
        "Tap to hop in the air",
        "Tap to do a swipe attack", //"Tap to eat berries or defend yourself",
        "Tap to (climb or jump) from trees",
        "Tap to begin dive attack",
        "Tap to strike forward",
        "Tap to land or take off",
        "Tap to shoot"
    };

    public static readonly string[] animalObjectives = {
        "Stay alive as long as possible",
        "Kill everything", //"Eat berries", 
        "Eat nuts",
        "Kill critters",
        "Kill other predators",
        "Eat dead things",
        "Hunt them beasts"
    };

    // instantiates and returns reference to correct prefab
    // as well as string object to send to phone
    public AnimalStartInfo GetNextAnimal() {
        // TODO make more complicated spawn algorithm
        int index = Random.Range(0, animalPrefabs.Length);

        Vector3 rand = Random.insideUnitCircle * 10.0f;
        rand.z = rand.y;
        rand.y = 0.0f;
        Vector3 spawnPoint = spawnCenter.position + Vector3.up * 0.5f + rand;
        GameObject prefab = (GameObject)Instantiate(animalPrefabs[index], spawnPoint, Quaternion.identity);

        return new AnimalStartInfo(prefab, index);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            connectionInfo.enabled = !connectionInfo.enabled;
        }
    }
}

public class AnimalInfo {
    public AnimalInfo(int index) {
        character = GameManager.animals[index];
        instructions = GameManager.animalInstructions[index];
        objective = GameManager.animalObjectives[index];
    }
    public string character;
    public string instructions;
    public string objective;
}

public class AnimalStartInfo {
    public AnimalStartInfo(GameObject prefab, int index) {
        this.prefab = prefab;
        data = new AnimalInfo(index);
    }

    public GameObject prefab;
    public AnimalInfo data;
}


