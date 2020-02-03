using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameData : MonoBehaviour
{
    public delegate void LoadDataComplete();
    public static event LoadDataComplete OnLoadDataComplete; 

    private GameObject _storePrefab = null;
    private GameObject _managerPrefab = null;
    private GameObject _upgradePrefab = null;
    private string _storename = null;

    [SerializeField] GameObject _storePanel = null;
    [SerializeField] GameObject _managerPanel = null;
    [SerializeField] GameObject _upgradesPanel = null;


    private void Start()
    {
        _storePrefab = Resources.Load("Prefabs/StorePrefab") as GameObject;
        _managerPrefab = Resources.Load("Prefabs/ManagerPrefab") as GameObject;
        _upgradePrefab = Resources.Load("Prefabs/UpgradePrefab") as GameObject;
        Invoke("LoadData", 0.1f);
        
    }

    public void LoadData()
    {
        TextAsset _gameData = new TextAsset();
        _gameData = (TextAsset)Resources.Load("Data/GameData");

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(_gameData.text);
        XmlNodeList startingBalanceNode = xmlDoc.GetElementsByTagName("StartingBalance");
        XmlNodeList companyNameNode = xmlDoc.GetElementsByTagName("CompanyName");
        XmlNodeList storeList = xmlDoc.GetElementsByTagName("Store");

        LoadGameManagerVariables(startingBalanceNode, companyNameNode);
        LoadStores(storeList);
    }

    private void LoadStores(XmlNodeList storeList)
    {
        foreach (XmlNode store in storeList)
        {

            GameObject newStore = (GameObject)Instantiate(_storePrefab);
            newStore.transform.SetParent(_storePanel.transform);
            Store thisstore = newStore.GetComponent<Store>();

            XmlNodeList storeNodes = store.ChildNodes;
            foreach (XmlNode storeNode in storeNodes)
            {
                if (storeNode.Name == "Name")
                {
                    string setName = storeNode.InnerText;
                    _storename = setName;
                    thisstore.StoreName = setName;
                }

                if (storeNode.Name == "Image")
                {
                    Sprite mySprite = Resources.Load<Sprite>("Icons/" + storeNode.InnerText);
                    Image storeImage = thisstore.transform.Find("ClickImage").GetComponent<Image>();
                    storeImage.sprite = mySprite;
                }

                if (storeNode.Name == "BaseStoreCost")
                {
                    thisstore.BaseStoreCost = float.Parse(storeNode.InnerText);
                }

                if (storeNode.Name == "BaseStoreUpgradeCost")
                {
                    thisstore.BaseStoreUpgradeCost = float.Parse(storeNode.InnerText);
                }

                if (storeNode.Name == "BaseStoreProfit")
                {
                    thisstore.BaseStoreProfit = float.Parse(storeNode.InnerText);
                }

                if (storeNode.Name == "BaseStoreTimer")
                {
                    thisstore.BaseStoreTimer = float.Parse(storeNode.InnerText);
                }

                if (storeNode.Name == "BaseCostMultiplier")
                {
                    thisstore.BaseStoreCostMultiplier = float.Parse(storeNode.InnerText);
                }

                if (storeNode.Name == "BaseProfitMultiplier")
                {
                    thisstore.BaseStoreProfitMultiplier = float.Parse(storeNode.InnerText);
                }

                if (storeNode.Name == "ManagerDetails")
                {
                    LoadManagerInfo(storeNode, newStore, _storename);
                }
                if(storeNode.Name == "UpgradesList")
                {
                    LoadUpgrades(storeNode, newStore, _storename);
                }

            }
        }
        OnLoadDataComplete();
    }

    private void LoadUpgrades(XmlNode storeNode, GameObject newStore, string storename)
    {
        

        XmlNodeList upgradeList = storeNode.ChildNodes;
        foreach (XmlNode upgradeItemNode in upgradeList)
        {
            GameObject newUpgrade = Instantiate(_upgradePrefab);
            newUpgrade.transform.SetParent(_upgradesPanel.transform);
            UpgradesManager upgradesManager = newUpgrade.GetComponent<UpgradesManager>();

            upgradesManager.UpgradeforStoreNameGameObject = newStore;
            upgradesManager.UpgradeforStoreName = storename;
            XmlNodeList upgradeDetails = upgradeItemNode.ChildNodes;
            foreach (XmlNode upgradeDetail in upgradeDetails)
            {
                if(upgradeDetail.Name == "UpgradeName")
                {
                    upgradesManager.UpgradeName = upgradeDetail.InnerText;
                }
                
                if (upgradeDetail.Name == "UpgradeCost")
                {
                    upgradesManager.UpgradeCost = float.Parse(upgradeDetail.InnerText);
                }
                
                if (upgradeDetail.Name == "UpgradeDiscription")
                {
                    upgradesManager.UpgradeDescription = upgradeDetail.InnerText;
                }
                
                if (upgradeDetail.Name == "UpgradeMultiplier")
                {
                    upgradesManager.UpgradeMulitplier = float.Parse(upgradeDetail.InnerText);
                }
            }
        }

    }

    private static void LoadGameManagerVariables(XmlNodeList startingBalanceNode, XmlNodeList companyNameNode)
    {
        if (companyNameNode != null)
        {
            string companyName = companyNameNode[0].InnerText;
            GameManager.instance.CompanyName = companyName;
        }

        if (startingBalanceNode != null)
        {
            GameManager.instance.Add_Subtract_Money(float.Parse(startingBalanceNode[0].InnerText));
        }
        OnLoadDataComplete();
    }
    private void LoadManagerInfo(XmlNode storeNode, GameObject newStore, string storename)
    {

        GameObject newManager = Instantiate(_managerPrefab);
        newManager.transform.SetParent(_managerPanel.transform);
        StoreManager manager = newManager.GetComponent<StoreManager>();

        XmlNodeList managerNodes = storeNode.ChildNodes;


        foreach (XmlNode managerNode in managerNodes)
        {
            manager._managedStore = newStore;
            manager.StoreBeingManaged = storename;


            if (managerNode.Name == "ManagerName")
            {
                manager.ManagerName = managerNode.InnerText;
                
            }

            else if (managerNode.Name == "ManagerHirePrice")
            {


                manager.ManagerHireCost = float.Parse(managerNode.InnerText);
                
            }

            else if (managerNode.Name == "ManagerCut")
            {

                manager.ManagerCut = float.Parse(managerNode.InnerText);
                
            }



        }
    }
}







