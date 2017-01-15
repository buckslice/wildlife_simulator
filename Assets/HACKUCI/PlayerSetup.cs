using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HappyFunTimes;

public class PlayerSetup : MonoBehaviour {

    void InitializeNetPlayer(SpawnInfo spawnInfo) {
        AnimalStartInfo asi = GameManager.instance.GetNextAnimal();
        spawnInfo.data = asi.data;
        asi.prefab.GetComponent<TopDownGamePad>().InitializeFromAnimalPick(spawnInfo);
        Destroy(gameObject);
    }
}
