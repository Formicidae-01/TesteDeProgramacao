using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe que mede um tempo específico e diz se esse tempo foi completo ou não
//Serve para alguns delays, nesse caso, um delay para voltar a verificação de GroundCheck do personagem após pular
[System.Serializable]
public class MedidorDeTempo
{
    [Header("Tempo até que a operação se complete")]
    public float tempoAEsperar;
    //Tempo desde que a operação ocorre (funciona como uma porcentagem);
    float tempoDesdeAEspera;

    //Reinicia o tempo desde que a operação ocorre
    public void ReiniciarTempo()
    {
        tempoDesdeAEspera = 0;
    }

    //Acrescenta um valor ao tempo desde que a operação ocorre, como acrescentar um valor a uma porcentagem
    public void Contar(float valor)
    {
        tempoDesdeAEspera += valor;
    }

    //Verifica se o tempo desde que a operação ocorre é maior que o tempo a esperar, para definir se a operação está completa ou não
    public bool Completo()
    {
        return tempoDesdeAEspera > tempoAEsperar;
    }
}
