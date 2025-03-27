using UnityEngine;
using System.Collections;

public class CannonProjectile : MonoBehaviour, IBullet {
	public float m_speed = 0.2f, m_lifetime = 5f;
	public int m_damage = 10;

    public void Start()
    {
		Destroy(gameObject, m_lifetime);
    }

	void OnTriggerEnter(Collider other) {

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
