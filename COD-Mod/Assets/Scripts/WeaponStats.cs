public class WeaponStats
{
    private string _name;           // name of weapon
    private int _rankLevel;         // level of weapon, 1-5, 1 being lowest
    private int _damage;            // damage of weapon's projectile
    private int _price;             // price of weapon
    private int _ammoCapacity;      // max ammo capacity
    private int _ammoCount;         // current ammo count
    private int _ammoPerReload;     // ammo count per reload
    private int _range;             // range of weapon's projectile
    private float _reloadTime;      // time it takes to reload weapon
    private float _fireRate;        // number of projectiles fired per second
    private float _fireDelay;       // delay between projectiles fired
    private int _burstCount;        // number of projectiles fired in a burst
    private float _burstDelay;      // delay between bursts
    private bool _isAutomatic;      // is weapon automatic

    private WeaponManager.FireMode _fireMode;
    private WeaponManager.WeaponType _weaponType;
    
    private float _BurstDuration => _fireDelay <= 0f ? _burstCount : _burstCount * _fireDelay;
    private float _FireDuration => _BurstDuration + _burstDelay;
    public WeaponManager.FireMode GetFireMode() => _fireMode;
    public float GetFireRate(float fSeconds) => fSeconds / _FireDuration;

    public static WeaponStats GetBasicRevolver()
    {
        return new WeaponStats()
        {
            _name = "Basic Revolver",
            _rankLevel = 0,
            _damage = 1,
            _price = 10,
            
            _ammoCapacity = 6,
            _ammoCount = 6,
            _ammoPerReload = 6,
            
            _range = 10,
            _reloadTime = 1f,
            
            _fireRate = 0f,
            _fireDelay = 0.5f,
            _burstCount = 1,
            _burstDelay = 0f,
            
            _fireMode = WeaponManager.FireMode.Single,
            _weaponType = WeaponManager.WeaponType.Revolver,
            _isAutomatic = false
        };
    }

    public float GetFireDelay() => _fireDelay;
    public float GetBurstDelay() => _burstDelay;
    public float GetReloadDelay() => _reloadTime;
    public WeaponManager.WeaponType GetWeaponType() => _weaponType;
    public int GetAmmoPerReload() => _ammoPerReload;
    public int GetBurstCount() => _burstCount;
    
    /*
     Taken from https://www.reddit.com/r/ShadowgunLegends/comments/8npx26/burst_rifle_actual_damage_per_second_calculations/
     * The Math – Calculating actual fire rate and burst delay:
     Take the magazine size and divide by the time it took to empty it. Multiply this by 60. 
     This is the actual fire rate of your burst rifle in rounds per minute.
     
     The burst delay should be almost exactly 0.3 no matter what gun you use. However, if you
     want to confirm this then take your magazine size, divide by the fire rate listed on the
     card, and multiply by 60. If your rifle had no burst functionality this is how much time it
     would take to empty a magazine. Take the measured time to empty a magazine and subtract the
     above calculated number from it, and divide by the number of bursts fired. You should get a
     value that is close to 0.3 seconds.
    */
    
    /*
     Taken from https://moderndayfirearms.fandom.com/wiki/Rate_of_Fire
     * RPM Ranges
        0-100: Very Slow, barely effective
        101-250: Slow, somewhat useful
        251-500: Average, useful enough
        501-750: Fast, not common
        751-1000: Very fast, rarely common
        1001+: Extremely fast, extremely rare in commonality

    * RPMs based on Weapon Type
        Sniper Rifles: These rifles will have the lowest, and is given a 0-100 RPM.
        SMGs: The SMG class consists of rapid-fire weapons, and are given 501-1001+ RPM.
        Assault Rifles: Assault rifles deliver stopping power at a fast pace, but is slow compared to SMGs, and are given a 251-750 RPM.
        Shotguns: Shotguns have a wide array of designs, but pump-actions are the slowest, due to reload times, and are given 0-100 RPM. Semi-autos and automatics are given 0-250 RPM.
        LMGs: LMGs have medium to fast RPMs and are rated a 501-1001+.

        Launchers: Although they have no definite RPM, they still get a rating, and the lowest one, too. (not added yet)
        
        [custom weapon rates - not from website]
        Revolvers: Revolvers use the slowest bolt-action/flintlock/hammer firing mechanisms, and are given a 0-100 RPM.        
        Pistols: Pistols with auto and semi-auto fire modes are faster than revolvers, and are given a 251-1001+ RPM.
     */
}