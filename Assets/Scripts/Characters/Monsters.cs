using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Monsters", fileName = "Monsters")]
public class Monsters : ScriptableObject
{
    private static Monsters m_monsters;

    public List<Monster> MonstersList = new List<Monster>();

    public static Monsters Get()
    {
        if (!m_monsters)
            m_monsters = Resources.Load<Monsters>("Monsters");

        return m_monsters;
    }
}
