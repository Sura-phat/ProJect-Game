using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;
using static BreakInfinity.BigDouble;

public class IdleTutorialGame : MonoBehaviour
{
    // Texts
    public Text coinsText;
    public Text coinsPerSecText;
    public Text clickValueText;
    
    //Main Currency Value
    public double coins;
    public double coinsPerSec;
    public double coinsClickValue;

    //Click Upgrade 1 
    public int clickUpgrade1Level;
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
    public int productionUpgrade1Level;
    public Text productionUpgrade1Text;
    
    //Production Upgrade 2
    public int productionUpgrade2Level;
    public double productionUpgrade2Power;
    public Text productionUpgrade2Text;

    //Click Upgrade 2
    public int clickUpgrade2Level;
    public Text clickUpgrade2Text;

    //Prestige Stuff
    public Text gemsText;
    public Text gemBoostText;
    public Text gemsToGetText;
    public double gems;
    public double gemBoost;
    public double gemsToGet;
   
    public Image clickUpgradeBar;

    public void Start()
    {
        Application.targetFrameRate = 60;
        Load();
    }

    public void Load()
    {
        coins = double.Parse(PlayerPrefs.GetString("coins","0"));
        coinsClickValue = double.Parse(PlayerPrefs.GetString("coinsClickValue","1"));
        productionUpgrade2Power = double.Parse(PlayerPrefs.GetString("productionUpgrade2Power","5"));

        gems = double.Parse(PlayerPrefs.GetString("gems","0"));

        productionUpgrade1Level = PlayerPrefs.GetInt("productionUpgrade1Level", 0);
        productionUpgrade2Level = PlayerPrefs.GetInt("productionUpgrade2Level", 0);
        clickUpgrade1Level = PlayerPrefs.GetInt("clickUpgrade1Level", 0);
        clickUpgrade2Level = PlayerPrefs.GetInt("clickUpgrade2Level", 0);
    }

    public void Save()
    {
        PlayerPrefs.SetString("coins", coins.ToString());
        PlayerPrefs.SetString("coinsClickValue", coinsClickValue.ToString());
        PlayerPrefs.SetString("productionUpgrade2Power", productionUpgrade2Power.ToString());

        PlayerPrefs.SetString("gems", gems.ToString());

        PlayerPrefs.SetInt("productionUpgrade1Level", productionUpgrade1Level);
        PlayerPrefs.SetInt("productionUpgrade2Level", productionUpgrade2Level);
        PlayerPrefs.SetInt("clickUpgrade1Level", clickUpgrade1Level);
        PlayerPrefs.SetInt("clickUpgrade2Level", clickUpgrade2Level);
    }

