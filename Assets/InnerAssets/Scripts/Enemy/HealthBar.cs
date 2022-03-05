using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoyWayTest.Scripts.Interfaces;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

namespace JoyWayTest.Scripts.Enemy
{
    [RequireComponent(typeof(IDamageable))]
    public class HealthBar : MonoBehaviour
    {
        private IDamageable _iDamageable;
        [SerializeField] private Canvas _canvas;
        private Transform _canvasTransform;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Image _fillArea;
        [SerializeField] private TMP_Text _healthValue;
        [SerializeField] private Gradient _healthGradient;
        private Transform _cameraTransform;

        private void Start()
        {
            Camera camera = Camera.main;
            _cameraTransform = camera.transform;
            _canvas.worldCamera = camera;
            _canvasTransform = _canvas.transform;
            _iDamageable = GetComponent<IDamageable>();

            _healthBar.maxValue = _iDamageable.MaxHealth;
            _healthBar.value = _iDamageable.CurrentHealth;


            _iDamageable.HealthChanged += ChangeHealth;
        }

        private void ChangeHealth()
        {
            _healthBar.DOValue(_iDamageable.CurrentHealth, 
                        (_healthBar.value - _iDamageable.CurrentHealth)/_iDamageable.MaxHealth);
            _healthValue.text = ((int)_iDamageable.CurrentHealth).ToString();
            _fillArea.color = _healthGradient.Evaluate(_healthBar.normalizedValue);
        }

        private void OnDestroy()
        {
            _iDamageable.HealthChanged -= ChangeHealth;
        }

        private void LateUpdate()
        {
            _canvasTransform.LookAt(_cameraTransform);
        }
    }
}
