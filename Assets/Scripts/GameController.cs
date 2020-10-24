using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int whoseTurn; //  daca e 0-x ori 1-0
    public int turnCount; //numara mutarile
    public GameObject[] turnIcons; //arata al cui e randul
    public Sprite[] playIcons;  //poza 0 = x si 1 = 0
    public Button[] tictaetoespaces; //bunoanele de pe tabla
    public int[] markedspaces;  //tinem cont cine ce a apasat si unde
    public Text winnerAlex; //afiseaza mesaj
    public Text winnerFlo; //afiseaza mesaj
    public GameObject[] winningline; //  taieturi cand a castigat cineva
    public Text AlexScore, FloScore; //tine scorul text
    public int scAl, scFlo; //scor int folosit in script
    public Button x, zero; //se alege jucatorul care incepe, by default e x
    public Text losers; //daca nu castiga nimeni se afiseaza mesaj
    public AudioSource sound, win;


    // Start is called before the first frame update
    void Start()
    {
        GameSetup();
    }

    void GameSetup()  //seteaza matricea de joc
    {
        whoseTurn = 0;
        turnCount = 0;
        losers.gameObject.SetActive(false);
        turnIcons[0].SetActive(true);
        turnIcons[1].SetActive(false);

        for(int i = 0; i < tictaetoespaces.Length; i++)
        {
            tictaetoespaces[i].interactable = true;
            tictaetoespaces[i].GetComponent<Image>().sprite = null;
        }

        for(int i = 0; i < markedspaces.Length; i++)
        {
            markedspaces[i] = -1;        
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TicTacToeButton(int number)
    {
        
        x.interactable = false;  //cand jocul incepe nu se mai poate alege al cui rand e
        zero.interactable = false;
        tictaetoespaces[number].image.sprite = playIcons[whoseTurn]; //apare imagine
        tictaetoespaces[number].interactable = false; //butonul devine inactiv
        markedspaces[number] = whoseTurn; //vectorul ce verifica castigatorul
        turnCount++; //numara mutarile

        if(turnCount <= 4)   //declansare sunet buton
            PlaySound();

        if (turnCount > 4)
        {
            winer(); //se verifica daca a castigat cineva
            
        }

        

        if(whoseTurn == 0)
        {
            whoseTurn = 1; //bulina arata al cui rand e
            turnIcons[0].SetActive(false);
            turnIcons[1].SetActive(true);
        }

        else
        {
            whoseTurn = 0;
            turnIcons[0].SetActive(true);
            turnIcons[1].SetActive(false);
        }
    }

    public void winer() //se cauta castigator in vector
    {
        int wini = -1;
        if (markedspaces[0] == markedspaces[1] && markedspaces[1] == markedspaces[2] && markedspaces[0] != -1)
            wini = 0;
        else if (markedspaces[3] == markedspaces[4] && markedspaces[4] == markedspaces[5] && markedspaces[3] != -1)
            wini = 1;
        else if (markedspaces[6] == markedspaces[7] && markedspaces[7] == markedspaces[8] && markedspaces[6] != -1)
            wini = 2;
        else if (markedspaces[0] == markedspaces[3] && markedspaces[3] == markedspaces[6] && markedspaces[0] != -1)
            wini = 3;
        else if (markedspaces[1] == markedspaces[4] && markedspaces[4] == markedspaces[7] && markedspaces[1] != -1)
            wini = 4;
        else if (markedspaces[2] == markedspaces[5] && markedspaces[5] == markedspaces[8] && markedspaces[2] != -1)
            wini = 5;
        else if (markedspaces[0] == markedspaces[4] && markedspaces[4] == markedspaces[8] && markedspaces[0] != -1)
            wini = 6;
        else if (markedspaces[2] == markedspaces[4] && markedspaces[4] == markedspaces[6] && markedspaces[2] != -1)
            wini = 7;

        if (wini == -1) //daca nu e mutare castigatoare se declanseaza sunetul butonului
        {   PlaySound();
            if (turnCount == 9)
            {
                nobodyWins();
            }
        }

        else if (wini != -1) //se anunta castigatorul
        {
            winnerDisplay(wini);
            return;
        }
        
       
    }

    void winnerDisplay(int index)
    {

        if (whoseTurn == 0)
        {
            winnerAlex.gameObject.SetActive(true);
            winnerAlex.text = "The fattest, the best!";
            scAl++;
            AlexScore.text = scAl.ToString();
        }
        else if(whoseTurn == 1)
        {
            winnerFlo.gameObject.SetActive(true);
            winnerFlo.text = "Give me more biscuits.";
            scFlo++;
            FloScore.text = scFlo.ToString();
        }

        winningline[index].SetActive(true); //se afiseaza linia castigatoare pentru a marca combinatia
        win.Play();

        for (int i = 0; i < tictaetoespaces.Length; i++) //nu mai poti apasa campurile
        {
            tictaetoespaces[i].interactable = false;
           
        }
    }

    public void Rematch()
    {
        GameSetup();
        for(int i = 0; i < winningline.Length; i++)
        {
            winningline[i].SetActive(false);
        }
        winnerAlex.gameObject.SetActive(false); //dispare textul ce anunta cine a castigat
        winnerFlo.gameObject.SetActive(false);
        x.interactable = true;  //poti alege cine incepe jocul
        zero.interactable = true;
    }

    public void Restart()
    {
        Rematch();
        scAl = 0;
        scFlo = 0;
        AlexScore.text = scAl.ToString();
        FloScore.text = scFlo.ToString(); 
    }

    public void WhoStarts(int who)
    {
        if (who == 0)
        {
            whoseTurn = 0;
            turnIcons[0].SetActive(true);
            turnIcons[1].SetActive(false);

        }
        else if (who == 1)
        {
            whoseTurn = 1;
            turnIcons[0].SetActive(false);
            turnIcons[1].SetActive(true);
        }
            
    }

    public void nobodyWins()
    {
        losers.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void PlaySound()
    {
        sound.Play();
    }
}
