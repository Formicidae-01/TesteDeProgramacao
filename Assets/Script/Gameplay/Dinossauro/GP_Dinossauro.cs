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

    void VerificarCogumelo()
    {
        if (rb.velocity.y >= 0 && !pulandoComCogumelo) return;

        if (chao && !pulandoComCogumelo)
        {
            if (colisorChao.gameObject.layer == LayerMask.NameToLayer("Cogumelo"))
            {
                tmpCogumelo = colisorChao.gameObject.GetComponent<GP_Cogumelo>();
                tmpCogumelo.Sacudir();
                indicePulo = 1;
                IniciarPulo();
                pulandoComCogumelo = true;
            }
        }

        if (pulandoComCogumelo)
        {
            transform.position = new Vector2(transform.position.x, tmpCogumelo.orientadorPulo.position.y);
        }
    }

    void Virar()
    {
        if (x > 0) {transform.rotation = new Quaternion(0,0,0,0);}

        else if (x < 0) {transform.rotation = new Quaternion(0,-1,0,0);}
    }

    void IniciarPulo()
    {
        travado = true;
        anim.SetTrigger("IniciarPulo");
    }

    public void AplicarPulo()
    {
        delayDeGroundCheck.ReiniciarTempo();
        chao = false;
        podeChecarChao = false;

        if (pulandoComCogumelo && segurandoPular) {indicePulo = 2;}
        if (segurandoCorrer) {indiceMovimento = 1;}

        pulandoComCogumelo = false;

        travado = false;
        rb.velocity = new Vector2(rb.velocity.x, forcaPulo[indicePulo]);

        indicePulo = 0;
    }

    void Animar()
    {
        anim.SetFloat("MovimentoX", rb.velocity.magnitude);
        anim.SetFloat("MovimentoY", rb.velocity.y);
        anim.SetBool("Correr", indiceMovimento == 1);
        anim.SetBool("Chao", chao);
    }

    [ContextMenu("Morrer")]
    void Morrer()
    {
        GameManager.Instance.AlterarVidas(-1);
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Morrer");
        pSystem.Emit(3);
        enabled = false;
        StartCoroutine(Renascer());
    }

    void FinalizarFase()
    {
        StartCoroutine(Desaparecer());
        finalizando = true;
        x = 1;
    }

    IEnumerator Desaparecer()
    {
        yield return new WaitForSeconds(2);
        GameManager.Instance.FinalizarJogo(true);
        gameObject.SetActive(false);
    }

    IEnumerator Renascer()
    {
        yield return new WaitForSeconds(tempoDeRespawn);

        if (GameManager.Instance.vidas <= 0)
        {
            GameManager.Instance.FinalizarJogo(false);
        }

        else
        {
            transform.position = FindObjectOfType<GP_PontosDeRespawn>().PontoMaisProximo(transform.position);
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Renascer");
            enabled = true;   
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (finalizando) {return;}

        if (other.gameObject.layer == LayerMask.NameToLayer("Moeda"))
        {
            GameManager.Instance.AdicionarMoedas(1);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Chave"))
        {
            FindObjectOfType<GP_EspinhoSaida>().Abrir();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Espinho"))
        {
            if (enabled)
            Morrer();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Saída"))
        {
            FinalizarFase();
        }
    }
}
