using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] Text _currentBalanceText = null;

    private void OnEnable()
    {
        GameManager.OnUpdateBalance += UpdateUI;
    }
    private void OnDisable()
    {
        GameManager.OnUpdateBalance -= UpdateUI;
    }
    private void Awake()
    {
        
    }

    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        _currentBalanceText.text = GameManager.instance.CurrentBalance.ToString("C2");
    }
}
