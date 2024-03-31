using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;


class test
{
    public static async System.Threading.Tasks.Task test1()
    {
        string networkName = "backend";

        // Создание экземпляра DockerClient для взаимодействия с Docker API
        DockerClient client = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();
        Console.WriteLine("1");
        // Получение списка контейнеров в Docker-сети
        IList<NetworkResponse> networks = await client.Networks.ListNetworksAsync();
        foreach (var net in networks)
        {
            Console.WriteLine(net.Name);
        }
        Console.WriteLine("2");
        var network = networks.FirstOrDefault(n => n.Name == networkName);
        IDictionary<string, EndpointResource> containers = network.Containers;

        // Вывод IP-адресов контейнеров
        foreach (var container in containers)
        {
            var containerInspect = await client.Containers.InspectContainerAsync(container.Key);
            Console.WriteLine($"Container: {containerInspect.Name}, IP Address: {containerInspect.NetworkSettings.Networks[networkName].IPAddress}");
        }
    }
}