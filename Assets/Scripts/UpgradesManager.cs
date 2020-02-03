using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    public GameObject UpgradeforStoreNameGameObject { get; set; } = null;
    public string UpgradeforStoreName { get; set; } = null;
    public string UpgradeName { get; set; } = null;
    public float UpgradeCost { get; set; } = 0.0f;
    public float UpgradeMulitplier { get; set; } = 0.0f;
    public string UpgradeDescription { get; set; } = null;
    
    
    private bool _upgradePurchased = false;
    private Store _upgradeForStore = null;
    private UpgradesUIManager _uiManager = null;

    // Start is called before the first frame update
    private void OnEnable()
    {
        GameManager.OnUpdateBalance += CheckCanBuy;
        LoadGameData.OnLoadDataComplete += CheckCanBuy;
        LoadGameData.OnLoadDataComplete += SetStore;
    }

    private void Awake()
    {
        _uiManager = GetComponent<UpgradesUIManager>();
    }
    private void SetStore()
    {
        _upgradeForStore = UpgradeforStoreNameGameObject.GetComponent<Store>();
    }

    private void CheckCanBuy()
    {
        if (UpgradeCost <= GameManager.instance.CurrentBalance)
        {
            _uiManager.SetHireButton(true);
        }
        else
        {
            _uiManager.SetHireButton(false);

        }
    }


    public void OnPurchaseClick()
    {
       

        if (_upgradePurchased == true) return;
        if (_upgradeForStore._storeOpen == false)
        {
            print("here");
            Debug.Log("Need to own Store First");
            return;
        }

        if (GameManager.instance.CurrentBalance < UpgradeCost) return;

        GameManager.instance.Add_Subtract_Money(-UpgradeCost);
        _uiManager.RemoveHireButtonandPrice();
        _upgradeForStore.AddUpgradeMultipiler(UpgradeMulitplier);
    }



    private void OnDisable()
    {
        GameManager.OnUpdateBalance -= CheckCanBuy;
        LoadGameData.OnLoadDataComplete -= CheckCanBuy;

    }
}
