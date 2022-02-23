using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseOutScript : MonoBehaviour
{
    public int maxCapacity;
    public List<GameObject> storedProductsList;
    [SerializeField] private AudioSource AS;
    private bool isBusy;
    private PlayerScript PS;
    private FactoryScript FS;

    private void Start()
    {
        FS = transform.root.GetComponent<FactoryScript>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isBusy)
        {
            PS = other.GetComponent<PlayerScript>();
            if (storedProductsList.Count > 0 && PS.carryProductList.Count < PS.maxCarryProducts)
            {
                isBusy = true;
                StartCoroutine(PickUpProductCoroutine());
                FS.isFactoryOn = true;
            }
        }
    }

    private IEnumerator PickUpProductCoroutine()
    {
        float posY = PS.carryProductList.Count * .075f + 1f;
        var product = storedProductsList[storedProductsList.Count - 1];
        product.transform.position = PS.transform.position + new Vector3(0, posY, 0);
        product.transform.SetParent(PS.transform);
        PS.carryProductList.Add(product);
        storedProductsList.RemoveAt(storedProductsList.Count - 1);
        AS.Play();
        yield return new WaitForSeconds(.2f);
        isBusy = false;
    }
}
