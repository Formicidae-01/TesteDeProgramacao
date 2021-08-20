using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe que cuida do espinho que bloqueia a saída, animando-o para abrir o caminho
public class GP_EspinhoSaida : MonoBehaviour
{
    public Animator anim;

    public void Abrir()
    {
        anim.enabled = true;
    }
}
