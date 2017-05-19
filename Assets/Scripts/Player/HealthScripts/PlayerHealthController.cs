using System;
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Events;

public class PlayerHealthController : MonoBehaviour
{
    public int MaxHealth = 5;

    private int _currentHealth;

    public HealthBarController HealthBar;

    [Serializable]
    public class AssignmentCollisionEventEmitter : UnityEvent<AssignmentCollisionEvent>
    {}

    public AssignmentCollisionEventEmitter OnAssignmentCollision;

    // Use this for initialization
    void Start()
    {
        Health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.tag.Equals("Assignment")) return;

        // Allow event listeners to cancel the event
        AssignmentCollisionEvent e = new AssignmentCollisionEvent(other.gameObject);
        OnAssignmentCollision.Invoke(e);
        if (e.Cancelled) return;

        Health -= other.gameObject.GetComponent<AbstractAssignmentController>().Damage;
        Destroy(other.gameObject);
    }

    public int Health
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            HealthBar.SetHealth(_currentHealth);

            if (_currentHealth > MaxHealth)
            {
                _currentHealth = MaxHealth;
            }
            else if (_currentHealth <= 0)
            {
                // TODO Implement fail mechanism
            }
        }
    }
}