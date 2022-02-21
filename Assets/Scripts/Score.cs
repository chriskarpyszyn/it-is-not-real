using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Transform player;
    public Text scoreText;
    private int score;

    // Update is called once per frame
    void Update()
    {
        int playerCeilingPosition = Mathf.CeilToInt(player.position.x)-1; 
        if (score < playerCeilingPosition) {
            score = playerCeilingPosition;
            scoreText.text = score.ToString("0");
        }
       

    }
}
