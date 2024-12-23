using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescriptionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI ShootCooldownVal;
    [SerializeField] private TextMeshProUGUI DamageVal;
    [SerializeField] private TextMeshProUGUI LevelVal;
    [SerializeField] private TextMeshProUGUI PriceVal;
    [SerializeField] private TextMeshProUGUI HpVal;
    [SerializeField] private TextMeshProUGUI Description;

    public void UpdateInfoInSelection(int nameid, float cd, float dmg, float price, float hp, int descid)
    {
        Name.text = Language.list[nameid];
        ShootCooldownVal.text = cd.ToString();
        DamageVal.text = dmg.ToString();
        PriceVal.text = price.ToString();
        HpVal.text = hp.ToString();
        Description.text = Language.list[descid];
        PriceVal.transform.parent.gameObject.SetActive(true);
        LevelVal.transform.parent.gameObject.SetActive(false);
    }

    public void UpdateInfoInGame(int nameid, float cd, float dmg, float price, float hp, int descid)
    {
        Name.text = Language.list[nameid];
        ShootCooldownVal.text = cd.ToString();
        DamageVal.text = string.Format("{0:f2}", dmg);
        LevelVal.text = price.ToString();
        HpVal.text = Mathf.CeilToInt(hp).ToString();
        Description.text = Language.list[descid];
        PriceVal.transform.parent.gameObject.SetActive(false);
        LevelVal.transform.parent.gameObject.SetActive(true);
    }
}
