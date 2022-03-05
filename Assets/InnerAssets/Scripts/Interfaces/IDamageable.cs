using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JoyWayTest.Scripts.Interfaces
{
    public interface IDamageable
    {
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public void TakeDamage(float damage);
        public Action HealthChanged { get; set; }
    }
}
