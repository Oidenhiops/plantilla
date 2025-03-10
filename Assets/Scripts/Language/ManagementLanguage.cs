using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class ManagementLanguage : MonoBehaviour
{
    private TMP_Text dialogText;
    private ManagementData managementData;
    public int id = 0;
    public ManagementData.TypeLanguage currentLanguage = ManagementData.TypeLanguage.English;

    void Start()
    {
        managementData = GameObject.FindWithTag("InformationBetweenScenes").GetComponent<ManagementData>();
        dialogText = GetComponent<TMP_Text>();
        ValidateChangeText();
    }
    public void ValidateChangeText()
    {
        if (dialogText == null) dialogText = GetComponent<TMP_Text>();
        if (ManagementData.csvData.Count != 0)
        {
            currentLanguage = ManagementData.saveData.configurationsInfo.currentLanguage;
            dialogText.text = GetDialog(id, ManagementData.saveData.configurationsInfo.currentLanguage.ToString());
        }
    }
    public string GetDialog(int id, string language)
    {
        int languageIndex = 0;
        for (int i = 0; i < ManagementData.csvData[0].Count(); i++)
        {
            if (ManagementData.csvData[0][i] == language)
            {
                languageIndex = i;
                break;
            }
        }
        if (languageIndex != 0) return ManagementData.csvData[id][languageIndex];
        return null;
    }
}