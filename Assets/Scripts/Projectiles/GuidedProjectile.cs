using UnityEngine;
using System.Collections;

public class GuidedProjectile : MonoBehaviour, IBullet
{
	public GameObject m_target;
	public float m_speed = 0.2f;
	public int m_damage = 10;

	void FixedUpdate () {
		if (m_target == null) {
			Destroy (gameObject);
			return;
		}

		var translation = m_target.transform.position - transform.position;

		if (translation.magnitude > m_speed) {
			translation = translation.normalized * m_speed;
		}
		transform.Translate (translation);
	}

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent(out IHealth monster);

		if (monster != null)
			OnHit(monster);
    }

    public void OnHit(IHealth target)
    {
        target.TakeDamage(m_damage);
        Destroy(gameObject);
    }
}
