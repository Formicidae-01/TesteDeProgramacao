using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe que faz com que a câmera persiga um determinado objeto em uma certa velocidade, dentro de uma posição limite
//(Essa classe não se limita somente a cameras, mas é onde foi aplicada)
public class GP_CameraSeguir : MonoBehaviour
{
    [Header("Alvo a ser seguido")]
    public Transform objetoAlvo;
    [Header("Velocidade de perseguição")]
    public float velocidade;

    [Header("Posições mínimas e máximas para onde este objeto pode ir")]
    [Header("Eixo X")]
    public float minimoX, maximoX;
    [Header("Eixo Y")]
    public float minimoY, maximoY;

    private void FixedUpdate() 
    {
        //Gera a posição onde o objeto deve estar, depois aplica os limites de posição
        Vector2 posicao = Vector2.Lerp(transform.position, objetoAlvo.position, velocidade);
        posicao = new Vector2(Mathf.Clamp(posicao.x, minimoX, maximoX), Mathf.Clamp(posicao.y, minimoY, maximoY));
        //Determina a posição desse objeto como a posição gerada acima
        transform.position = posicao;    
    }
}
