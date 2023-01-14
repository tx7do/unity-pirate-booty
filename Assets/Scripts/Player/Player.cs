using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _deathParticles;
    [SerializeField] private PlayerSpawner _spawner;

    [Header("Weapons")]
    [SerializeField] private Transform _weaponParent;
    private Weapon[] _weapons;

    private float _lastShotTime;
    private Weapon _currentWeapon;
    private SoundManager _soundManager;

    public event Action GameOver;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _soundManager = ServiceLocator.Get<SoundManager>();
        _spawner = ServiceLocator.Get<PlayerSpawner>();
        _weapons = _weaponParent.GetComponentsInChildren<Weapon>(true);
        _currentWeapon = _weapons[0];
    }

    private void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.X))
            if (_lastShotTime <= 0)
            {
                _currentWeapon.Shoot();
                _lastShotTime = _currentWeapon.WeaponData.ShootingCooldown;
            }
        _lastShotTime -= Time.deltaTime;
    }
    public void ChangeWeapon(int randomWeaponIndex)
    {
        _currentWeapon.gameObject.SetActive(false);
        _currentWeapon = _weapons[randomWeaponIndex];
        _currentWeapon.gameObject.SetActive(true);
    }

    public void ResetPlayer()
    {
        _spawner.Spawn(this);
    }

    public void Die()
    {
        Instantiate(_deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}