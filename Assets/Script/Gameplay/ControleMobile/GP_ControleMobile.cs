using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe que passa comandos ao script de movimento do personagem.
public class GP_ControleMobile : MonoBehaviour
{
    // Cria uma referência ao script de personagem e o obtém ao iniciar
    GP_Dinossauro dinossauro;

    void Start()
    {
        dinossauro = FindObjectOfType<GP_Dinossauro>();
    }

    //Métodos que passam comandos de Movimento, Pulo e Corrida (Esses métodos são chamados nos "EventTrigger" dos botões presentes na HUD)
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
