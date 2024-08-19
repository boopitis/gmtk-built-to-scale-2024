using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag_Bar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private GameObject[] fullFlags;

    // How You Would Make A List
    //private IList<int> healthNums = new List<int>();




    private void Start()
    {
        StartHearts();
    }

    private void Update()
    {
        // Debugging purposes.
        //Debug.Log(fullHearts[4].GetComponent<Full_Heart>().isDead());

        HealthManagement();
        // Make code to make sure the current health and the displayed hearts are equal.
    }
    


    public void Heartbreak(int num)
    // only public while testing.
    {
        // it would allow for breaking the appropriate amount of hearts.
        var pos = new List<int>();
        for (int i = 8; i >= 0 && i >= 9 - num; i--)
        // Due to the way I arranged it, it starts from the outer most one.
            {
                if (fullFlags[i].activeInHierarchy)
                     pos.Add(i);
            }
        
        if (pos.ToArray().Length >= 1)
            {
                int[] healthNumsD1 = pos.ToArray();
                foreach (int i in healthNumsD1)
                    fullFlags[i].GetComponent<Full_Flags>().Deactivate();
            }
    }

    private void HeartGain(int num)
    // Private or Public depending on if there is an animation for gaining more hearts.
    {
        var pos = new List<int>();
        // It would allow for regaining the appropriate amount of hearts.
        for (int i = 0; i < fullFlags.Length && i < num; i++)
        // There are 9 hearts in total.
            {
                if (!fullFlags[i].activeInHierarchy)
                    pos.Add(i);
            }

        if (pos.ToArray().Length >= 1)
            {
                int[] healthNumsU1 = pos.ToArray();
                foreach (int i in healthNumsU1)
                    fullFlags[i].GetComponent<Full_Flags>().Reactivate();
            }
    }

    private void StartHearts()
    {
        // Starting with 5 hearts.
        Heartbreak(9 - (int)playerHealth.health);
        // Breaks all excess hearts.
    }

    private void HealthManagement()
    {
        // Makes sure there are the correct amount of dead hearts.
        var opos = new List<int>();
        for (int i = (int)playerHealth.health; i >= 0 && i < 9; i++)
        // because the player healh number is already 1 above the index position of the heart.
            {
                if (fullFlags[i].activeInHierarchy && fullFlags[i].GetComponent<Full_Flags>().isAlive())
                    opos.Add(i);
            }

        if (opos.ToArray().Length >= 1)
            {
                int[] healthNums1 = opos.ToArray();
                foreach (int i in healthNums1)
                    {
                        fullFlags[i].GetComponent<Full_Flags>().HeartApp("dead");
                    }
            }

        // Makes sure there are the correct amount of alive hearts.
        var apos = new List<int>();
        for (int i = (int)playerHealth.health - 1; i >= 0 && i < (int)playerHealth.health; i--)
        {
            if (fullFlags[i].activeInHierarchy && fullFlags[i].GetComponent<Full_Flags>().isDead())
                apos.Add(i);
        }

        if (apos.ToArray().Length >= 1)
            {
            int[] healthNumsh1 = apos.ToArray();
            foreach (int i in healthNumsh1)
                {
                    fullFlags[i].GetComponent<Full_Flags>().HeartApp("alive");
                }
            }
    }
}
