using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    private Player playerS;

    #region Bullets
    [SerializeField]
    private GameObject uIBullet;
    private int bulletCount = 0;
    private float offset = 10f;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        playerS = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        transform.Find("HealthBar").transform.GetChild(0).GetComponent<Image>().fillAmount = playerS.health/100;
        transform.Find("WillpowerBar").transform.GetChild(0).GetComponent<Image>().fillAmount = playerS.Willpower / playerS.getMaxWillpower();

        //Bullets
        if(playerS.Ammo > bulletCount)
        {
            GameObject bullet = Instantiate(uIBullet, GameObject.Find("BulletOrigin").transform);
            //bullet.GetComponent<RectTransform>().localPosition = Vector3.zero;
            bullet.GetComponent<RectTransform>().localPosition += new Vector3(offset * GameObject.Find("BulletOrigin").transform.childCount, 0, 0);
            bulletCount += 1;
        }
        else if (playerS.Ammo < bulletCount)
        {
            Destroy(GameObject.Find("BulletOrigin").transform.GetChild(GameObject.Find("BulletOrigin").transform.childCount));
            bulletCount -= 1;
        }
    }

}
