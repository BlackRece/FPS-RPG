using BlackRece;
using BlackRece.ProjectilePooler;
using UnityEngine;

[RequireComponent(typeof(ProjectilePooler))]
public class WeaponManager : MonoBehaviour
{
    
    [Space(10)] [Header("DEBUG Flags")]
    [SerializeField] private bool _IsFiring;
    [SerializeField] private bool _IsBursting;
    [SerializeField] private bool _IsReloading;

    private ProjectilePooler _pooler;
    private WeaponStats _weaponStats;
    
    private int _rankLevel;
    private Ticker _burstTicker;
    private Ticker _fireTicker;
    private Ticker _reloadTicker;
    private float _timer;
    private Vector3 _fireRot;
    private Vector3 _firePos;
    private WeaponStats _currentWeapon;
    private bool _canFire;
    
    private int _currentAmmoCount;          // total ammo
    private int _currentMagCount;
    private int _currentAmmoInMag;          // ammo in mag
    private int _currentBurstCount;

    private float _fireTickerProgress;

    public enum FireMode
    {
        Single,
        Burst,
        Auto
    }

    public enum WeaponType
    {
        Revolver,
        Pistol,
        SniperRifle,
        Shotgun,
        SubMachineGun,
        AssaultRifle,
        LargeMachineGun,
        Launcher,
    }
    
    private void Awake()
    {
        _pooler = GetComponent<ProjectilePooler>();
    }
    
    private void Start()
    {
        _pooler.Init();

        SetWeapon();
        RefillAmmo();
    }

    private void Update()
    {
        if (_IsReloading)
        {
            if (_reloadTicker.Tick()) 
                _IsReloading = false;
        }
        
        if (_IsFiring)
        {
            if(_fireTicker.Tick())
                _IsFiring = false;
        }
        
        // if(_IsBursting)
        // {
        //     if (_burstTicker.Tick())
        //     {
        //         _IsBursting = false;
        //         Shoot();
        //     }
        // }
        
        UIWeaponStatsManager.UpdateAmmoCount(_currentAmmoInMag, _currentAmmoCount);
        UIWeaponStatsManager.UpdateReloadBar(_reloadTicker.GetProgress());
        
        _fireTickerProgress = _fireTicker.GetProgress();
    }

    private void Shoot()
    {
        // _IsBursting = true;

        if (_currentAmmoInMag <= 0)
        {
            ReloadWeapon();
            return;
        }
        
        _IsFiring = true;
        _fireTicker.Reset();
        
        _currentAmmoInMag--;
        _currentBurstCount--;
        
        _pooler
            .GetGameObject()
            .GetComponent<Projectile>()
            .Init(_firePos, _fireRot);
    }
    
    public void SetWeapon()
    {
        // assign weapon stats
        _currentWeapon = WeaponStats.GetBasicRevolver();
        _canFire = true;
        
        // assign delay to timers
        _fireTicker = new Ticker(_currentWeapon.GetFireDelay());
        _burstTicker = new Ticker(_currentWeapon.GetBurstDelay());
        _reloadTicker = new Ticker(_currentWeapon.GetReloadDelay());
        
        
    }
    
    private void RefillAmmo()
    {
        _currentAmmoCount = GetMaxAmmoCount();
        _currentAmmoInMag = _currentWeapon.GetAmmoPerReload();
    }
    
    public void FireWeapon(Vector3 pos, Vector3 rot)
    {
        if(!_canFire)
            return;
        
        if (_IsFiring || _IsReloading)
            return;
        
        _firePos = pos;
        _fireRot = rot;

        switch (_currentWeapon.GetFireMode())
        {
            case FireMode.Single:
                FireSingle();
                break;
            case FireMode.Burst:
                FireBurst();
                break;
            case FireMode.Auto:
                FireAuto();
                break;
        }
    }
    
    private void FireSingle()
    {
        
        if (!_IsBursting)
        {
            _currentBurstCount = _currentWeapon.GetBurstCount();
            _burstTicker.Reset();
            //_IsBursting = true;
        }
        
        Shoot();
    }
    
    private void FireBurst()
    {
        
    }
    
    private void FireAuto()
    {
        
    }
    
    public void ReloadWeapon()
    {
        _IsReloading = true;
        _reloadTicker.Reset();
        
        var amountOfAmmoNeeded = _currentWeapon.GetAmmoPerReload() - _currentAmmoInMag;
        if (_currentAmmoCount > amountOfAmmoNeeded)
        {
            _currentAmmoCount -= amountOfAmmoNeeded;
            _currentAmmoInMag += amountOfAmmoNeeded;
        }
        else
        {
            _currentAmmoInMag += _currentAmmoCount;
            _currentAmmoCount = 0;
        }
    }

    private int GetMaxAmmoCount()
    {
        // number of magazines per weapon type
        var magCount = 0;
        
        switch (_currentWeapon.GetWeaponType())
        {
            case WeaponType.Revolver:
                magCount = 4;
                break;
            case WeaponType.Pistol:
                magCount = 8;
                break;
            case WeaponType.Shotgun:
                magCount = 6;
                break;
            case WeaponType.SniperRifle:
                magCount = 6;
                break;
            case WeaponType.SubMachineGun:
                magCount = 8;
                break;
            case WeaponType.AssaultRifle:
                magCount = 6;
                break;
            case WeaponType.LargeMachineGun:
                magCount = 4;
                break;
            case WeaponType.Launcher:
                magCount = 2;
                break;
        }
        
        return magCount * _currentWeapon.GetAmmoPerReload();
    }
}
