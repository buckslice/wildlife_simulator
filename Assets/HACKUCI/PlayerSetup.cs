using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HappyFunTimes;

public class PlayerSetup : MonoBehaviour {

    void InitializeNetPlayer(SpawnInfo spawnInfo) {

        GameObject newPrefab = GameManager.instance.GetNextCharacter();
        newPrefab.SendMessage("InitializeFromAnimalPick", spawnInfo);

        Destroy(gameObject);
    }
}
