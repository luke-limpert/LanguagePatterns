using static ShapeCore.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ShapeApi.Client;

namespace BlazorShape.Services
{
    public class ShapeService
    {
        private readonly ShapeClient _shapeClient;
        public ShapeService() 
        {
            _shapeClient = new ShapeClient(new HttpClient());
            _shapeClient.BaseUrl = "http://localhost:5000/";
        }

        public async void GetShapes()
        {
            var shapes = await _shapeClient.GetShapesAsync();
            Console.WriteLine("blub");
        }

    }
}
