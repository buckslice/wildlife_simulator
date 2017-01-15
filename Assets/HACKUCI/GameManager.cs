using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public Text connectionInfo;
    public Object shadowPrefab;
    public Transform shadowHolder;

    public static GameManager get = null;
    void Awake() {
        if(get == null) {
            get = this;
        } else {
            Destroy(gameObject);
        }
    }

    static readonly string[] animals = {
        "Rabbit",
        "Squirrel",
        "Bear",
        "Eagle",
        "Snake",
        "Vulture",
        //"Spider" // to kill birds??
        "Human",
    };

    static readonly string[] animalInstructions = {
        "Tap to hop in the air",
        "Tap to (climb or jump) from trees",
        "Tap to eat berries or defend yourself",
        "Tap to begin dive attack",
        "Tap to strike forward",
        "Tap to land or take off",
        "Tap to shoot"
    };

    static readonly string[] animalObjectives = {
        "Stay alive as long as possible",
        "Eat nuts",
        "Eat berries",
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
