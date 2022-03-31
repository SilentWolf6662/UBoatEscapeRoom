using UBER.Util;

public class ItemWorldSpawner : CacheBehaviour2D 
{
    public Item item;

    private void Awake() 
    {
        ItemWorld.SpawnItemWorld(transform.position, item);
        Destroy(gameObject);
    }
}
