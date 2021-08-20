using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GP_Cogumelo : MonoBehaviour
{
    public Animator anim;
    public Transform orientadorPulo;

    public void Sacudir()
    {
        anim.SetTrigger("Sacudir"); 
    }
}
