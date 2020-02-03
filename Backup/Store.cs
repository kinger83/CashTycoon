using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

enum StoreType { Food_DrinkStand,Pet_Store,SuperMarket,Hotel,Rental,Bank}
public class Store : MonoBehaviour
{
    
    private int _storeCount = 0;
    private int _nextStoreNumber = 0;
    private float _CurrentTimer = 0.0f;
    private float _actualNextCost = 0.0f;

    private bool _startTimer = false;
    private bool _storeUnlocked = false;
    private int _storeTimerDivisionValue = 5;

    public bool _ManagerAutoTimer { get; set; } = false;

    public float _ManagerMultiplyBonus { get; set; } = 1.0f;
    public float _ManagerTimerSpeedBonus { get; set; } = 1.0f;

    //private GameManager _gameManager = null;
    private CanvasGroup _cg = null;

    [SerializeField] float _baseStoreCost = 5.0f;
    [SerializeField] float _baseStoreUpgradeCost = 0.5f;
    [SerializeField] float _storeCostMultiplier = 1.25f;
    [SerializeField] float _storeProfitMuliplier = 1.3f;
    [SerializeField] float _baseStoreTimer = 4.0f;
    [SerializeField] float _baseStoreProfit = 0.5f;
    [SerializeField] Text __nextCost = null;
    [SerializeField] Text _storeCountText = null;
    [SerializeField] Slider _progressSlider = null;
    [SerializeField] GameObject _upgradeCostGameobject = null;
    [SerializeField] GameObject _openStoreGameobject = null;
    [SerializeField] GameObject _storeCostGameobject = null;
    [SerializeField] Button _openStoreButton = null;
    [SerializeField] Button _upgradeButton = null;
    [SerializeField] StoreType _storeType;


    // Start is called before the first frame update
    private void Awake()
    {
        _cg = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        
        _actualNextCost = _baseStoreCost;
        _progressSlider.value = 0.0f;
        _upgradeCostGameobject.SetActive(false);
        UpdateStoreCount(0);
        UpdateStoreCost();
        _cg.interactable = false;
        _cg.alpha = 0.0f;
        _storeUnlocked = false;
    }

    // Update is called once per frame
    void Update()
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
        }
        
    }
    public void OpenStore()
    {
        if(_actualNextCost > GameManager.instance.CurrentBalance) return;
        BuyStoreOnClick();
        _openStoreGameobject.SetActive(false);
        _storeCostGameobject.SetActive(false);
        _upgradeCostGameobject.SetActive(true);
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
            _progressSlider.value = _CurrentTimer / _baseStoreTimer;

            if (_CurrentTimer >= _baseStoreTimer)
            {
                if (!_ManagerAutoTimer)
                {
                    _startTimer = false;
                }
                _CurrentTimer = 0.0f;
                _progressSlider.value = 0.0f;
                CreditFunds();
            }
        }
    }

    private void CreditFunds()
    {
        GameManager.instance.Add_Subtract_Money(_baseStoreProfit *(Mathf.Pow(_storeProfitMuliplier , _storeCount)));
    }

    private void UpdateStoreCount(int amount)
    {
        _storeCount += amount;
        _storeCountText.text = _storeCount.ToString();
    }

    private void UpdateStoreCost()
    {
        if (_storeCount <= 0)
        {
            _actualNextCost = _baseStoreCost;
            __nextCost.text = _baseStoreCost.ToString("C2");
        }
        else
        {
            _actualNextCost = _baseStoreUpgradeCost * (Mathf.Pow(_storeCostMultiplier,_storeCount));
            if(_nextStoreNumber % _storeTimerDivisionValue == 0)
            {
                _actualNextCost *= 5.0f;
            }
            __nextCost.text = _actualNextCost.ToString("C2");
        }
    }

   private void CheckCanBuy()
    {
        float unlockValue = _actualNextCost * .85f;
        if(!_storeUnlocked && GameManager.instance.CurrentBalance >= unlockValue)
        {
            _cg.interactable = true;
            _cg.alpha = 1.0f;
            _storeUnlocked = true;
        }

        if(_actualNextCost <= GameManager.instance.CurrentBalance)
        {
            _openStoreButton.interactable = true;
            _upgradeButton.interactable = true;
        }
        else
        {
            _openStoreButton.interactable = false;
            _upgradeButton.interactable = false;
        }
    }

    
}
