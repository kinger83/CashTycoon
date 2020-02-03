using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    private ManagerUiManager _uiManager = null;
    private Store _managedStoreScript = null;
    private bool _managerHired = false;
    public string StoreBeingManaged { get; set; } = null;
    public string ManagerName { get; set; } = null;
    public float ManagerHireCost { get; set; }
    public float ManagerCut { get; set; }
    public GameObject _managedStore = null;

    // Start is called before the first frame update
    private void OnEnable()
    {
        GameManager.OnUpdateBalance += CheckCanBuy;
        LoadGameData.OnLoadDataComplete += CheckCanBuy;
    }

    private void Awake()
    {
        _uiManager = GetComponent<ManagerUiManager>();
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPurchaseManager()
    {
        _managedStoreScript = _managedStore.GetComponent<Store>();
        if (_managerHired == true) return;
        if(_managedStoreScript._storeOpen == false)
        {
            Debug.Log("Need to own Store First");
            return;
        }
        if (GameManager.instance.CurrentBalance < ManagerHireCost) return;
        GameManager.instance.Add_Subtract_Money(-ManagerHireCost);
        _uiManager.RemoveHireButtonandPrice();
        _managedStoreScript.UnlockStoreManager();
    }

    private void CheckCanBuy()
    {
        if (ManagerHireCost <= GameManager.instance.CurrentBalance)
        {
            _uiManager.SetHireButton(true);
        }
        else
        {
            _uiManager.SetHireButton(false);
          
        }
    }

    private void OnDisable()
    {
        GameManager.OnUpdateBalance -= CheckCanBuy;
        LoadGameData.OnLoadDataComplete -= CheckCanBuy;

    }
}
