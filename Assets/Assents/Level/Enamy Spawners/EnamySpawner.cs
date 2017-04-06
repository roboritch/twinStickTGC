using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnamySpawner : MonoBehaviour {
	[SerializeField]
	protected GameObject enamyPrefab;
	//TODO load premade enamy decks from xml rather than setting decks in prefabs
	[SerializeField]
	protected string targetTagName = "";
	
	[SerializeField]
	protected SpawnTimes spawnTimings;

	#region Interval Spawing
	[SerializeField]
	protected float spawnIntervel_seconds;
	protected float initalSpawnInterval_seconds;
	protected void initIntervalSpawing() {
		initalSpawnInterval_seconds = spawnIntervel_seconds;
	}

	protected void incrmentInterval() {
		spawnIntervel_seconds -= Time.deltaTime;
		if(spawnIntervel_seconds <= 0) {
			spawnEnamy();
			spawnIntervel_seconds = initalSpawnInterval_seconds;
		}
	}

	#endregion

	public void spawnEnamy() {
		if(enamyPrefab == null) {
			Debug.LogError("no actor to spawn: " + GetInstanceID());
			return;
		}

		AI enamy =  Instantiate(enamyPrefab,transform.position,new Quaternion()).GetComponent<AI>();
		if(enamy == null) {
			Debug.LogWarning("no actor component in spawning prefab (this spawner is not for players!):" + GetInstanceID());
			return;
		}
		if (targetTagName != "") {
			enamy.setTargetByTag(targetTagName);
		}
	}

	// Use this for initialization
	void Start () {
		if ((spawnTimings & SpawnTimes.onStart) == SpawnTimes.onStart) {
			spawnEnamy();
		}
		if ((spawnTimings & SpawnTimes.atIntervel) == SpawnTimes.atIntervel) {
			initIntervalSpawing();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if ((spawnTimings & SpawnTimes.atIntervel) == SpawnTimes.atIntervel) {
			incrmentInterval();
		}
	}
}

[System.Flags]
public enum SpawnTimes {
	onTriger = 1 << 0,
	onStart = 1 << 1,
	atIntervel = 1 << 2,
}
