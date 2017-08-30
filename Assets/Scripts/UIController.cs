using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class UIController : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject pauseMenu;

    public GameObject playerShoot;

    private bool paused;

    //GameObject[] pauseObjects;

    public Image fillBar;
    public Image equipWepIcon;
    public Text numHealth;
    public Text Ammo;

    public GameObject player;

    int health = 0;
    // Use this for initialization
    void Start()
    {
        // wepBase = GetComponent<WeaponBase>();
        //pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        //hidePaused();
    }


    void Update()
    {

        if (player)
        {
            Update_UI();

        }
        //uses the p button to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }


    void Update_UI()
    {
        if (!player.GetComponentInChildren<WeaponController>().equipWep)
            return;
        else
        {
            if (!player)
                return;
            var equiptWep = player.GetComponentInChildren<WeaponController>().equipWep.GetComponent<WeaponBase>();
            fillBar.fillAmount = (float)player.GetComponent<PlayerHealth>().health / player.GetComponent<PlayerHealth>().maxHealth;
            if (health > player.GetComponent<PlayerHealth>().health)
            {
                transform.root.GetComponentInChildren<DamageBorder>().Owch();
            }
            else if (health < player.GetComponent<PlayerHealth>().health && health != 0)
            {
                transform.root.GetComponentInChildren<DamageBorder>().OhhhYea();
            }
            health = player.GetComponent<PlayerHealth>().health;
            numHealth.text = health.ToString();
            Ammo.text = equiptWep.curBullets.ToString() + "/" + equiptWep.ammoRemaining.ToString();
            // curAmmo.text = equiptWep.curBullets.ToString();
            //maxAmmo.text = equiptWep.ammoRemaining.ToString();
        }
    }



    void OnDestroy()
    {

    }

    void GameOverActive()
    {
        gameOverPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    //shows objects with ShowOnPause tag
    public void showPaused()
    {
        //foreach (GameObject g in pauseObjects)
        //{
        //    //g.SetActive(true);
        //    pauseMenu.SetActive(true);
        //    Time.timeScale = 0;
        //    playerShoot.SetActive(false);
        //}
    }

    //hides objects with ShowOnPause tag
    public void hidePaused()
    {
        //foreach (GameObject g in pauseObjects)
        //{
        //    //g.SetActive(false);
        //    pauseMenu.SetActive(false);
        //    Time.timeScale = 1;
        //    playerShoot.SetActive(true);
        //}
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}