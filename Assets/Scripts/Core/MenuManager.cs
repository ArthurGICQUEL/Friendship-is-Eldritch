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
    public GameObject ambiancesound;
    public GameObject gosound;
    public GameObject panelreg;
    public GameObject panelcredits;

    public void OnClick_Start()
    {
        ambiancesound.SetActive(false);
        gosound.SetActive(true);
        StartCoroutine(WaitGame());
    }

    IEnumerator WaitGame()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(2);
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

    public void Panelreg()
    {
        panelreg.SetActive(true);
    }

    public void Panelcredits()
    {
        panelcredits.SetActive(true);
    }

    public void Menu()
    {
        panelreg.SetActive(false);
        panelcredits.SetActive(false);
    }
}
