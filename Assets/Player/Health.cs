using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHp = 5;
    [SerializeField] private int _hp;

    public UnityEvent<int> Healed;
    public UnityEvent<int> Damaged;
    public UnityEvent Died;
    public int MaxHp => _maxHp;

    public int Hp
    {
        get => _hp;
        private set
        {
            var isDamage = value < _hp;
            _hp = Mathf.Clamp(value, 0, _maxHp);
            if (isDamage)
            {
                Damaged?.Invoke(_hp);
            }
            else
            {
                Healed?.Invoke(_hp);
            }
            if (_hp <= 0)
            {
                Died?.Invoke();
            }
        }
    }
    public void Awake()
    {
        _hp = _maxHp - 2;
    }

    public void Damage()
    {
        _hp -= 1;
    }

    public void Heal()
    {
        _hp += 1;
    }

    public void Kill()
    {
       _hp = 0;
    }


}
