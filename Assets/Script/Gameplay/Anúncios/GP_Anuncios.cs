using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class GP_Anuncios : MonoBehaviour
{
    void Start()
    {
        Advertisement.Initialize("4272167");
    }

    public void ReproduzirAnuncio()
    {
        if (Advertisement.IsReady("LevelEnd"))
        {
            Advertisement.Show("LevelEnd");
        }
    }
}
