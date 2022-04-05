using UBER.Util;
using UnityEngine;

public class ItemAssets : CacheBehaviour2D 
{
    public static ItemAssets Instance { get; private set; }

    private void Awake() => Instance = this;

    public Transform pfItemWorld;

    public Sprite keySprite;
    public Sprite noteSprite;
}
