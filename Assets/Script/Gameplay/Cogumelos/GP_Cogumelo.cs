using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe que executa a animação de um cogumelo e guarda uma posição de orientação, que o personagem seguirá ao saltar com um cogumelo
public class GP_Cogumelo : MonoBehaviour
{
    [Header("Animador")]
    public Animator anim;
    [Header("Orientador de pulo do personagem")]
    public Transform orientadorPulo;

    public void Sacudir()
    {
        anim.SetTrigger("Sacudir"); 
    }
}
