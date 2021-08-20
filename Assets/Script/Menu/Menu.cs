using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Classe que executará as funções no menu inicial
public class Menu : MonoBehaviour
{
    [Header("Animador de telas do menu inicial")]
    public Animator anim;

    //Nome da próxima cena que será carregada
    string proximaCena;

    [Header("Componentes de texto que mostram a pontuação na tela de resultados")]
    public Text textoVidas, textoMoedas;

    private void Start() 
    {
        //Verifica se uma fase foi finalizada antes do jogador entrar na cena de Menu, para poder mostrar a tela de resultados
        if (DadosSalvos.completo)
        {
            //Desativa o script e exibe a tela de resultados
            enabled = false;
            ExibirResultados();
        }    
    }

    private void Update() 
    {
        //Durante a tela de título, o script se mantém ativado para realizar a transição para a tela de menu automaticamente caso o jogador execute um clique
        if (Input.GetButton("Fire1"))
        {
            anim.SetTrigger("Iniciar");   
            enabled = false;
        }
    }

    //Método que recebe o nome da próxima cena a ser carregada
    public void ReceberNomeDeFase(string nome)
    {
        proximaCena = nome;
    }

    //Carrega a cena seguinte (definida pela string "próxima cena")
    public void CarregarFase()
    {
        SceneManager.LoadScene(proximaCena);
    }

    //Método que exibirá os resultados da fase finalizada
    public void ExibirResultados()
    {
        anim.SetTrigger("Resultados");
        //Substitui o texto dos componentes de texto pela pontuação guardada na classe estática
        textoVidas.text = "VIDAS: " + DadosSalvos.vidasRestantes.ToString();
        textoMoedas.text = "MOEDAS: " + DadosSalvos.moedas.ToString();
    }
}
