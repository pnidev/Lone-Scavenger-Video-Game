using UnityEngine;

public class TutorialController : MonoBehaviour
{   
    [Header("Object chứa Animation Clip")]
    public GameObject animationObject;

    [Header("Clip Animation")]
    public AnimationClip clip;

    private Animation animationComp;

    void OnEnable()
    {
        if (animationObject != null && clip != null)
        {
            animationObject.SetActive(true);

            animationComp = animationObject.GetComponent<Animation>();
            if (animationComp == null)
                animationComp = animationObject.AddComponent<Animation>();

            animationComp.clip = clip;
            animationComp.Play();
        }
    }

    void OnDisable()
    {
        if (animationObject != null)
        {
            animationObject.SetActive(false);
        }
    }
}
