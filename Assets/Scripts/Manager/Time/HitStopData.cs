using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/HitStop", order = 0)]
public class HitStopData : ScriptableObject
{
    [SerializeField] private float duration;
    [SerializeField] private float timeScale;
    
    public float Duration => duration;
    public float TimeScale => timeScale;
}

public enum HitStopType {Small, Normal, Large, Custom }
