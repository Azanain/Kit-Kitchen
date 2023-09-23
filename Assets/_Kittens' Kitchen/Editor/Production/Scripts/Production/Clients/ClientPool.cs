using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPool : MonoBehaviour
{
    private Queue<Client> ClientQueue = new Queue<Client>();

    public void Initialize(Client clientTemplate, int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            Client newClient = InstantiateClient(clientTemplate, Vector3.zero);
            AddClientToPool(newClient);
        }
    }

    private Client InstantiateClient(Client clientTemplate, Vector3 position)
    {
        clientTemplate = Instantiate(clientTemplate, position, Quaternion.identity);
        clientTemplate.gameObject.SetActive(false);
        clientTemplate.transform.SetParent(transform);

        return clientTemplate;
    }

    public void AddClientToPool(Client client)
    {
        client.gameObject.SetActive(false);
        client.transform.SetParent(transform);
        ClientQueue.Enqueue(client);
    }

    public Client GetClient(Vector3 position)
    {
        Client newClient = ClientQueue.Dequeue();
        newClient.gameObject.SetActive(true);
        newClient.transform.position = position;

        return newClient;
    }
}
