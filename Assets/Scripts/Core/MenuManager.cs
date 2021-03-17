using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject cthulubirthday;
    public GameObject movementindonjon;
    public GameObject humans;
    public GameObject explications1;
    public GameObject explications2;
    public GameObject go;

    public void OnClick_Start()
    {
        SceneManager.LoadScene(1);
    }

    public void Cthulubirthday()
    {
        cthulubirthday.SetActive(false);
        movementindonjon.SetActive(true);
    }

    public void Movementindonjon()
    {
        movementindonjon.SetActive(false);
        humans.SetActive(true);
    }

    public void Humans()
    {
        humans.SetActive(false);
        explications1.SetActive(true);
    }

    public void Explications1()
    {
        explications1.SetActive(false);
        explications2.SetActive(true);
    }
    public void Explications2()
    {
        explications2.SetActive(false);
        go.SetActive(true);
    }
}
