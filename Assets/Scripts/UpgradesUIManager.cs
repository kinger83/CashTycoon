using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesUIManager : MonoBehaviour
{
    private UpgradesManager _upgradeManager = null;
    private Text _upgradeForStore = null;
    private Text _upgradeName = null;
    private Text _upgradeDescription = null;
    private Text _upgradeCost = null;
    private Text _upgradeMultiplier = null;
    private Button _costButton = null;
    private Text _purchasedText = null;
    // Start is called before the first frame update
    private void OnEnable()
    {
        LoadGameData.OnLoadDataComplete += UpdateUpgradesUI;
    }

    private void Awake()
    {
        _costButton = transform.Find("UpgradesBuyButton").GetComponent<Button>();
        _purchasedText = transform.Find("Owned Text").GetComponent<Text>();
        _upgradeForStore = transform.Find("UpgradesStoreText").GetComponent<Text>();
        _upgradeName = transform.Find("UpgradesNameText").GetComponent<Text>();
        _upgradeDescription = transform.Find("UpgradesDescription").GetComponent<Text>();
        _upgradeCost = transform.Find("UpgradesBuyText").GetComponent<Text>();
        _upgradeMultiplier = transform.Find("UpgradesMultiplierText").GetComponent<Text>();
        _upgradeManager = GetComponent<UpgradesManager>();
    }


    // Update is called once per frame
    private void UpdateUpgradesUI()
    {
        _purchasedText.gameObject.SetActive(false);
        _upgradeForStore.text = _upgradeManager.UpgradeforStoreName;
        _upgradeName.text = _upgradeManager.UpgradeforStoreName;
        _upgradeForStore.text = _upgradeManager.UpgradeName;
        _upgradeDescription.text = _upgradeManager.UpgradeDescription;
        _upgradeCost.text = _upgradeManager.UpgradeCost.ToString("C2");
        _upgradeMultiplier.text = _upgradeManager.UpgradeMulitplier.ToString("C2");
    }


    public void SetHireButton(bool interactable)
    {
        _costButton.interactable = interactable;
    }

    public void RemoveHireButtonandPrice()
    {
        _costButton.gameObject.SetActive(false);
        _upgradeCost.gameObject.SetActive(false);
        _purchasedText.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        LoadGameData.OnLoadDataComplete -= UpdateUpgradesUI;
    }

}
