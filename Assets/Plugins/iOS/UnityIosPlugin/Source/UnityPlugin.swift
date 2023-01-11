import Foundation
import CoreLocation

@objc public class UnityPlugin : NSObject, CLLocationManagerDelegate  {
    
    var manager: CLLocationManager = CLLocationManager()
    var result = ""
    @objc public static let shared = UnityPlugin()
    @objc public func AddTwoNumber(a:Int,b:Int ) -> String {
        
        return result;
    }

    @objc public func Start(){
        manager.delegate = self
        manager.desiredAccuracy = kCLLocationAccuracyBest
        manager.requestWhenInUseAuthorization()
        manager.startUpdatingLocation()
    }

    func locationManager( manager: CLLocationManager, didUpdateLocations locations: [CLLocation]){
        guard let first = locations.first else{
            return
        }

        result = "\(first.coordinate.longitude) | \(first.coordinate.latitude)"

    } 
}
