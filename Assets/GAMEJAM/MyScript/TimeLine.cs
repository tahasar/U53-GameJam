using UnityEngine;

namespace GAMEJAM.MyScript
{
    public class TimeLine : MonoBehaviour
    {

        [SerializeField] GameManager gameManager;
        Animator anim;
        private static readonly int Menu = Animator.StringToHash("Menu");

        public void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void Update()
        {
            if (gameManager.storyActive)
            {
                anim.SetBool(Menu, true);
            }
            else
            {
                anim.SetBool(Menu, false);
            }
        }
    }
}
