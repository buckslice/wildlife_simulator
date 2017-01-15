using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    static readonly string[] animals = {
        "Rabbit",
        "Bear",
        //"Squirrel",
        //"Eagle",
        //"Snake",
        //"Vulture",
        //"Spider" // to kill birds??
        //"Human",
    };

    static readonly string[] animalInstructions = {
        "Tap to hop in the air",
        "Tap to eat berries or defend yourself",
        "Tap to (climb or jump) from trees",
        "Tap to begin dive attack",
        "Tap to strike forward",
        "Tap to land or take off",
        "Tap to shoot"
    };

    static readonly string[] animalObjectives = {
        "Stay alive as long as possible",
        "Eat berries",
        "Eat nuts",
        "Kill critters",
        "Kill other predators",
        "Eat dead things",
        "Hunt them beasts"
    };

    public class MessageCharacter {
        public MessageCharacter(int index) {
            character = animals[index];
            instructions = animalInstructions[index];
            objective = animalObjectives[index];
        }
        public string character;
        public string instructions;
        public string objective;
    }

    public MessageCharacter NextCharacter() {
        // TODO put spawn algo here

        int index = Random.Range(0, animals.Length);
        return new MessageCharacter(index);
    }

    // instantiates and returns reference to correct prefab
    public GameObject GetNextCharacter() {
        int index = Random.Range(0, animalPrefabs.Length);
        Vector3 rand = Random.insideUnitCircle * 10.0f;
        rand.z = rand.y;
        rand.y = 0.0f;
        Vector3 spawnPoint = spawnCenter.position + Vector3.up * 0.5f + rand;
        GameObject prefab = (GameObject)Instantiate(animalPrefabs[index], spawnPoint, Quaternion.identity);
        return prefab;
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
