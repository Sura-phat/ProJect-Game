using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;
using System;
using static BreakInfinity.BigDouble;

public class IdleTutorialGame : MonoBehaviour
{
    // Texts
    public Text coinsText;
    public Text coinsPerSecText;
    public Text clickValueText;
    
    //Main Currency Value
    public BigDouble coins;
    public BigDouble coinsPerSec;
    public BigDouble coinsClickValue;

    //Click Upgrade 1 
    public BigDouble clickUpgrade1Level;
    public Text clickUpgrade1Text;

    //Click Upgrade 1 Buy Max
    public Text clickUpgrade1MaxText;

    //Click Upgrade 2 Buy Max
    public Text clickUpgrade2MaxText;

    //Production Upgrade 1 Buy Max
    public Text productionUpgrade1MaxText;

    //Production Upgrade 2 Buy Max
    public Text productionUpgrade2MaxText;

    //Production Upgrade 1
    public BigDouble productionUpgrade1Level;
    public Text productionUpgrade1Text;
    
    //Production Upgrade 2
    public BigDouble productionUpgrade2Level;
    public BigDouble productionUpgrade2Power;
    public Text productionUpgrade2Text;

    //Click Upgrade 2
    public BigDouble clickUpgrade2Level;
    public Text clickUpgrade2Text;

    //Prestige Stuff
    public Text gemsText;
    public Text gemBoostText;
    public Text gemsToGetText;
    public BigDouble gems;
    public BigDouble gemBoost;
    public BigDouble gemsToGet;

    //E11
    public CanvasGroup mainMenuGroup;
    public CanvasGroup upgradeGroup;
    public int tabSwitcher;
   
    public Image clickUpgradeBar;

    public void Start()
    {
        Application.targetFrameRate = 60;
        
        CanvasGroupChanger(true, mainMenuGroup);
        CanvasGroupChanger(false, upgradeGroup);
        tabSwitcher = 0;

        Load();
    }

    public void CanvasGroupChanger(bool x, CanvasGroup y)
    {
        if (x)
        {
            y.alpha = 1;
            y.interactable = true;
            y.blocksRaycasts = true;
            return;
        }
        y.alpha = 0;
        y.interactable = false;
        y.blocksRaycasts = false;
    }

    public void Load()
    {
        coins = Parse(PlayerPrefs.GetString("coins","0"));
        coinsClickValue = Parse(PlayerPrefs.GetString("coinsClickValue","1"));
        productionUpgrade2Power = Parse(PlayerPrefs.GetString("productionUpgrade2Power","5"));

        gems = Parse(PlayerPrefs.GetString("gems","0"));

        productionUpgrade1Level = Parse(PlayerPrefs.GetString("productionUpgrade1Level", "0"));
        productionUpgrade2Level = Parse(PlayerPrefs.GetString("productionUpgrade2Level", "0"));
        clickUpgrade1Level = Parse(PlayerPrefs.GetString("clickUpgrade1Level",  "0"));
        clickUpgrade2Level = Parse(PlayerPrefs.GetString("clickUpgrade2Level",  "0"));
    }

    public void Save()
    {
        PlayerPrefs.SetString("coins", coins.ToString());
        PlayerPrefs.SetString("coinsClickValue", coinsClickValue.ToString());
        PlayerPrefs.SetString("productionUpgrade2Power", productionUpgrade2Power.ToString());

        PlayerPrefs.SetString("gems", gems.ToString());

        PlayerPrefs.SetString("productionUpgrade1Level", productionUpgrade1Level.ToString());
        PlayerPrefs.SetString("productionUpgrade2Level", productionUpgrade2Level.ToString());
        PlayerPrefs.SetString("clickUpgrade1Level", clickUpgrade1Level.ToString());
        PlayerPrefs.SetString("clickUpgrade2Level", clickUpgrade2Level.ToString());
    }

