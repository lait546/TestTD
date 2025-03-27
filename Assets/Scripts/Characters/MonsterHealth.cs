using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour, IHealth
{
    [SerializeField] private readonly int m_maxHP = 30;
    [SerializeField] private Monster m_monster;

    public int CurHP { get; set; }

    void Start()
    {
        CurHP = m_maxHP;
    }

    public void TakeDamage(int value)
    {
        CurHP -= value;

        if (CurHP <= 0)
            Monster.Remove(m_monster);
    }
}
