using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Language
{
    public static Dictionary<int, string> list = new Dictionary<int, string>();
    static Language()
    {
        list[1001] = "��ͨ������";
        list[10011] = "����һ����ͨ��������";
        list[1002] = "�෢������";
        list[10021] = "����һ������෢�ӵ��ķ�������";
    }
}
