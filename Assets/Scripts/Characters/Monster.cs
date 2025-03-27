using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour {

    [HideInInspector] public static List<Monster> Monsters = new List<Monster>();

    public MonsterHealth Health;
    [SerializeField] private float m_speed = 0.1f;
    private GameObject m_moveTarget;
    private const float m_reachDistance = 0.3f;
	private Vector3 m_curVelocity;

	private void FixedUpdate () {
		if (m_moveTarget == null)
			return;
		
		if (Vector3.Distance (transform.position, m_moveTarget.transform.position) <= m_reachDistance) {
			Remove(this);
            return;
		}

        m_curVelocity = m_moveTarget.transform.position - transform.position;

		if (m_curVelocity.magnitude > m_speed) {
            m_curVelocity = m_curVelocity.normalized * m_speed;
        }
		transform.Translate (m_curVelocity);
	}

	public Vector3 GetSpeed()
	{
		return	m_curVelocity;
    }

	public void SetTarget(GameObject gameObject)
	{
        m_moveTarget = gameObject;
    }

	public static Monster Create(Vector3 pos, GameObject target)
	{
        Monster monster = Instantiate(global::Monsters.Get().MonstersList[0], pos, Quaternion.identity);
        monster.SetTarget(target);
        Monsters.Add(monster);
		return monster;
    }

	public static void Remove(Monster monster)
	{
        Monsters.Remove(monster);
        Destroy(monster.gameObject);
    }
}
