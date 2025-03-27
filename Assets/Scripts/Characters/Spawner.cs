using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class Spawner : MonoBehaviour {
	public float m_interval = 3;
	public GameObject m_moveTarget;

	private float m_lastSpawn = -1;

	private void FixedUpdate () {
		if (Time.time > m_lastSpawn + m_interval) {

            Monster.Create(transform.position, m_moveTarget);

            m_lastSpawn = Time.time;
		}
	}
}