    public void Update()
    {
        //The Higer the > 1e15 is the longer it takes to earn peels
        gemsToGet = 150 * Sqrt(coins / 1e7);
        gemBoost = gems * 0.05 + 1;

        gemsToGetText.text = "Prestige:\n+" + Floor(gemsToGet).ToString("F0") + " Gems";
        gemsText.text = "Gems :" + Floor(gems).ToString("F0");
        gemBoostText.text = gemBoost.ToString("F2") + "x boost";
        
        coinsPerSec = (productionUpgrade1Level + (productionUpgrade2Power * productionUpgrade2Level)) * gemBoost;
        
        clickValueText.text = "Click\n+" + NotationMethod(coinsClickValue, "F0") + " Money";
        coinsText.text = "$" + NotationMethod(coins, "F0");
        coinsPerSecText.text = "$" + NotationMethod(coinsPerSec, "F0") + "/s";

        //Click Upgrade Exponant Starts

        string clickUpgrade1CostString;
        var clickUpgrade1Cost = 10 * Pow(1.07, clickUpgrade1Level);
        clickUpgrade1CostString = NotationMethod(clickUpgrade1Cost, "F0");
        
        string clickUpgrade1LevelString;
        clickUpgrade1LevelString = NotationMethod(clickUpgrade1Level, "F0");

        string clickUpgrade2CostString;
        var clickUpgrade2Cost = 25 * Pow(1.07, clickUpgrade2Level);
        clickUpgrade2CostString = NotationMethod(clickUpgrade2Cost, "F0");
        
        string clickUpgrade2LevelString;
        clickUpgrade2LevelString = NotationMethod(clickUpgrade2Level, "F0");
        
        clickUpgrade1Text.text = "Click UPG 1\nCost: " + clickUpgrade1CostString + " coins\nPower: +1 Click\nLevel: " + clickUpgrade1LevelString;    
        clickUpgrade2Text.text = "Click UPG 2\nCost: " + clickUpgrade2CostString + " coins\nPower: +5 Click\nLevel: " + clickUpgrade2LevelString;

        clickUpgrade1MaxText.text = "Buy Max (" + BuyClickUpgrade1MaxCount() + ")";
        clickUpgrade2MaxText.text = "Buy Max (" + BuyClickUpgrade2MaxCount() + ")";

        //Click Upgrade Exponant Ends

        
        //Production Upgrade Exponant Starts

        string productionUpgrade1CostString;
        var productionUpgrade1Cost = 25 * Pow(1.07, productionUpgrade1Level);
        if (productionUpgrade1Cost > 1000)
        {
            var exponent = (Floor(Log10(Abs(productionUpgrade1Cost))));
            var mantissa = (productionUpgrade1Cost / Pow(10, exponent));
            productionUpgrade1CostString = mantissa.ToString("F2") + "e" + exponent;
        }
        else
        {
            productionUpgrade1CostString = productionUpgrade1Cost.ToString("F0");
        }

        string productionUpgrade1LevelString;
        if (productionUpgrade1Level > 1000)
        {
            var exponent = (Floor(Log10(Abs(productionUpgrade1Level))));
            var mantissa = (productionUpgrade1Level / Pow(10, exponent));
            productionUpgrade1LevelString = mantissa.ToString("F2") + "e" + exponent;
        }
        else
        {
            productionUpgrade1LevelString = productionUpgrade1Level.ToString("F0");
        }

        string productionUpgrade2CostString;
        var productionUpgrade2Cost = 250 * Pow(1.07, productionUpgrade2Level);
        if (productionUpgrade2Cost > 1000)
        {
            var exponent = (Floor(Log10(Abs(productionUpgrade2Cost))));
            var mantissa = (productionUpgrade2Cost / Pow(10, exponent));
            productionUpgrade2CostString = mantissa.ToString("F2") + "e" + exponent;
        }
        else
        {
            productionUpgrade2CostString = productionUpgrade2Cost.ToString("F0");
        }

        string productionUpgrade2LevelString;
        if (productionUpgrade2Level > 1000)
        {
            var exponent = (Floor(Log10(Abs(productionUpgrade2Level))));
            var mantissa = (productionUpgrade2Level / Pow(10, exponent));
            productionUpgrade2LevelString = mantissa.ToString("F2") + "e" + exponent;
        }
        else
        {
            productionUpgrade2LevelString = productionUpgrade2Level.ToString("F0");
        }
       
        productionUpgrade1Text.text = "Production UPG 1\nCost: " + productionUpgrade1CostString + " Money\nPower: +" + gemBoost.ToString("F2") + "\nLevel: " + productionUpgrade1Level;       
        productionUpgrade2Text.text = "Production UPG 2\nCost: " + productionUpgrade2CostString + " Money\nPower: +" + (productionUpgrade2Power * gemBoost).ToString("F2") +  "\nLevel: " + productionUpgrade2Level;

        productionUpgrade1MaxText.text = "Buy Max (" + BuyProductionUpgrade1MaxCount() + ")";
        productionUpgrade2MaxText.text = "Buy Max (" + BuyProductionUpgrade2MaxCount() + ")";

        //Product Upgrade Exponants End
        
        coins += coinsPerSec * Time.deltaTime;

        Save();
    }

    public string NotationMethod(BigDouble x, string y)
    {
        if (x > 1000)
        {
            var exponent = Floor(Log10(Abs(x)));
            var mantissa = x / Pow(10, exponent);
            return mantissa.ToString("F2") + "e" + exponent;
        }
        return x.ToString(y);
    }

    
    
    // Prestige
    public void Prestige()
    {
        if (coins > 1000)
        {
            coins = 0;
            coinsClickValue = 1;
            productionUpgrade2Power = 5;
            productionUpgrade1Level = 0;
            productionUpgrade2Level = 0;
            clickUpgrade1Level = 0;
            clickUpgrade2Level = 0;
            gems += gemsToGet;
        }
    }

    // Button
    public void Click()
    {
        coins += coinsClickValue;
    }

