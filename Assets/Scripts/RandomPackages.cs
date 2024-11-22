using UnityEngine;

public class RandomPackages : MonoBehaviour
{
    public Sprite[] Packages;
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if(spriteRenderer.sprite != null)
        {
            return;
        }
        else
        {
            RandomPackagesPicker();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // RandomPackagesPicker();
    }
    void RandomPackagesPicker()
    {
        if(Packages.Length>0)
        {
            int RandomPackagesIndex = Random.Range(0,Packages.Length);
            spriteRenderer.sprite = Packages[RandomPackagesIndex];
        }
        else
        {
            Debug.Log("Do not have any packages");
        }
    }
}
