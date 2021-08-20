using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GP_Dinossauro : MonoBehaviour
{
    //Movimentação
    //Valor X, que define a direção para qual o personagem se moverá
    float x;

    [Header("Variações de velocidade de movimento e força de pulo")]
    public float[] velMovimento, forcaPulo;
    
    //Índices que definem a variação de velocidade de movimento e força de pulo a ser usada
    int indiceMovimento, indicePulo;

    //Valor que define se o personagem está travado, para que não se mova horizontalmente
    bool travado;

    //Controles
    //Valores que definem se os comandos de corrida e pulo estão sendo pressionados (útil para os comandos na tela)
    bool segurandoCorrer, segurandoPular;

    //GroundCheck
    [Header("Posições origem e destino do colisor de GroundCheck do personagem")]
    public Transform[] overlapChao = new Transform[2];

    [Header("Camadas de chão que podem ser verificadas no GroundCheck")]
    public LayerMask camadasChao;

    //Valores que indicam se o personagem está no chão e se pode fazer essa verificação
    bool chao, podeChecarChao;

    [Header("Atraso no qual o personagem não pode fazer a verificação de chão (Aplicado após pular)")]
    public MedidorDeTempo delayDeGroundCheck;

    [Header("Colisor do objeto com o qual o personagem colidiu para definir que está no chão")]
    public Collider2D colisorChao;

    //Cogumelo
    //Script do Cogumelo no qual o jogador pulará
    GP_Cogumelo tmpCogumelo;
    //Valor que define se o jogador está pulando com um cogumelo
    bool pulandoComCogumelo;

    //Respawn
    [Header("Tempo de respawn após morrer")]
    public float tempoDeRespawn;

    //Componentes
    public Rigidbody2D rb;
    public Animator anim;
    public ParticleSystem pSystem;

    //Fim de fase
    //Valor que indica se o personagem está terminando a fase, para que ande automaticamente
    bool finalizando;

    private void Update() 
    {
        //Recebe os comandos de teclado caso o jogo esteja sendo executado em Windows
        if (Application.platform == RuntimePlatform.WindowsPlayer) {ReceberComandosTeclado();}

        //Processa os comandos recebidos
        ProcessarComandos();
    }
    
    private void FixedUpdate() 
    {
        //Métodos que envolvem física e animação
        Mover();
        VerificarChao();
        Virar();
        Animar();
    }

    //Método que recebe os comandos do teclado
    void ReceberComandosTeclado()
    {
        //Define valores de comando de acordo com teclas pressionadas
        ComandoX(Input.GetAxisRaw("Horizontal"));
        
        ComandoCorrer(Input.GetButton("Run"));

        ComandoPulo(Input.GetButton("Jump"));
    }

    void ProcessarComandos()
    {
        //Ignora os comandos caso o personagem esteja travado ou finaliando a fase
        if (travado || finalizando) {return;}
        //Processa comandos caso o personagem esteja no chão ou pulando com um cogumelo
        if (chao || pulandoComCogumelo)
        {
            //Se o comando de corrida estiver ativado, a segunda variação de velocidade de corrida é utilizada
            //Caso contrário, a primeira variação é usada
            if (segurandoCorrer) {indiceMovimento = 1;}
            else {indiceMovimento = 0;}

            //Se o comando de pulo estiver ativado, inicia um salto
            if (segurandoPular) 
            {
                IniciarPulo();
            }
        }
    }

    //Método que define o valor "X" de comando, a não ser que a fase esteja sendo finalizada
    public void ComandoX(float valor)
    {
        if (finalizando) {return;}
        x = valor;
    }

    //Método que define se o comando de corrida está sendo pressionado
    public void ComandoCorrer(bool pressionando)
    {
        segurandoCorrer = pressionando;
    }

    //Método que define se o comando de pulo está sendo pressionado
    public void ComandoPulo(bool pressionando)
    {
        segurandoPular = pressionando;
    }    

    //Define a velocidade X do RigidBody do personagem de acordo com o valor "X" de comando,
    void Mover()
    {
        //Define a velocidade X do Rigidbody como 0 caso o valor "travado" seja real
        if (travado) 
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;    
        }

        //Movimenta o personagem de acordo com valor "X" de comando, a variação de velocidade atual e a velocidade Y padrão
        rb.velocity = new Vector2(x * velMovimento[indiceMovimento], rb.velocity.y);
    }

    //Método que faz a verificação para definir se o personagem está no chão
    void VerificarChao()
    {
        //Caso seja possível fazer a verificação de chão, gera um colisor que vai da posição origem até a posição final definida pelos transform "Overlap"
        if (podeChecarChao)
        {
            colisorChao = Physics2D.OverlapArea(overlapChao[0].position, overlapChao[1].position, camadasChao);
            chao = colisorChao;
        }
       
        //Caso não seja possível fazer a verificação, é aplicado o delay até que a verificação possa ser feita novamente
        //Esse delay é usado para impedir que o personagem defina que está no chão logo após iniciar um salto e ter a chance de fazer um segundo salto indesejado caso o botão de pulo esteja pressionado
        else
        {
            chao = false;
            if (!delayDeGroundCheck.Completo())
            {
                delayDeGroundCheck.Contar(Time.deltaTime); 
            }
            
            else
            {
                podeChecarChao = true;
            }
        }

        //Verifica se o objeto pisado é um cogumelo, para executar um salto mais alto
        VerificarCogumelo();
    }

    //Método que verifica se o personagem pisou em um cogumelo para executar um grande salto
    void VerificarCogumelo()
    {
        //Método é interrompido caso o personagem não esteja caindo nem já pulando com um outro cogumelo
        //O segundo impedindo (!pulandoComCogumelo) previne o personagem de executar saltos adicionais ao pisar em um cogumelo
        if (rb.velocity.y >= 0 && !pulandoComCogumelo) return;

        //Executa função caso o personagem esteja no chão e pisando em um cogumelo
        if (chao && !pulandoComCogumelo)
        {
            if (colisorChao.gameObject.layer == LayerMask.NameToLayer("Cogumelo"))
            {
                //Executa animação do cogumelo que o faz 'sacudir', altera a variação da força de pulo e inicia um salto
                tmpCogumelo = colisorChao.gameObject.GetComponent<GP_Cogumelo>();
                tmpCogumelo.Sacudir();
                indicePulo = 1;
                IniciarPulo();
                pulandoComCogumelo = true;
            }
        }

        //Faz com que o personagem se prenda na posição da parte de cima do cogumelo por um breve instante, se soltando depois ao pular
        if (pulandoComCogumelo)
        {
            transform.position = new Vector2(transform.position.x, tmpCogumelo.orientadorPulo.position.y);
        }
    }

    //Método que rotaciona o personagem de acordo com o valor de comando X, para que ele se vire na direção em que se move
    void Virar()
    {
        if (x > 0) {transform.rotation = new Quaternion(0,0,0,0);}

        else if (x < 0) {transform.rotation = new Quaternion(0,-1,0,0);}
    }

    //Inicia a animação de pulo do personagem e bloqueia seu movimento
    void IniciarPulo()
    {
        travado = true;
        anim.SetTrigger("IniciarPulo");
    }

    //Aplica a força de pulo, fazendo com o que o personagem de falto salte
    //Esse método é utilizado como evento na animação de pulo do personagem
    public void AplicarPulo()
    {
        //Reinicia o delay até que o personagem possa checar se está no chão novamente
        delayDeGroundCheck.ReiniciarTempo();
        chao = false;
        podeChecarChao = false;

        //Caso o personagem esteja executando um salto a partir de um cogumelo e esteja com o botão de pulo pressionado, irá saltar ainda mais alto
        if (pulandoComCogumelo && segurandoPular) {indicePulo = 2;}
        //Caso o botão de corrida esteja pressionado, o personagem poderá se mover mais rápido horizontalmente durante o salto
        if (segurandoCorrer) {indiceMovimento = 1;}

        //Reinicia alguns valores do personagem, para que não esteja mais travado e o salto com cogumelo não seja mais considerado (afinal, essa condição já teve seu efeito utilizado nas linhas anteriores)
        pulandoComCogumelo = false;

        travado = false;

        //Aplica a força de pulo ao eixo Y do Rigidbody do personagem, causando o salto
        rb.velocity = new Vector2(rb.velocity.x, forcaPulo[indicePulo]);

        //Reinicia a variação da força de pulo
        indicePulo = 0;
    }

    //Método que administra as animações do personagem, de acordo com a física do jogador, corrida e verificação de chão
    void Animar()
    {
        anim.SetFloat("MovimentoX", rb.velocity.magnitude);
        anim.SetFloat("MovimentoY", rb.velocity.y);
        anim.SetBool("Correr", indiceMovimento == 1);
        anim.SetBool("Chao", chao);
    }

    //Método que causa a morte do personagem
    void Morrer()
    {
        //Subtrai uma vida do personagem
        GameManager.Instance.AlterarVidas(-1);
        //Reinicia a física do personagem
        rb.velocity = Vector2.zero;
        //Executa a animação de morte do personagem
        anim.SetTrigger("Morrer");
        //Emite partículas de feedback
        pSystem.Emit(3);
        //Desativa esse script
        enabled = false;
        //Inicia Corrotina que revive o personagem após um determinado tempo (se possível)
        StartCoroutine(Renascer());
    }

    //Método que finaliza o comportamento do personagem ao completar a fase
    void FinalizarFase()
    {
        //Faz com que o personagem desapareça depois de um determinado tempo
        StartCoroutine(Desaparecer());
        //Define que o personagem está finalizando a fase
        finalizando = true;
        //Define o valor de comando "X" como 1, para que ele ande automaticamente para a direita
        x = 1;
    }

    //Corrotina que apaga o personagem depois de alguns segundos e informa ao GameManager que a fase foi completa com sucesso
    IEnumerator Desaparecer()
    {
        yield return new WaitForSeconds(2);
        GameManager.Instance.FinalizarJogo(true);
        gameObject.SetActive(false);
    }

    //Corrotina que faz o renascimento do personagem
    IEnumerator Renascer()
    {
        yield return new WaitForSeconds(tempoDeRespawn);

        //Caso o personagem não tenha mais vidas, finaliza o jogo informando que a fase não foi completa com sucesso
        if (GameManager.Instance.vidas <= 0)
        {
            GameManager.Instance.FinalizarJogo(false);
        }

        //Caso o personagem tenha vidas, o transporta para o ponto de respawn mais próximo, reiniciando seu comportamento
        else
        {
            transform.position = FindObjectOfType<GP_PontosDeRespawn>().PontoMaisProximo(transform.position);
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Renascer");
            enabled = true;   
        }
    }

    //Administra a colisão do personagem, a não ser que ele esteja finalizando a fase
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (finalizando) {return;}

        //Adiciona um valor a pontuação de moedas caso colida com uma moeda
        if (other.gameObject.layer == LayerMask.NameToLayer("Moeda"))
        {
            GameManager.Instance.AdicionarMoedas(1);
        }

        //Anima o espinho que bloqueia a saída caso colida com uma chave
        if (other.gameObject.layer == LayerMask.NameToLayer("Chave"))
        {
            FindObjectOfType<GP_EspinhoSaida>().Abrir();
        }

        //Morre caso colida com um Espinho e o script esteja ativado (segunda condição previne o personagem de perder 2 vidas caso colida com 2 espinhos de uma vez)
        if (other.gameObject.layer == LayerMask.NameToLayer("Espinho"))
        {
            if (enabled)
            Morrer();
        }

        //Finaliza a fase ao colidir com a saída (colisor invisível)
        if (other.gameObject.layer == LayerMask.NameToLayer("Saída"))
        {
            FinalizarFase();
        }
    }
}
