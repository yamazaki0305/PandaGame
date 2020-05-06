using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LessonImg : MonoBehaviour
{

    public GameObject LessonObj;
    private Image image;
    private Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = Resources.Load<Sprite>("LessonIMG/"+ LessonObj.GetComponent<LevelController>().Level);
        image = this.GetComponent<Image>();
        image.sprite = sprite;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
