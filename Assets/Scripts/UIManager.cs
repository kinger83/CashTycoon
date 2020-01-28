using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum state { Game, Managers}
    public static UIManager instance;

    [SerializeField] Text _currentBalanceText = null;
    [SerializeField] GameObject _companyName = null;

    private CanvasGroup _storePanel = null;
    private CanvasGroup _managerPanel = null;

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
        _managerPanel = GameObject.Find("ManagerPanel").GetComponent<CanvasGroup>();

    }

    void Start()
    {
        OnShowMain();
    }

   
   private void OnShowManagers()
    {
        _currentState = state.Managers;
        HideStorePanel();
        ShowManagerPanel();

    }

    private void ShowManagerPanel()
    {
        _managerPanel.alpha = 1.0f;
        _managerPanel.interactable = true;
        _managerPanel.blocksRaycasts = true;
    }

    private void HideStorePanel()
    {
        _storePanel.alpha = 0.0f;
        _storePanel.interactable = false;
        _storePanel.blocksRaycasts = false;
    }

    private void OnShowMain()
    {
        _currentState = state.Game;
        HideManagerPanel();
        ShowStorePanel();

    }

    private void ShowStorePanel()
    {
        _storePanel.alpha = 1.0f;
        _storePanel.interactable = true;
        _storePanel.blocksRaycasts = true;
    }

    private void HideManagerPanel()
    {
        _managerPanel.alpha = 0.0f;
        _managerPanel.interactable = false;
        _managerPanel.blocksRaycasts = false;
    }

    public void OnManagerButtonClick()
    {
        if(_currentState == state.Game)
        {
            OnShowManagers();
        }
        else
        {
            OnShowMain();
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
