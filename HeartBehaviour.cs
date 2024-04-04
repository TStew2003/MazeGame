using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class HeartBehaviour : MonoBehaviour
{
    public GameObject flag;
    public GameObject hitEffectPrefab;
    private bool bottom;
    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            GameObject go = col.gameObject;

            PlayerMovement player = go.GetComponent<PlayerMovement>();

            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);


            GameManager.instance().SwitchMaze();
            //GameManager.instance().updateHeartText();
            if (bottom)
            {
                Vector3 pos = new Vector3(0.5f, 8f, 0f);
                this.transform.position = pos;
                bottom = false;
            }
            else
            {
                Vector3 pos = new Vector3(0.5f, -7f, 0f);
                this.transform.position = pos;
                bottom = true;
            }

        }

    }

    public void Reset()
    {
        Vector3 pos = new Vector3(0.5f, -7f, 0f);
        this.transform.position = pos;
        this.gameObject.SetActive(true);
        bottom = true;
    }
}
