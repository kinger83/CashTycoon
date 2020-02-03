using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void UpdateBalance();
    public static event UpdateBalance OnUpdateBalance;
    public static GameManager instance;
    private float _currentBalance = 5.0f;


    public float CurrentBalance { get { return _currentBalance; } /*set { _currentBalance = value; }*/ }
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add_Subtract_Money(float amount)
    {
        _currentBalance +=amount;
        OnUpdateBalance();  // sending even message out to listeners
        //UIManager.instance.UpdateUI();


    }

    
}
