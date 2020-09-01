using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	[SerializeField] List<WaveConfig> waveConfigs;
	[SerializeField] int startingWave = 0;
	[SerializeField] bool looping = false;

	IEnumerator Start () {
		do {
			yield return StartCoroutine(SpawnAllWaves());
		} while(looping);
	}
	
	IEnumerator SpawnAllWaves() {
		for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex ++) {
			WaveConfig currentWave = waveConfigs[waveIndex];
			yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
		}
	}

	IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig) {
		float enemyCounts = waveConfig.GetNumberOfEnemies();
		for (int enemyOrder = 0; enemyOrder < enemyCounts; enemyOrder++){
			GameObject enemy = Instantiate(
				waveConfig.GetEnemyPrefab(),
				waveConfig.GetWayPoints()[0].transform.position,
				Quaternion.identity
			);
			enemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
			yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
		}
	}
}
