using TMPro;
using UnityEngine;

public class UIResourceStatsManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _WoodText;
    [SerializeField] private TMP_Text _StoneText;
    [SerializeField] private TMP_Text _MetalText;

    private static int _woodCount;
    private static int _stoneCount;
    private static int _metalCount;
    
    public static void UpdateWood(int amount) => _woodCount = amount;

    public static void UpdateStone(int amount) => _stoneCount = amount;

    public static void UpdateMetal(int amount) => _metalCount = amount;

    private void Start()
    {
        _woodCount = 0;
        _stoneCount = 0;
        _metalCount = 0;
    }

    private void Update()
    {
        _WoodText.text = _woodCount.ToString();
        _StoneText.text = _stoneCount.ToString();
        _MetalText.text = _metalCount.ToString();
    }
}
