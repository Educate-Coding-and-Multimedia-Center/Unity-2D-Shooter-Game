using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour {

	WaveConfig waveConfig;
	float moveSpeed = 5f;
	List<Transform> wayPoints;
	int waypointIndex = 0;

	// Use this for initialization
	void Start () {
		if (waveConfig != null)
		{
			wayPoints = waveConfig.GetWayPoints();
			moveSpeed = waveConfig.GetMoveSpeed();
			transform.position = wayPoints[waypointIndex].transform.position;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (wayPoints != null)
		{
			if (waypointIndex <= wayPoints.Count - 1)
			{
				Vector3 targetPosition = wayPoints[waypointIndex].transform.position;
				float movement = moveSpeed * Time.deltaTime;
				transform.position = Vector2.MoveTowards(transform.position, targetPosition, movement);
				if (transform.position == targetPosition)
				{
					waypointIndex++;
				}
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}

	public void SetWaveConfig(WaveConfig waveConfig) {
		this.waveConfig = waveConfig;
	}
}
