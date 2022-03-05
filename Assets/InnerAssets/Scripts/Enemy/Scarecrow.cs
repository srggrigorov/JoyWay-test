using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoyWayTest.Scripts.Interfaces;
using System;

namespace JoyWayTest.Scripts.Enemy
{
    [RequireComponent(typeof(Collider))]
    public class Scarecrow : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _maxHealth;
        public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
        [HideInInspector] public float CurrentHealth { get; set; }
        [SerializeField] private float _maxWet = 100;
        [Min(0)] private float _wet = 0;
        [SerializeField, ColorUsage(true, true)] private Color _fireColor;
        [SerializeField, ColorUsage(true, true)] private Color _wetColor;
        [SerializeField] private GameObject _explode;
        private bool _onFire = false;
        private List<Material> _materials = new List<Material>();
        private float _fireDamage = 5;
        private float _fireTime = 10;

        #region PropertiesIDs
        private int _colorPropID;
        private int _enableEmissionPropID;
        private int _fresnelPowerPropID;
        #endregion

        private float _fireTimeElapsed = 0;
        public Action HealthChanged { get; set; }

        private void Awake()
        {
            _materials.AddRange(GetComponentInChildren<Renderer>().materials);
            _colorPropID = Shader.PropertyToID("_EmissionColor");
            _enableEmissionPropID = Shader.PropertyToID("_EmissionEnabled");
            _fresnelPowerPropID = Shader.PropertyToID("_FresnelPower");

            Reload();
        }


        public void TakeDamage(float damage)
        {
            if (_onFire)
            {
                CurrentHealth -= damage + 10;
            }
            else if (_wet > 0)
            {
                CurrentHealth -= Mathf.Clamp(damage - 10, 0, damage - 10);
            }
            else CurrentHealth -= damage;
            HealthChanged.Invoke();
            CheckHealth();
        }

        public void SetOnFire()
        {
            CurrentHealth--;
            HealthChanged.Invoke();
            if (_onFire)
            {
                _fireTimeElapsed = 0;
                return;
            }
            else if (_wet > 0)
            {
                _wet--;
                if (_wet <= 0) _materials.ForEach(material => material.SetInt(_enableEmissionPropID, 0));
                return;
            }
            else
            {
                _materials.ForEach(material =>
                {
                    material.SetColor(_colorPropID, _fireColor);
                    material.SetFloat(_fresnelPowerPropID, 1);
                    material.SetInt(_enableEmissionPropID, 1);
                });
                _onFire = true;
                StartCoroutine(Ignite());
            }
            CheckHealth();
        }

        private IEnumerator Ignite()
        {
            if (!_onFire) yield break;
            yield return new WaitForSeconds(1);
            CurrentHealth -= _fireDamage;
            HealthChanged.Invoke();
            CheckHealth();
            _fireTimeElapsed++;
            if (_fireTimeElapsed >= _fireTime)
            {
                _onFire = false;
                _materials.ForEach(material => material.SetInt(_enableEmissionPropID, 0));
            }
            else StartCoroutine(Ignite());
        }

        public void GetWet(float amount)
        {
            _onFire = false;
            StopCoroutine(Ignite());
            _materials.ForEach(material =>
               {
                   if (_wet <= 0)
                   {
                       material.SetColor(_colorPropID, _wetColor);
                       material.SetInt(_enableEmissionPropID, 1);
                   }
                   material.SetFloat(_fresnelPowerPropID, -0.09f * _wet + 10);
               });
            _wet = Mathf.Clamp(_wet + amount, 0, _maxWet);
        }

        private void CheckHealth()
        {
            if (CurrentHealth <= 0) Destroy(gameObject);
        }

        public void Reload()
        {

            CurrentHealth = MaxHealth;
            _wet = 0;
            _onFire = false;
            _materials.ForEach(material => material.SetInt(_enableEmissionPropID, 0));
        }

        private void OnDestroy()
        {
            Instantiate(_explode, transform.position, transform.rotation);
        }
    }
}
