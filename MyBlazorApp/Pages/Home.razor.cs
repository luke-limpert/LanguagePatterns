using static InteropTypes.Temperature;
using static InteropTypes.Temperature2;
using InteropTypes.Temperature3;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace MyBlazorApp.Pages;
public partial class Home
{
    Temperature temp1 = new(50);
    Temperature2 temp2 = new(50);
    Temperature3 temp3 = new(50);

    private void TemperatureChange(ChangeEventArgs e)
    {
        if (double.TryParse(e.Value?.ToString(), out var parsed))
        {
            temp2 = new Temperature2(parsed);
        }
        else
        {
            temp2 = null;
        }
    }
}