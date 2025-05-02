namespace InteropTypes.ShippingRoute

module Location = 

    type Position = 
        {
            Longitude: double
            Latitude: double
        }

    let GetDistance(current: Position, newPosition: Position) = ((current.Longitude - newPosition.Longitude),(current.Latitude - newPosition.Latitude))

namespace InteropTypes.ShippingRoute2


type Position2 = 
    {
        Longitude: double
        Latitude: double
    }

module Location2 = 
    let GetDistance(current: Position2, newPosition: Position2) = ((current.Longitude - newPosition.Longitude),(current.Latitude - newPosition.Latitude))