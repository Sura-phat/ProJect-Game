using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private RectTransform shop;
    [SerializeField] private RectTransform content;

    [Header("Workers")]
    [SerializeField] private ShopWorkerUI ShopWorkerUI;

    public void AddWorker(Worker worker)
    {
        var newWorker = Instantiate(ShopWorkerUI, content);
        newWorker.UpdateUI(worker);
    }

    public void OpenShop()
    {
        shop.gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        shop.gameObject.SetActive(false);
    }
}
