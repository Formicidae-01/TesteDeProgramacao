using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe de objetos coletáveis
public class GP_Coletavel : MonoBehaviour
{
    [Header("Partículas")]
    public ParticleSystem pSystem;
    [Header("Renderizador deSprite")]
    public SpriteRenderer sRenderer;
    [Header("Colisor")]
    public BoxCollider2D boxCollider;

    //Método de coleta do objeto, emitindo partículas como feedback, desativando o renderizador e colisor, então destruindo o objeto após um segundo
    public void Coletar()
    {
        pSystem.Emit(3);
        sRenderer.enabled = false;
        boxCollider.enabled = false;
        Destroy(gameObject,1);
    }

    //Executa o método de coleta ao haver colisão (acontece somente entre esse objeto e o jogador)
    private void OnTriggerEnter2D(Collider2D other) 
    {
        Coletar();    
    }
}
