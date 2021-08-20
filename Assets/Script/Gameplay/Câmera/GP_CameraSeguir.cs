using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GP_CameraSeguir : MonoBehaviour
{
    public Transform objetoAlvo;
    public float velocidade;

    public float minimoX, maximoX;
    public float minimoY, maximoY;

    private void FixedUpdate() 
    {
        Vector2 posicao = Vector2.Lerp(transform.position, objetoAlvo.position, velocidade);
        posicao = new Vector2(Mathf.Clamp(posicao.x, minimoX, maximoX), Mathf.Clamp(posicao.y, minimoY, maximoY));
        transform.position = posicao;    
    }
}
