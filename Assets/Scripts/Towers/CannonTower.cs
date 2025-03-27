using UnityEngine;

public class CannonTower : MonoBehaviour {
	public float m_shootInterval = 0.5f;
	public float m_range = 4f, m_rangeDetection = 6f;
	public float m_angleInDegrees, m_rotationSpeed;
	public GameObject m_projectilePrefab;
	public Transform m_cannonHub;
	public Transform m_shootPoint;

	private float m_lastShotTime = -0.5f;
	private Monster m_curMonster;
	private Vector3 m_shootDirection;
    private bool m_isReadyShoot = false;
    private float _v;

	void FixedUpdate () {
		if (m_projectilePrefab == null || m_shootPoint == null)
			return;

		m_curMonster = null;

        foreach (var monster in Monster.Monsters) {

			if (m_curMonster == null)
				m_curMonster = monster;

            if (Vector3.Distance(transform.position, monster.transform.position) < Vector3.Distance(transform.position, m_curMonster.transform.position))
				m_curMonster = monster;

            // Направление выстрела
            m_shootDirection = (GetPredictionPos(m_curMonster) - transform.position);

            if (Vector3.Distance(transform.position, m_curMonster.transform.position) > m_rangeDetection)
				continue;

            Rotate();

            if (Vector3.Distance(transform.position, m_curMonster.transform.position) > m_range)
				continue;

            if (m_lastShotTime + m_shootInterval > Time.time)
                continue;
            else
                m_isReadyShoot = true;

            Shot(m_shootDirection);
		}

	}

    private Vector3 GetPredictionPos(Monster monster)
    {
        Vector3 fromTo = monster.transform.position - transform.position;
        Vector3 fromToXZ = new Vector3(fromTo.x, 0f, fromTo.z);

        float x = fromToXZ.magnitude;
        float y = fromTo.y;

        float AngleInRadians = m_angleInDegrees * Mathf.PI / 180f;

        float v2 = (Physics.gravity.y * x * x) / (2f * (y - Mathf.Tan(AngleInRadians) * x) * Mathf.Pow(Mathf.Cos(AngleInRadians), 2f));
        float v = Mathf.Sqrt(Mathf.Abs(v2));

        Vector3 targetVelocity = monster.GetComponent<Monster>().GetSpeed() * 100f;
        
        // Время полета снаряда
        float timeToHit = Vector3.Distance(transform.position, monster.transform.position) / v;

        // Прогнозируемая позиция цели
        Vector3 predictedTargetPosition = monster.transform.position + targetVelocity * timeToHit;

        return predictedTargetPosition;
    }

	private void Rotate()
	{
		m_cannonHub.localEulerAngles = new Vector3(Mathf.LerpAngle(m_cannonHub.localEulerAngles.x, -m_angleInDegrees, Time.fixedDeltaTime * m_rotationSpeed), 0f, 0f);
		Vector3 fromTo = m_shootDirection - transform.position;
        Vector3 fromToXZ = new Vector3(fromTo.x, 0f, fromTo.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(fromToXZ, Vector3.up), Time.fixedDeltaTime * m_rotationSpeed);
    }

	public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere (transform.position, m_range);
    }

	public void Shot(Vector3 target)
	{
		Vector3 fromTo = target - transform.position;
		Vector3 fromToXZ = new Vector3(fromTo.x, 0f, fromTo.z);

		//transform.rotation = Quaternion.LookRotation(fromToXZ, Vector3.up);

		float x = fromTo.magnitude;
		float y = fromTo.y;

		float AngleInRadians = m_angleInDegrees * Mathf.PI / 180f;

		float v2 = (Physics.gravity.y * x * x) / (2f * (y - Mathf.Tan(AngleInRadians) * x) * Mathf.Pow(Mathf.Cos(AngleInRadians), 2f));
		float v = Mathf.Sqrt(Mathf.Abs(v2));

        GameObject newBullet = Instantiate(m_projectilePrefab, m_shootPoint.position, m_shootPoint.rotation);
        newBullet.GetComponent<Rigidbody>().velocity = m_shootPoint.forward * v;

        m_isReadyShoot = false;
        m_lastShotTime = Time.time;
    }
}
