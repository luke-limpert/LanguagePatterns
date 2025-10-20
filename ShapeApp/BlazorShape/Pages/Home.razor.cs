using BlazorShape.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorShape.Pages
{
    public partial class Home
    {
        [Inject] public ShapeService _client { get; set; } = default!;

        public async void CheckShapes()
        {
            _client.GetShapes();
        }
    }
}
