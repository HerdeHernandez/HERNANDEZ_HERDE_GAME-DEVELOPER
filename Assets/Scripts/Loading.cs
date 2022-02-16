using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        slider.value = 0;
        this.GetComponent<Animator>().enabled = false;
    }

    private void Update()
    {
        slider.value += .005f;

        if (slider.value >= 1)
        {
            //

            //Show Profile after Loading
            this.GetComponent<Animator>().enabled = true;
            this.transform.parent.GetChild(0).gameObject.SetActive(true);
            this.transform.parent.GetChild(0).gameObject.GetComponent<CanvasGroup>().alpha = 1;
            StartCoroutine(load());
        }
    }

    IEnumerator load()
    {
        yield return new WaitForSeconds(1.1f);
        this.gameObject.SetActive(false);
        
    }
}
