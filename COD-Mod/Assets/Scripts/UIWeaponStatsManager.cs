using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponStatsManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _WeaponNameText;
    
    [SerializeField] private TMP_Text _AmmoCountText;
    [SerializeField] private TMP_Text _AmmoInMagText;
    
    [SerializeField] private Image _WeaponReloadBar;
    
    private static int _ammoCount;
    private static int _ammoInMagCount;
    private static float _reloadBarProgress;

    public static void UpdateAmmoCount(int currentAmmoInMag, int currentAmmoCount)
    {
        _ammoCount = currentAmmoCount;
        _ammoInMagCount = currentAmmoInMag;
    }

    private void Update()
    {
        _AmmoCountText.text = _ammoCount.ToString();
        _AmmoInMagText.text = _ammoInMagCount.ToString();
        
        _WeaponReloadBar.fillAmount =  _reloadBarProgress;
    }

    public static void UpdateReloadBar(float fProgress)
    {
        _reloadBarProgress = fProgress;
    }
}
