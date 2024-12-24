using UnityEngine;
using TMPro;

public class TankMonster : NormalEnemy
{
    public int shieldNum;
    [SerializeField] GameObject Shield;
    [SerializeField] TextMeshProUGUI shieldNumText;

    public override bool DoDamage(float dmg)
    {
        if (shieldNum > 0)
        {
            shieldNum--;
            UpdateVisual();
            return false;
        }
        return base.DoDamage(dmg);
    }

    public override void UpdateVisual()
    {
        base.UpdateVisual();
        shieldNumText.text = shieldNum.ToString();
        if (shieldNum <= 0)
        {
            Shield.SetActive(false);
        }
    }

    public override void LookAt(Vector3 pos)
    {
        base.LookAt(pos);
        shieldNumText.GetComponent<RectTransform>().rotation = Quaternion.identity;
    }
}