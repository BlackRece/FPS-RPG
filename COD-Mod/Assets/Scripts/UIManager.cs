using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _UpgradePanelPrefab;
    [SerializeField] private GameObject _HUDPanelPrefab;

    private static GameObject _upgradePanel;
    private static GameObject _hudPanel;

    private int _woodAmount;
    private int _stoneAmount;
    private int _metalAmount;
    
    private static float _rankLevel = 0;
    
    private static int _wood;
    private static int _stone;
    private static int _metal;
    
    private void Awake()
    {
        if (_UpgradePanelPrefab == null)
            throw new ArgumentNullException("ERROR: UpgradePanel is null!");
        _upgradePanel = _UpgradePanelPrefab;
        
        if (_HUDPanelPrefab == null)
            throw new ArgumentNullException("ERROR: HUDPanel is null!");
        _hudPanel = _HUDPanelPrefab;
        
        _wood = 0;
    }
    
    private void Start()
    {
        _upgradePanel.SetActive(false);
        _hudPanel.SetActive(true);
    }
    
    private void Update()
    {
        _woodAmount = _wood;
        _stoneAmount = _stone;
        _metalAmount = _metal;
        
        UIResourceStatsManager.UpdateWood(_woodAmount);
        UIResourceStatsManager.UpdateStone(_stoneAmount);
        UIResourceStatsManager.UpdateMetal(_metalAmount);
    }
    
    public static void ToggleUpgradePanel(bool bActive)
    {
        _upgradePanel.SetActive(bActive);
        _hudPanel.SetActive(!bActive);
    }

    public static void IncreaseRankLevel() => _rankLevel++;
    
    public static void IncreaseAllResources(int amount)
    {
        _wood += amount;
        
        if(_rankLevel > 2)
            _stone += amount;
        
        if(_rankLevel > 4)
            _metal += amount;
    }
}
