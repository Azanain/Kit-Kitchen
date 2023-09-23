using UnityEngine;
using Zenject;

public class ClientFactory
{
    private ClientPool _clientPool;

    public Client CreateClient(Vector3 position, Sprite clientSprite)
    {
        Client newClient = _clientPool.GetClient(position);
        SpriteRenderer spriteClient = newClient.GetComponent<SpriteRenderer>();
        spriteClient.sprite = clientSprite;

        return newClient;
    }
    
    public void SetClientPool(ClientPool clientPool) => _clientPool = clientPool;
}