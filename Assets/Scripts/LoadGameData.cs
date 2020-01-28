using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameData : MonoBehaviour
{
    public delegate void LoadDataComplete();
    public static event LoadDataComplete OnLoadDataComplete; 

    private GameObject _storePrefab;
    private GameObject _managerPrefab;

    [SerializeField] GameObject _storePanel = null;
    [SerializeField] GameObject _managerPanel = null;


    private void Start()
    {
        _storePrefab = Resources.Load("Prefabs/StorePrefab") as GameObject;
        _managerPrefab = Resources.Load("Prefabs/ManagerPrefab") as GameObject;
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
                    Text storeText = thisstore.transform.Find("Store Name").GetComponent<Text>();
                    storeText.text = setName;
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
                    LoadManagerInfo(storeNode, newStore );
                }

            }
        }
        OnLoadDataComplete();
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
    private void LoadManagerInfo(XmlNode storeNode, GameObject newStore)
    {

        GameObject newManager = (GameObject)Instantiate(_managerPrefab);
        newManager.transform.SetParent(_managerPanel.transform);
        StoreManager manager = newManager.GetComponent<StoreManager>();

        XmlNodeList managerNodes = storeNode.ChildNodes;


        foreach (XmlNode managerNode in managerNodes)
        {
            manager._managedStore = newStore;

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







