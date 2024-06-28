using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health;

    private void Start()
    {
        int gridStaticMask = LayerMask.GetMask("GridStatic");
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z), Vector3.up, out hit, 1f, gridStaticMask))
        {
            Debug.Log("Hit gridStaticMask object!");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        PlayerController PlayerCont = collision.transform.GetComponent<PlayerController>();
        if (PlayerCont != null)
        {
            IDamagable idamagable = collision.transform.GetComponent<IDamagable>();
            idamagable.Damage(-health);

            Destroy(gameObject);
        }
    }
}
