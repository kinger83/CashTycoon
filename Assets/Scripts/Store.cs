using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

enum StoreType { Food_DrinkStand,Pet_Store,SuperMarket,Hotel,Rental,Bank}
public class Store : MonoBehaviour
{
    private StoreUIManager _storeUiManager;
    
    private int _storeCount = 0;
    private int _nextStoreNumber = 0;
    private float _CurrentTimer = 0.0f;
    private float _actualNextCost = 0.0f;
    private float _minTimerTime = 1.0f;

    private bool _startTimer = false;
    private bool _storeUnlocked  = false;
    public bool _storeOpen { get; private set; } = false;

    private int _storeTimerDivisionValue = 5;
    private float _upgradeMultilier = 1.0f;

    [SerializeField] float _baseStoreCost = 5.0f;
    [SerializeField] float _baseStoreUpgradeCost = 0.5f;
    [SerializeField] float _baseStoreProfit = 0.5f;
    [SerializeField] float _baseStoreTimer = 4.0f;
    [SerializeField] float _storeCostMultiplier = 1.25f;
    [SerializeField] float _storeProfitMuliplier = 1.3f;
    [SerializeField] StoreType _storeType;

    public bool ManagerAutoTimer { get; set; } = false;
    public float _ManagerMultiplyBonus { get; set; } = 1.0f;
    public float _ManagerTimerSpeedBonus { get; set; } = 1.0f;
    public float ProgressTimerValue { get; private set; }

    public float BaseStoreCost { get { return _baseStoreCost; } set { _baseStoreCost = value; } }
    public float BaseStoreUpgradeCost { get { return _baseStoreUpgradeCost; } set { _baseStoreUpgradeCost = value; } }
    public float BaseStoreProfit { get { return _baseStoreProfit; } set { _baseStoreProfit = value; } }
    public float BaseStoreTimer { get { return _baseStoreTimer; } set { _baseStoreTimer = value; } }
    public float BaseStoreCostMultiplier { get { return _storeCostMultiplier; } set { _storeCostMultiplier = value; } }
    public float BaseStoreProfitMultiplier { get { return _storeProfitMuliplier; } set { _storeProfitMuliplier = value; } }
    public string StoreName { get; set; } = null;






    public void UnlockStoreManager()
    {
        ManagerAutoTimer = true;
        StoreStartTimerOnClick();
    }



    // Start is called before the first frame update
    private void Awake()
    {
        _storeUiManager = GetComponent<StoreUIManager>();
    }
    private void OnEnable()
    {
        LoadGameData.OnLoadDataComplete += StoreStartProcedure ;
    }
    private void Start()
    {
        StoreStartProcedure();
    }

    private void StoreStartProcedure()
    {
        _actualNextCost = _baseStoreCost;
        UpdateStoreCount(0);
        UpdateStoreCost();

        _storeUnlocked = false;
    }

    private void Update()
    {
        TimerControl();
        CheckCanBuy();
    }

    public void BuyStoreOnClick()
    {
        if (_actualNextCost > GameManager.instance.CurrentBalance) return;
        GameManager.instance.Add_Subtract_Money(-_actualNextCost);
        UpdateStoreCount(1);
        _nextStoreNumber = _storeCount + 1;
        UpdateStoreCost();

        if(_storeCount % _storeTimerDivisionValue == 0)
        {
            _baseStoreTimer /= 2;
            if (_baseStoreTimer < _minTimerTime)
                _baseStoreTimer = _minTimerTime;
        }
        
    }

    public void OpenStore()
    {
        if(_actualNextCost > GameManager.instance.CurrentBalance) return;
        BuyStoreOnClick();
        _storeOpen = true;
        _storeUiManager.UpDateUIButtons(true, false, false);
    }

    public void StoreStartTimerOnClick()
    {
        if (_storeCount <= 0) return;

        if (_startTimer == false)
        {
            _startTimer = true;
            _CurrentTimer = 0.0f;
        }
    }

    private void TimerControl()
    {
        if (_startTimer)
        {
            _CurrentTimer += Time.deltaTime * _ManagerTimerSpeedBonus;

            ProgressTimerValue = _CurrentTimer / _baseStoreTimer;

            if (_CurrentTimer >= _baseStoreTimer)
            {
                if (!ManagerAutoTimer)
                {
                    _startTimer = false;
                }
                _CurrentTimer = 0.0f;
                ProgressTimerValue = 0.0f;
                CreditFunds();
            }
        }
    }

    private void CreditFunds()
    {
        GameManager.instance.Add_Subtract_Money(_baseStoreProfit *(Mathf.Pow(_storeProfitMuliplier , _storeCount))*_upgradeMultilier);
    }

    private void UpdateStoreCount(int amount)
    {
        _storeCount += amount;
        _storeUiManager.UpdateStrings(_storeCount, _actualNextCost);
    }


    private void UpdateStoreCost()
    {
        if (_storeCount <= 0)
        {
            _actualNextCost = _baseStoreCost;
            _storeUiManager.UpdateStrings(_storeCount, _actualNextCost);
        }
        else
        {
            _actualNextCost = _baseStoreUpgradeCost * (Mathf.Pow(_storeCostMultiplier,_storeCount));
            if(_nextStoreNumber % _storeTimerDivisionValue == 0)
            {
                _actualNextCost *= 5.0f;
            }
            _storeUiManager.UpdateStrings(_storeCount, _actualNextCost);
        }
    }

    private void CheckCanBuy()
    {
        float unlockValue = _actualNextCost * .85f;
        
        if (_storeUnlocked == false)
            
        {
            if(GameManager.instance.CurrentBalance < unlockValue)
            {
                _storeUiManager.UpdateCanvasGroup(false, 0, true, false);
            }

            else if (GameManager.instance.CurrentBalance >= unlockValue)
            {
                _storeUiManager.UpdateCanvasGroup(true, 1, true, true);

                _storeUnlocked = true;
            }
        }

        if (_storeUnlocked)
        {
            if (_actualNextCost <= GameManager.instance.CurrentBalance)
            {
                _storeUiManager.UpdateCanvasGroup(true, 1, true, true);
            }
            else
            {
                _storeUiManager.UpdateCanvasGroup(true, 1, false, false);

            }
        }
    }

    public void AddUpgradeMultipiler(float multiplier)
    {
        _upgradeMultilier += multiplier;
    }

    private void OnDisable()
    {
        LoadGameData.OnLoadDataComplete -= StoreStartProcedure;
    }
}
