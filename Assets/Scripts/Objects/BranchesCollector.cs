using UnityEngine;

public class BranchesCollector : MonoBehaviour
{
    [SerializeField] GameObject branchMiddle;
    [SerializeField] GameObject brunchUp;
    [SerializeField] GameObject cartHintTrigger;
    [SerializeField] GameObject brigeHintTrigger;
    int counter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BranchesUp"))
        {
            Destroy(other.gameObject);
            if (counter == 0) { branchMiddle.gameObject.SetActive(true); counter++; }
            else 
            { 
                brunchUp.gameObject.SetActive(true);

                GameObject cartTriggerObject = GameObject.FindGameObjectWithTag("CartTrigger");
                if (cartTriggerObject != null)
                {
                    CartBehevior cartBehavior = cartTriggerObject.GetComponent<CartBehevior>();
                    if (cartBehavior != null)
                    {
                        cartBehavior.enabled = true;
                    }
                }

                    cartHintTrigger.SetActive(true); 
                    brigeHintTrigger.SetActive(true);
            }
        }
    }
}
