using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe presente nos paineis de fase do menu, contendo o nome da fase a ser carregada e transferindo esse nome para o script de Menu
public class PainelDeFase : MonoBehaviour
{
    public string nomeDaFase;

    public void TransferirFase()
    {
        FindObjectOfType<Menu>().ReceberNomeDeFase(nomeDaFase);
    }
}
