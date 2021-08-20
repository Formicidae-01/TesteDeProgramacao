using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GP_BotaoHud : MonoBehaviour
{
    //Essa classe serve para controlar o animador do botão que aparece na HUD
    //Os eventos do botão já são controlados pelo "EventTrigger"

    public Animator anim;

    public void Apertado(bool valor)
    {
        anim.SetBool("Apertado", valor);
    }
}
