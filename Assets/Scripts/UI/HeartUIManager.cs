using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUIManager : MonoBehaviour
{
    public GameObject heartPrefab;
    public List<Image> hearts;
    
    [SerializeField] private Health playerHealth;

    private void Awake()
    {
        for(int i = 0; i < Mathf.Ceil(playerHealth.GetMaxHealth() / 2f); i++)
        {
            GameObject heart = Instantiate(heartPrefab, this.transform);
            hearts.Add(heart.GetComponent<Image>());
        }
    }

    public void UpdateHealth()
    {
        float heartFill = playerHealth.GetHealth() / 2f;

        foreach(Image image in hearts)
        {
            image.fillAmount = heartFill;
            heartFill -= 1f;
        }
    }

    public void UpdateMaxHealth()
    {
        foreach (Image image in hearts)
        {
            Destroy(image.gameObject);
        }
        hearts.Clear();
        for(int i = 0; i < Mathf.Ceil(playerHealth.GetMaxHealth() / 2f); i++)
        {
            GameObject heart = Instantiate(heartPrefab, this.transform);
            hearts.Add(heart.GetComponent<Image>());
        }
    }
}
