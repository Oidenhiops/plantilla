using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationBetweenScenes : MonoBehaviour
{
    public static InformationBetweenScenes informationBetweenScenes;
    private void Awake()
    {
        if (InformationBetweenScenes.informationBetweenScenes == null)
        {
            InformationBetweenScenes.informationBetweenScenes = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
