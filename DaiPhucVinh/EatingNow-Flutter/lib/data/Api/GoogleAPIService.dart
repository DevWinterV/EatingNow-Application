import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:geolocator/geolocator.dart';

class GoogleAPIService {
  final String apiKey;

  GoogleAPIService(this.apiKey);

  Future<AddressResult> fetchPlacesFromLocation(double latitude, double longitude) async {
    final query = '$latitude,$longitude';
    // final baseUrl = 'https://maps.googleapis.com/maps/api/place/textsearch/json';

    final baseUrl2 = 'https://maps.googleapis.com/maps/api/geocode/json?latlng=${latitude},${longitude}&key=AIzaSyAG61NrUZkmMW8AS9F7B8mCdT9KQhgG95s';
    final response = await http.get(
      Uri.parse(baseUrl2),
    );
    if (response.statusCode == 200) {
      final jsonData = json.decode(response.body);
      final result = AddressResult();
      result.formatted_address = jsonData['results'][0]['formatted_address'];
      result.name_address = jsonData['results'][0]["address_components"][0]['long_name'];
      print(result);
      return result;
    } else {
      throw Exception('Failed to fetch places');
    }
  }

  Future<AddressResult> fetchPlacesFromCurrentUserLocation() async {
    try {
      final position = await Geolocator.getCurrentPosition(
        desiredAccuracy: LocationAccuracy.high,
      );
      return await fetchPlacesFromLocation(position.latitude, position.longitude);
    } catch (e) {
      throw Exception('Error getting location: $e');
    }
  }
}
class AddressResult{
  String? formatted_address;
  String? name_address;
}