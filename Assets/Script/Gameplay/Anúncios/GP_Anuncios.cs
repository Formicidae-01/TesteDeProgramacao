using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

//Classe que inicia e reproduz os anúncios
public class GP_Anuncios : MonoBehaviour
{
    //Inicia os anúncios a partir do ID Android
    void Start()
    {
        Advertisement.Initialize("4272167");
    }

    //Método que reproduz anúncios
    public void ReproduzirAnuncio()
    {
        if (Advertisement.IsReady("LevelEnd"))
        {
            Advertisement.Show("LevelEnd");
        }
    }
}
