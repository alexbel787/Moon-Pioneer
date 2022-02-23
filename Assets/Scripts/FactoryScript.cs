using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FactoryScript : MonoBehaviour
{
    public bool isFactoryOn;
    [SerializeField] private bool isBusy;
    [SerializeField] private GameObject productPrefab;
    [SerializeField] private float productionTime;
    [SerializeField] private WarehouseInScript[] warehousesIn;
    [SerializeField] private WarehouseOutScript warehouseOut;
    [SerializeField] private GameObject factoryTextObj;
    [SerializeField] private GameObject smokeParticle;

    private GameManagerScript GMS;

    private void Start()
    {
        GMS = GameObject.Find("GameManager").GetComponent<GameManagerScript>();

        StartCoroutine(ProductionCycleCoroutine());
    }

    private IEnumerator ProductionCycleCoroutine()
    {
        while (true)
        {
            if (isFactoryOn && !isBusy)
            {
                if (warehouseOut.storedProductsList.Count < warehouseOut.maxCapacity)
                {
                    bool allResourcesStocked = true;
                    foreach (WarehouseInScript wis in warehousesIn)
                        if (wis.storedProductsList.Count == 0)
                        {
                            allResourcesStocked = false;
                            WarningText("NO resources!");
                            smokeParticle.SetActive(false);
                            isFactoryOn = false;
                            break;
                        }
                    if (allResourcesStocked)
                    {
                        factoryTextObj.SetActive(false);
                        smokeParticle.SetActive(true);
                        foreach (WarehouseInScript wis in warehousesIn) //consume resources to produce product
                        {
                            GameObject lastInList = wis.storedProductsList[wis.storedProductsList.Count - 1];
                            wis.storedProductsList.Remove(lastInList);
                            Destroy(lastInList);
                        }
                        yield return new WaitForSeconds(productionTime);
                        float posY = warehouseOut.storedProductsList.Count * .075f + .05f;
                        var product = Instantiate(productPrefab, warehouseOut.transform.position + new Vector3(0, posY, 0),
                            Quaternion.identity);
                        product.transform.SetParent(warehouseOut.transform);
                        warehouseOut.storedProductsList.Add(product);
                    }
                }
                else
                {
                    WarningText("Warehouse is FULL!");
                    smokeParticle.SetActive(false);
                    isFactoryOn = false;
                }
            }
            yield return new WaitForSeconds(.2f);
        }
    }

    private void WarningText(string txt)
    {
        factoryTextObj.GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(transform.position);
        factoryTextObj.GetComponent<Text>().text = txt;
        factoryTextObj.SetActive(true);
    }

}
