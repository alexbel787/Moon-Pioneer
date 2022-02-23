using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseInScript : MonoBehaviour
{
    public int maxCapacity;
    public enum ProductType
    {
        Product1,
        Product2,
        Product3
    }
    public ProductType canStoreProduct;

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
            if (storedProductsList.Count < maxCapacity && PS.carryProductList.Count > 0)
            {
                for (int i = PS.carryProductList.Count - 1; i >= 0; i--)
                {
                    if (PS.carryProductList[i].name.Contains(canStoreProduct.ToString()))
                    {
                        isBusy = true;
                        if (i < PS.carryProductList.Count - 1)  //move all above products in pile down if takes one below
                        {
                            for (int i2 = i + 1; i2 <= PS.carryProductList.Count - 1; i2++)
                                PS.carryProductList[i2].transform.position -= new Vector3(0, .075f, 0);
                        }
                        StartCoroutine(PutDownProductCoroutine(PS.carryProductList[i]));
                        FS.isFactoryOn = true;
                        break;
                    }
                }
            }
        }
    }

    private IEnumerator PutDownProductCoroutine(GameObject product)
    {
        float posY = storedProductsList.Count * .075f + .05f;
        product.transform.position = transform.position + new Vector3(0, posY, 0);
        product.transform.SetParent(transform);
        storedProductsList.Add(product);
        PS.carryProductList.Remove(product);
        AS.Play();

        yield return new WaitForSeconds(.2f);
        isBusy = false;
    }
}
