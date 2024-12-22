using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Language
{
    public static Dictionary<int, string> list = new Dictionary<int, string>();
    static Language()
    {
        list[1001] = "普通防御塔";
        list[10011] = "这是一个普通防御塔。";
        list[1002] = "多发防御塔";
        list[10021] = "这是一个发射多发子弹的防御塔。";
    }
}
