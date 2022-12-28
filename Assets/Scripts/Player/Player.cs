using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class Player : MonoBehaviour
{
    [SerializeField] private Crate _crate;

    [Header("Configurations")]
    private int _playerScore = 0;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _dieSound;

    [Header("Current Weapon")]
    [SerializeField] private Transform _weaponParent;
    [SerializeField] private List<Weapon> _weapons;

    private float _lastShotTime;
    private Weapon _currentWeapon;

    public Weapon CurrentWeapon => _currentWeapon;

    //private void OnEnable() => _coin.Collected += ChangeCountWallet;

    //private void OnDisable() => _coin.Collected -= ChangeCountWallet;

    private void OnEnable() => _crate.CollectWeapon += ChangeWeapons;

    private void OnDisable() => _crate.CollectWeapon -= ChangeWeapons;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _dieSound = GetComponent<AudioSource>();
        _currentWeapon = InstantiateWeapon(_weapons);
    }

    private void Update()
    {
        PlayerShoot();
        Debug.Log(_currentWeapon.WeaponData.Label);
    }

    private Weapon InstantiateWeapon(List<Weapon> weapons)
    {
        return Instantiate(_weapons[Random.Range(0, weapons.Count)], _weaponParent.position, transform.rotation, _weaponParent);
    }

    private void PlayerShoot()
    {
        if (Input.GetButtonDown("Fire1"))
            if (_lastShotTime <= 0)
            {
                _currentWeapon.Shoot();
                _lastShotTime = _currentWeapon.WeaponData.ShootingCooldown;
            }
        _lastShotTime -= Time.deltaTime;
    }

    private void ChangeScore(int score)
    {
        _playerScore += score;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Enemy enemy))
        {
            Destroy(gameObject);
        }
    }

    private void ChangeWeapons(Weapon weapon)
    {
        //Destroy(_currentWeapon);
        Debug.Log($"THIS: {weapon.WeaponData.Label}");
        _currentWeapon = Instantiate(weapon, _weaponParent.position, transform.rotation, _weaponParent);
    }

    //private IEnumerator Die()
    //{
    //    var waitTime = 0.5f;

    //    _animator.Play("Die");
    //    _dieSound.Play();

    //    yield return new WaitForSeconds(waitTime);
    //    Destroy(gameObject);
    //}
}