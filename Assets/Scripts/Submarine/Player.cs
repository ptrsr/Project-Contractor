using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Singleton
{
    public class Player : MonoBehaviour
    {
        private SubMovement _movement;
        public underwaterFX _fx;
        private ParticleSystem _particles;

        [SerializeField]
        private float
            _oxygen = 10000,
            _oxygenLossOnHit = 100;

        private float _maxOxygen;

        private static Player _instance;

        public static Player Instance
        {
            get { return _instance; }
            private set { _instance = value; }
        }

        private void Start()
        {
            if (_instance != null)
            {
                Debug.LogWarning("Can't have 2 instances of player!");
                Destroy(gameObject);
            }

            _instance = this;

            _maxOxygen = _oxygen;

            _movement = GetComponent<SubMovement>();
            _particles = GetComponentInChildren<ParticleSystem>();
        }

        private void Update()
        {
            Remove(Time.deltaTime);
        }

        public void AddOxygen(float value)
        {
            _oxygen += value;
            ChangeValues();

        }
        public void Remove(float value)
        {
            _oxygen -= value;
            ChangeValues();
        }

        private void ChangeValues()
        {
            if (_oxygen > 0)
                _fx._vignette.range = _oxygen / _maxOxygen;

            if (_oxygen < 0)
                _movement.Surface();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Oxygen")
            {
                Oxygen bubble = other.gameObject.GetComponent<Oxygen>();
                AddOxygen(bubble.Amount);
                Destroy(other.gameObject);
            }
            else if (other.gameObject.tag == "Shark")
            {
                Remove(_oxygenLossOnHit);
            }

        }
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Wall")
            {
                Remove(_oxygenLossOnHit);
                _particles.Play();
            }
        }
    }
}