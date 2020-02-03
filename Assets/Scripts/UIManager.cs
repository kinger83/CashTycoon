using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum state { Game, Managers, Upgrades}
    public static UIManager instance;

    [SerializeField] Text _currentBalanceText = null;
    [SerializeField] GameObject _companyName = null;

    private CanvasGroup _storePanel = null;
    private CanvasGroup _managerPanel = null;
    private CanvasGroup _upgradesPanel = null;
    private GameObject _closeButton = null;

    private state _currentState;

    private void OnEnable()
    {
        GameManager.OnUpdateBalance += UpdateUI;
        LoadGameData.OnLoadDataComplete += UpDateCompanyInfo;
    }
    private void OnDisable()
    {
        GameManager.OnUpdateBalance -= UpdateUI;
        LoadGameData.OnLoadDataComplete -= UpDateCompanyInfo;
    }
    private void Awake()
    {
        _companyName = GameObject.Find("Canvas/CompanyName");
        _storePanel = GameObject.Find("StorePanel").GetComponent<CanvasGroup>();
        _upgradesPanel = GameObject.Find("UpgradesPanel").GetComponent<CanvasGroup>();
        _managerPanel = GameObject.Find("ManagerPanel").GetComponent<CanvasGroup>();
        _closeButton = GameObject.Find("Canvas/CloseButton");

    }

    void Start()
    {
        OnShowMain();
    }

   
   private void OnShowManagers()
    {
        _currentState = state.Managers;
        _closeButton.SetActive(true);
        HideStorePanel();
        HideUpgradesPanel();
        ShowManagerPanel();
    }

    private void OnShowMain()
    {
        _currentState = state.Game;
        _closeButton.SetActive(false);
        HideManagerPanel();
        HideUpgradesPanel();
        ShowStorePanel();
    }

    private void OnShowUpgrades()
    {
        _currentState = state.Upgrades;
        _closeButton.SetActive(true);
        HideManagerPanel();
        HideStorePanel();
        ShowUpgradesPanel();
    }

    private void ShowStorePanel()
    {
        _storePanel.alpha = 1.0f;
        _storePanel.interactable = true;
        _storePanel.blocksRaycasts = true;
    }
    private void HideStorePanel()
    {
        _storePanel.alpha = 0.0f;
        _storePanel.interactable = false;
        _storePanel.blocksRaycasts = false;
    }
    private void ShowManagerPanel()
    {
        _managerPanel.alpha = 1.0f;
        _managerPanel.interactable = true;
        _managerPanel.blocksRaycasts = true;

    }
    private void HideManagerPanel()
    {
        _managerPanel.alpha = 0.0f;
        _managerPanel.interactable = false;
        _managerPanel.blocksRaycasts = false;
    }
    private void ShowUpgradesPanel()
    {
        _upgradesPanel.alpha = 1.0f;
        _upgradesPanel.interactable = true;
        _upgradesPanel.blocksRaycasts = true;

    }
    private void HideUpgradesPanel()
    {
        _upgradesPanel.alpha = 0.0f;
        _upgradesPanel.interactable = false;
        _upgradesPanel.blocksRaycasts = false;
    }

    public void CloseButton()
    {
        OnShowMain();
    }

    public void OnManagerButtonClick()
    {
        if(_currentState == state.Managers)
        {
            OnShowMain();
        }
        else
        {
            OnShowManagers();
        }
    }

    public void OnUpgradesButtonClick()
    {
        if (_currentState == state.Upgrades)
        {
            OnShowMain();
        }
        else
        {
            OnShowUpgrades();
        }
    }

    public void UpdateUI()
    {
        _currentBalanceText.text = GameManager.instance.CurrentBalance.ToString("C2");
    }

    private void UpDateCompanyInfo()
    {
        SetCompanyName(GameManager.instance.CompanyName);
    }

    private void SetCompanyName(string companyName)
    {
        _companyName.GetComponent<Text>().text = companyName;
    }

    
}
