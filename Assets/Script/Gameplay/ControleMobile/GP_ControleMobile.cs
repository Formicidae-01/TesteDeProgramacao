using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GP_ControleMobile : MonoBehaviour
{
    GP_Dinossauro dinossauro;

    void Start()
    {
        dinossauro = FindObjectOfType<GP_Dinossauro>();
    }

    public void Mover(float valor)
    {
        dinossauro.ComandoX(valor);
    }

    public void Pulo(bool valor)
    {
        dinossauro.ComandoPulo(valor);
    }

    public void Correr(bool valor)
    {
        dinossauro.ComandoCorrer(valor);
    }

}
