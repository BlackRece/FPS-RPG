using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _UpgradePanelPrefab;
    [SerializeField] private GameObject _HUDPanelPrefab;

    private static GameObject _upgradePanel;
    private static GameObject _hudPanel;
    
    private void Awake()
    {
        if (_UpgradePanelPrefab == null)
            throw new ArgumentNullException("ERROR: UpgradePanel is null!");
        _upgradePanel = _UpgradePanelPrefab;
        
        if (_HUDPanelPrefab == null)
            throw new ArgumentNullException("ERROR: HUDPanel is null!");
        _hudPanel = _HUDPanelPrefab;
    }
    
    private void Start()
    {
        _upgradePanel.SetActive(false);
        _hudPanel.SetActive(true);
    }
    
    public static void ToggleUpgradePanel(bool bActive)
    {
        _upgradePanel.SetActive(bActive);
        _hudPanel.SetActive(!bActive);
    }
}
