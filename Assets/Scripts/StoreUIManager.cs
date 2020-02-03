using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreUIManager : MonoBehaviour
{
    private Store _store;
    private CanvasGroup _cg = null;

    [SerializeField] Text _storeName = null;
    [SerializeField] Slider _progressSlider = null;
    [SerializeField] GameObject _upgradeCostGameobject = null;
    [SerializeField] GameObject _openStoreGameobject = null;
    [SerializeField] GameObject _storeCostGameobject = null;
    [SerializeField] Button _openStoreButton = null;
    [SerializeField] Button _upgradeButton = null;

    [SerializeField] Text _nextCost = null;
    [SerializeField] Text _storeCountText = null;
    // Start is called before the first frame update

    private void OnEnable()
    {
        LoadGameData.OnLoadDataComplete += SetStoreName_Image;
    }
    private void Awake()
    {
        _store = GetComponent<Store>();
        _cg = GetComponent<CanvasGroup>();
    }
    
    void Start()
    {
        _progressSlider.value = 0.0f;
        _upgradeCostGameobject.SetActive(false);
        _cg.interactable = false;
        _cg.alpha = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        UpDateSlider();
    }

    private void UpDateSlider()
    {
        _progressSlider.value = _store.ProgressTimerValue;
    }

    public void UpDateUIButtons(bool upgradeCostGO, bool openStoreGO, bool storeCostGO)
    {
        _upgradeCostGameobject.SetActive(upgradeCostGO);
        _openStoreGameobject.SetActive(openStoreGO);
        _storeCostGameobject.SetActive(storeCostGO);
    }

    public void UpdateStrings(int storeCount, float storeCost)
    {
        _storeCountText.text = storeCount.ToString();
        _nextCost.text = storeCost.ToString("C2");
    }

    public void UpdateCanvasGroup(bool cgInteractable, float cgAlpha, bool openStoreButtonInteractable, bool upgradeStoreButtonInteractable)
    {
        _cg.interactable = cgInteractable;
        _cg.alpha = cgAlpha;
        _openStoreButton.interactable = openStoreButtonInteractable;
        _upgradeButton.interactable = upgradeStoreButtonInteractable;

    }

    private void SetStoreName_Image()
    {
        _storeName.text = _store.StoreName;
    }

    private void OnDisable()
    {
        LoadGameData.OnLoadDataComplete -= SetStoreName_Image;
    }
}