    public void BuyUpgrade(string upgradeID)
    {
        
        //p means production
        var pcost = 25 * Pow(1.07, productionUpgrade1Level);
        var pcost2 = 250 * Pow(1.07, productionUpgrade2Level);
            
        //normal cost is the click upgrade costs
        var cost1 = 10 * Pow(1.07, clickUpgrade1Level);
        var cost2 = 25 * Pow(1.07, clickUpgrade2Level);
        
        switch (upgradeID)
        {
            case "C1":
                if (coins >= cost1)
                {
                    clickUpgrade1Level++;
                    coins -= cost1;
                    coinsClickValue++;          
                }
                break;
            
            case "C2":
                if (coins >= cost2)
                {
                    clickUpgrade2Level++;
                    coins -= cost2;
                    coinsClickValue += 5;
                }
                break;
                         
            case "P1":
                if (coins >= pcost)
                {
                    productionUpgrade1Level++;
                    coins -= pcost;
                }
                break;
            
            case "P2":
                if (coins >= pcost2)
                {
                    productionUpgrade2Level++;
                    coins -= pcost2;
                } 
                break;
            
            default:
                Debug.Log("I'm not assigned to a proper upgrade!");
                break;
        }
    }

    //Buy Upgrade 1 Buy Max Method Below

    public void BuyclickUpgrade1Max()
    {
        var b = 10;
        var c = coins;
        var r = 1.07;
        var k = clickUpgrade1Level;
        var n = Floor(Log((c * (r - 1)) / (b * Pow(r,k)) + 1, r));

        var cost = b * (Pow(r, k) + (Pow(r, n) - 1) / (r - 1));

        if(coins >= cost)
        {
            clickUpgrade1Level += n;
            coins -= cost;
            coinsClickValue += n;
        }                
    }

    public BigDouble BuyClickUpgrade1MaxCount()
    {
        var b = 10;
        var c = coins;
        var r = 1.07;
        var k = clickUpgrade1Level;
        var n = Floor(Log((c * (r - 1)) / (b * Pow(r,k)) + 1, r));
        return n;
    }

    //Buy Upgrade 2 Buy Max Method Below
    public void BuyclickUpgrade2Max()
    {
        var b = 25;
        var c = coins;
        var r = 1.07;
        var k = clickUpgrade2Level;
        var n = Floor(Log((c * (r - 1)) / (b * Pow(r,k)) + 1, r));

        var cost = b * (Pow(r, k) + (Pow(r, n) - 1) / (r - 1));

        if(coins >= cost)
        {
            clickUpgrade2Level += n;
            coins -= cost;
            coinsClickValue += n;
        }                
    }

    public BigDouble BuyClickUpgrade2MaxCount()
    {
        var b = 25;
        var c = coins;
        var r = 1.07;
        var k = clickUpgrade2Level;
        var n = Floor(Log((c * (r - 1)) / (b * Pow(r,k)) + 1, r));
        return n;
    }

    // Buy Production Upgrade 1 Buy Max Method Below
    
    public void BuyProductionUpgrade1Max()
    {
        var b = 25;
        var c = coins;
        var r = 1.07;
        var k = productionUpgrade1Level;
        var n = Floor(Log((c * (r - 1)) / (b * Pow(r,k)) + 1, r));

        var cost = b * (Pow(r, k) + (Pow(r, n) - 1) / (r - 1));

        if(coins >= cost)
        {
            productionUpgrade1Level += n;
            coins -= cost;
            coinsPerSec += n;
        } 
    }

    public BigDouble BuyProductionUpgrade1MaxCount()
    {
        var b = 25;
        var c = coins;
        var r = 1.07;
        var k = productionUpgrade1Level;
        var n = Floor(Log((c * (r - 1)) / (b * Pow(r,k)) + 1, r));
        return n;
    }

    // Buy Production Upgrade 2 Buy Max Method Below

    public void BuyProductionUpgrade2Max()
    {
        var b = 250;
        var c = coins;
        var r = 1.07;
        var k = productionUpgrade2Level;
        var n = Floor(Log((c * (r - 1)) / (b * Pow(r,k)) + 1, r));

        var cost = b * (Pow(r, k) + (Pow(r, n) - 1) / (r - 1));

        if(coins >= cost)
        {
            productionUpgrade2Level += n;
            coins -= cost;
            coinsPerSec += n;
        } 
    }

    public BigDouble BuyProductionUpgrade2MaxCount()
    {
        var b = 250;
        var c = coins;
        var r = 1.07;
        var k = productionUpgrade2Level;
        var n = Floor(Log((c * (r - 1)) / (b * Pow(r,k)) + 1, r));
        return n;
    }

    public void ChangeTabs(string id)
    {
        switch (id)
        {
            case "upgrades":
                CanvasGroupChanger(false, mainMenuGroup);
                CanvasGroupChanger(true, upgradeGroup);
                break;
            case "main":
                CanvasGroupChanger(true, mainMenuGroup);
                CanvasGroupChanger(false, upgradeGroup);
                break;
        }
    }

    public void FullReset()
    {
        coins = 0;
        coinsClickValue = 1;
        productionUpgrade2Power =5;

        gems = 0;

        productionUpgrade1Level = 0;
        productionUpgrade2Level = 0;
        clickUpgrade1Level = 0;
        clickUpgrade2Level = 0;
    }
}