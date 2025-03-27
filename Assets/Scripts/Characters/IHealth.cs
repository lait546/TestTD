using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public int CurHP { get; set; }

    public void TakeDamage(int value);
}
