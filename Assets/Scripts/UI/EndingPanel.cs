using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EndingPanel : MonoBehaviour
{
    public GameObject panel;
    public Text gameResult;
    float time = 0;

    private void Start()
    {
        if (Managers.StageManager.Player.IsDead == false)
        {
            gameResult.text = "Success Mission!";
            gameResult.color = new Color(240/255f, 240/255f, 90/255f, 255/255f);
        }

        time = 0;
        Destroy(Managers.StageManager.Player);
        Destroy(Managers.Instance.gameObject);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > 5&&time<73)
        {
            transform.Translate(Vector3.up * Time.deltaTime*175.0f);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("MainLobbyScene");
        }
    }
}
