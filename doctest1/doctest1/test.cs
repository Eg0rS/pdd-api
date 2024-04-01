using Docker.DotNet;
using Docker.DotNet.Models;


class test
{
    public static async System.Threading.Tasks.Task test1()
    {
        string networkName = "pdd-api_backend";
        Console.WriteLine("0");
        // Создание экземпляра DockerClient для взаимодействия с Docker API
        DockerClient client = new DockerClientConfiguration().CreateClient();
        Console.WriteLine("1");
        // Получение списка контейнеров в Docker-сети
        IList<NetworkResponse> networks = await client.Networks.ListNetworksAsync();
        var network1 = networks.FirstOrDefault(n => n.Name == networkName);
        Console.WriteLine("1111");
        if (network1 != null)
        {
            var netInspect = await client.Networks.InspectNetworkAsync(network1.ID);

            var containerInNetwork = netInspect.Containers.FirstOrDefault(n => n.Value.Name == "pdd-api-api-1");
            var ip = containerInNetwork.Value.IPv4Address;
            Console.WriteLine(ip);
            //запрос к контейнеру по ip 
            await GetAsync("http://172.18.0.9");
            // await GetAsync("api");
            
           // await GetAsync(ip);
            

            Console.WriteLine(ToJson(netInspect));
        }

        Console.WriteLine("2222");
        foreach (var net in networks)
        {
            Console.WriteLine(ToJson(net));
            Console.WriteLine($"Network: {net.ID} {net.Name} {net.Containers.Count}");
            var containers = net.Containers;
            foreach (var container in containers)
            {
                Console.WriteLine($"Container: {container.Key}");
            }
        }

        Console.WriteLine("2");

        var containersAll = await client.Containers.ListContainersAsync(new ContainersListParameters());
        foreach (var container in containersAll)
        {
            Console.WriteLine($"Container: {container.ID} {container.NetworkSettings.Networks.Count} ");
            var names = container.Names;
            foreach (var name in names)
            {
                Console.WriteLine($"Name: {name}");
            }

            var networksContainer = container.NetworkSettings.Networks;
            foreach (var network in networksContainer)
            {
                Console.WriteLine($"Network: {network.Value.NetworkID} {network.Key}  {network.Value.IPAddress}");
            }
        }
    }

    private static string ToJson(object obj)
    {
        return System.Text.Json.JsonSerializer.Serialize(obj);
    }

    private static async Task GetAsync(string v)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                // Здесь указывается URL, на который нужно выполнить GET-запрос
                string url = $"{v}/Ping";

                // Выполняем GET-запрос
                HttpResponseMessage response = await httpClient.GetAsync(url);

                // Проверяем успешность запроса
                if (response.IsSuccessStatusCode)
                {
                    // Получаем содержимое ответа
                    string content = await response.Content.ReadAsStringAsync();

                    // Выводим содержимое ответа
                    Console.WriteLine("Ответ:");
                    Console.WriteLine(content);
                }
                else
                {
                    // Если запрос не удался, выводим соответствующее сообщение
                    Console.WriteLine($"Ошибка: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                // Если возникла ошибка при выполнении запроса, выводим сообщение об ошибке
                Console.WriteLine($"Ошибка: {e.Message}");
            }
        }
    }
}