    public void Update()
    {
        //The Higer the > 1e15 is the longer it takes to earn peels
        gemsToGet = 150 * System.Math.Sqrt(coins / 1e7);
        gemBoost = gems * 0.05 + 1;

        gemsToGetText.text = "Prestige:\n+" + System.Math.Floor(gemsToGet).ToString("F0") + " Gems";
        gemsText.text = "Gems :" + System.Math.Floor(gems).ToString("F0");
        gemBoostText.text = gemBoost.ToString("F2") + "x boost";
        
        coinsPerSec = (productionUpgrade1Level + (productionUpgrade2Power * productionUpgrade2Level)) * gemBoost;
        
        clickValueText.text = "Click\n+" + NotationMethod(coinsClickValue, "F0") + " Money";
        coinsText.text = "$" + NotationMethod(coins, "F0");
        coinsPerSecText.text = "$" + NotationMethod(coinsPerSec, "F0") + "/s";

        //Click Upgrade Exponant Starts

        string clickUpgrade1CostString;
        var clickUpgrade1Cost = 10 * System.Math.Pow(1.07, clickUpgrade1Level);
        clickUpgrade1CostString = NotationMethod(clickUpgrade1Cost, "F0");
        
        string clickUpgrade1LevelString;
        clickUpgrade1LevelString = NotationMethod(clickUpgrade1Level, "F0");

        string clickUpgrade2CostString;
        var clickUpgrade2Cost = 25 * System.Math.Pow(1.07, clickUpgrade2Level);
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
        var productionUpgrade1Cost = 25 * System.Math.Pow(1.07, productionUpgrade1Level);
        if (productionUpgrade1Cost > 1000)
        {
            var exponent = (System.Math.Floor(System.Math.Log10(System.Math.Abs(productionUpgrade1Cost))));
            var mantissa = (productionUpgrade1Cost / System.Math.Pow(10, exponent));
            productionUpgrade1CostString = mantissa.ToString("F2") + "e" + exponent;
        }
        else
        {
            productionUpgrade1CostString = productionUpgrade1Cost.ToString("F0");
        }

        string productionUpgrade1LevelString;
        if (productionUpgrade1Level > 1000)
        {
            var exponent = (System.Math.Floor(System.Math.Log10(System.Math.Abs(productionUpgrade1Level))));
            var mantissa = (productionUpgrade1Level / System.Math.Pow(10, exponent));
            productionUpgrade1LevelString = mantissa.ToString("F2") + "e" + exponent;
        }
        else
        {
            productionUpgrade1LevelString = productionUpgrade1Level.ToString("F0");
        }

        string productionUpgrade2CostString;
        var productionUpgrade2Cost = 250 * System.Math.Pow(1.07, productionUpgrade2Level);
        if (productionUpgrade2Cost > 1000)
        {
            var exponent = (System.Math.Floor(System.Math.Log10(System.Math.Abs(productionUpgrade2Cost))));
            var mantissa = (productionUpgrade2Cost / System.Math.Pow(10, exponent));
            productionUpgrade2CostString = mantissa.ToString("F2") + "e" + exponent;
        }
        else
        {
            productionUpgrade2CostString = productionUpgrade2Cost.ToString("F0");
        }

        string productionUpgrade2LevelString;
        if (productionUpgrade2Level > 1000)
        {
            var exponent = (System.Math.Floor(System.Math.Log10(System.Math.Abs(productionUpgrade2Level))));
            var mantissa = (productionUpgrade2Level / System.Math.Pow(10, exponent));
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

    public string NotationMethod(double x, string y)
    {
        if (x > 1000)
        {
            var exponent = System.Math.Floor(System.Math.Log10(System.Math.Abs(x)));
            var mantissa = x / System.Math.Pow(10, exponent);
            return mantissa.ToString("F2") + "e" + exponent;
        }
        return x.ToString(y);
    }

    public string NotationMethod(float x, string y)
    {
        if (x > 1000)
        {
            var exponent = Mathf.Floor(Mathf.Log10(Mathf.Abs(x)));
            var mantissa = x / Mathf.Pow(10, exponent);
            return mantissa.ToString("F2") + "e" + exponent;
        }
        return x.ToString(y);
    }

    public string NotationMethod(int x, string y)
    {
        if (x > 1000)
        {
            var exponent = System.Math.Floor(System.Math.Log10(System.Math.Abs(x)));
            var mantissa = x / System.Math.Pow(10, exponent);
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
        var pcost = 25 * System.Math.Pow(1.07, productionUpgrade1Level);
        var pcost2 = 250 * System.Math.Pow(1.07, productionUpgrade2Level);
            
        //normal cost is the click upgrade costs
        var cost1 = 10 * System.Math.Pow(1.07, clickUpgrade1Level);
        var cost2 = 25 * System.Math.Pow(1.07, clickUpgrade2Level);
        
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
        var n = System.Math.Floor(System.Math.Log((c * (r - 1)) / (b * System.Math.Pow(r,k)) + 1, r));

        var cost = b * (System.Math.Pow(r, k) + (System.Math.Pow(r, n) - 1) / (r - 1));

        if(coins >= cost)
        {
            clickUpgrade1Level += (int)n;
            coins -= cost;
            coinsClickValue += n;
        }                
    }

    double BuyClickUpgrade1MaxCount()
    {
        var b = 10;
        var c = coins;
        var r = 1.07;
        var k = clickUpgrade1Level;
        var n = System.Math.Floor(System.Math.Log((c * (r - 1)) / (b * System.Math.Pow(r,k)) + 1, r));
        return n;
    }

    //Buy Upgrade 2 Buy Max Method Below
    public void BuyclickUpgrade2Max()
    {
        var b = 25;
        var c = coins;
        var r = 1.07;
        var k = clickUpgrade2Level;
        var n = System.Math.Floor(System.Math.Log((c * (r - 1)) / (b * System.Math.Pow(r,k)) + 1, r));

        var cost = b * (System.Math.Pow(r, k) + (System.Math.Pow(r, n) - 1) / (r - 1));

        if(coins >= cost)
        {
            clickUpgrade2Level += (int)n;
            coins -= cost;
            coinsClickValue += n;
        }                
    }

    double BuyClickUpgrade2MaxCount()
    {
        var b = 25;
        var c = coins;
        var r = 1.07;
        var k = clickUpgrade2Level;
        var n = System.Math.Floor(System.Math.Log((c * (r - 1)) / (b * System.Math.Pow(r,k)) + 1, r));
        return n;
    }

    // Buy Production Upgrade 1 Buy Max Method Below
    
    public void BuyProductionUpgrade1Max()
    {
        var b = 25;
        var c = coins;
        var r = 1.07;
        var k = productionUpgrade1Level;
        var n = System.Math.Floor(System.Math.Log((c * (r - 1)) / (b * System.Math.Pow(r,k)) + 1, r));

        var cost = b * (System.Math.Pow(r, k) + (System.Math.Pow(r, n) - 1) / (r - 1));

        if(coins >= cost)
        {
            productionUpgrade1Level += (int)n;
            coins -= cost;
            coinsPerSec += n;
        } 
    }

    double BuyProductionUpgrade1MaxCount()
    {
        var b = 25;
        var c = coins;
        var r = 1.07;
        var k = productionUpgrade1Level;
        var n = System.Math.Floor(System.Math.Log((c * (r - 1)) / (b * System.Math.Pow(r,k)) + 1, r));
        return n;
    }

    // Buy Production Upgrade 2 Buy Max Method Below

    public void BuyProductionUpgrade2Max()
    {
        var b = 250;
        var c = coins;
        var r = 1.07;
        var k = productionUpgrade2Level;
        var n = System.Math.Floor(System.Math.Log((c * (r - 1)) / (b * System.Math.Pow(r,k)) + 1, r));

        var cost = b * (System.Math.Pow(r, k) + (System.Math.Pow(r, n) - 1) / (r - 1));

        if(coins >= cost)
        {
            productionUpgrade2Level += (int)n;
            coins -= cost;
            coinsPerSec += n;
        } 
    }

    double BuyProductionUpgrade2MaxCount()
    {
        var b = 250;
        var c = coins;
        var r = 1.07;
        var k = productionUpgrade2Level;
        var n = System.Math.Floor(System.Math.Log((c * (r - 1)) / (b * System.Math.Pow(r,k)) + 1, r));
        return n;
    }
}