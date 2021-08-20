using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe que possui as posições onde o personagem pode renascer depois de morrer, selecionando a posição mais próxima dele
public class GP_PontosDeRespawn : MonoBehaviour
{
    [Header("Pontos onde o personagem poderá renascer")]
    public Transform[] pontos;

    //Método que verifica qual ponto está mais próximo da "posição utilizada" (parâmetro do método) e retorna esse ponto
    public Vector2 PontoMaisProximo(Vector2 posicaoUtilizada)
    {
        //Define que a posição mais próxima é a posição do primeiro ponto e que tem a menor distância até a posição utilizada
        Vector2 posicaoProxima = pontos[0].position;
        float menorDistancia = Vector2.Distance(posicaoUtilizada, posicaoProxima);

        //Verifica a distância entre todos os pontos, se a distância de algum ponto for menor que a distância entre a posição utilizada e o primeiro ponto, o ponto mais próximo é alterado
        float tmpDistancia;

        for (int i = 0; i < pontos.Length; i++)
        {
            tmpDistancia = Vector2.Distance(posicaoUtilizada, pontos[i].position);
            if (tmpDistancia < menorDistancia)
            {
                posicaoProxima = pontos[i].position;
                menorDistancia = tmpDistancia;
            }
        }

        return posicaoProxima;
    }
}
