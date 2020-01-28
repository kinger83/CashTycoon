using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerUiManager : MonoBehaviour
{
    private Text _mangerNameText = null;
    private Text _managerHireText = null;
    private Text _mangerCutText = null;
    private Text _hiredText = null;
    private StoreManager _managerScript = null;
    private Button _hireButton = null;


    // Start is called before the first frame update
    private void OnEnable()
    {
        LoadGameData.OnLoadDataComplete += SetManagerDetails;
    }

    private void Awake()
    {

        _mangerNameText = transform.Find("ManagerNameText").GetComponent<Text>();
        _managerHireText = transform.Find("ManagerHireText").GetComponent<Text>();
        _mangerCutText = transform.Find("ManagerPercentageText").GetComponent<Text>();
        _hiredText = transform.Find("HiredText").GetComponent<Text>();
        _managerScript = transform.GetComponent<StoreManager>();
        _hireButton = transform.Find("ManagerHireButton").GetComponent<Button>();
    }


    private void SetManagerDetails()
    {
        _hiredText.gameObject.SetActive(false);
        _mangerNameText.text = _managerScript.ManagerName;
        _managerHireText.text = _managerScript.ManagerHireCost.ToString("C2");
        _mangerCutText.text = _managerScript.ManagerCut.ToString("C2");
    }

    public void SetHireButton(bool interactable)
    {
        _hireButton.interactable = interactable;
    }

    public void RemoveHireButtonandPrice()
    {
        _hireButton.gameObject.SetActive(false);
        _managerHireText.gameObject.SetActive(false);
        _hiredText.gameObject.SetActive(true);
    }



    private void OnDisable()
    {
        LoadGameData.OnLoadDataComplete -= SetManagerDetails;
    }
}
