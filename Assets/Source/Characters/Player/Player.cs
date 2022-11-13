using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Movement))]
public class Player : Characters
{
    [SerializeField] private float _health;

    private Movement _movement;
    private float _currentHealth;
    private State _currentState = State.Move;

    public event UnityAction<float> ChangedHealth;
    public event UnityAction Dying;

    public State CurrentState => _currentState;

    private void Start()
    {
        _movement = GetComponent<Movement>();
        _currentHealth = _health;
        _movement.ChangedState += OnChangedState;
    }

    private void OnDisable()
    {
        _movement.ChangedState -= OnChangedState;
    }

    public override void TakeDamage(float damage)
    {
        if (_currentState == State.Move)
        {
            _currentHealth = ChangeHealth(-damage);
            IsAlive();
        }
    }

    private void OnChangedState(State state)
    {
        _currentState = state;
    }

    private float ChangeHealth(float value)
    {
        float healthValue = Mathf.Clamp(_currentHealth + value, 0, _health);
        ChangedHealth?.Invoke(healthValue);
        Debug.Log($"здоровье: {healthValue}");
        return healthValue;
    }

    private void IsAlive()
    {
        if (_currentHealth == 0)
        {
            Dying?.Invoke();
            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentState == State.Attack)
        {
            if (other.TryGetComponent(out IDamageable damageable))
                Attack(damageable);
        }
    }
    /*    else
        {
            if (other.TryGetComponent(out Coin coin))
                Debug.Log($"GetCoin: {coin.name}");//coin.GetCoin();
        }
    }*/
}

public enum State
{
    Move,
    Attack
}