using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class HealthDisplay : MonoBehaviour {

    public Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

	public void UpdateHealth(float percentage)
    {
        if (image)
	    {
		    image.fillAmount = percentage;
	    }
    }
}
