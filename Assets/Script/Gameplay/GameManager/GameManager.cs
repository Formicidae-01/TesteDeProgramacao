using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Classe que realizará operações durante as fases
    //Gerando Singleton
    public static GameManager Instance;

    private void Awake() 
    {
        if (Instance)
        {
            Destroy(gameObject);
        }    

        else
        {
            Instance = this;
        }
    }

    //Vidas do jogador
    public int vidas = 3;
    //Texto que exibirá as vidas do jogador na tela
    public Text textoVidas;

    //Moedas do jogador
    int moedas;
    //Texto que exibirá as moedas do jogador na tela
    public Text textoMoedas;

    //Animador do Canvas que faz transições
    Animator telaFimDeJogo;

    //Script que cuidará dos anúncios
    GP_Anuncios anuncios;

    void Start()
    {
        //Localiza os componentes desejados, para que não haja dependência direta entre esse script e os outros
        textoVidas = GameObject.Find("VidaTexto").GetComponent<Text>();
        textoMoedas = GameObject.Find("MoedasTexto").GetComponent<Text>();
        telaFimDeJogo = GameObject.Find("PainelEFader").GetComponent<Animator>();
        anuncios = FindObjectOfType<GP_Anuncios>();
    }

    //Esse método acrescentará (ou diminuirá) o valor de moedas que o jogador possui, atualizando também seu texto na HUD
    public void AdicionarMoedas(int valor)
    {
        moedas += valor;
        AtualizarTextoHUD();
    }

    //Esse método acrescentará (ou diminuirá) o valor de vidas que o jogador possui, atualizando também seu texto na HUD
    public void AlterarVidas(int valor)
    {
        vidas += valor;
        AtualizarTextoHUD();
    }

    //Esse método atualizará os textos na HUD relativos a quantidade de moedas e vidas na tela
    public void AtualizarTextoHUD()
    {
        textoVidas.text = vidas.ToString();
        textoMoedas.text = moedas.ToString();
    }
    
    //Esse método finalizará o jogo, exibindo o GameOver ou realizando a transição para voltar ao menu (Dependendo do boolean "Completo")
    public void FinalizarJogo(bool completo)
    {
        //Executa função caso a fase tenha sido completa, passando a pontuação à classe estática que guarda dados
        if (completo)
        {
            RepassarPontuacao();
        }

        else
        {
            DadosSalvos.completo = false;
        }

        VoltarMenu();
        telaFimDeJogo.SetTrigger("FadeOutSemPainel");
    }

    //Limpa os dados salvos na classe estática de Dados
    public void ReiniciarPontuacao()
    {
        DadosSalvos.moedas = 0;
        DadosSalvos.vidasRestantes = 0;
        DadosSalvos.completo = false;
    }

    //Repassa os valores dessa classe para a classe estática
    public void RepassarPontuacao()
    {
        ReiniciarPontuacao();
        DadosSalvos.moedas = moedas;
        DadosSalvos.vidasRestantes = vidas;
        DadosSalvos.completo = true;
    }

    //Esse método iniciará a Corrotina que espera dois segundos antes de carregar o menu
    //Método criado para poder incluir a Corrotina em um dos métodos de evento do botão que retorna ao menu na tela de GameOver
    public void VoltarMenu()
    {
        StartCoroutine(CO_VoltarAoMenu());
    }

    public IEnumerator CO_VoltarAoMenu()
    {
        yield return new WaitForSeconds(2);
        anuncios.ReproduzirAnuncio();
        SceneManager.LoadScene("Menu");
    }
}
