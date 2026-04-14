using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [SerializeField] private SO_Merchant _merchant;
    [SerializeField] private UI_MerchantWindow _merchantWindow;

    private void Start()
    {
        _merchantWindow.Setup(_merchant);
    }
}
