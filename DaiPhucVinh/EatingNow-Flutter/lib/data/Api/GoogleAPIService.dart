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
      print(jsonData);
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

  Future<DistanceAndTime> calculateDistanceAndTime(String origin, String destination) async {
    final apiUrl = 'https://maps.googleapis.com/maps/api/directions/json?origin=${origin}&destination=${destination}&key=${apiKey}'; // Thay YOUR_API_KEY bằng key của bạn
    final response = await http.get(Uri.parse(apiUrl));

    if (response.statusCode == 200) {
      final jsonData = json.decode(response.body);
      final result = DistanceAndTime();
    print(response.body);
      // Kiểm tra xem có routes không và có legs không
      if (jsonData['routes'] != null && jsonData['routes'].length > 0 && jsonData['routes'][0]['legs'] != null && jsonData['routes'][0]['legs'].length > 0) {
        result.time = jsonData['routes'][0]['legs'][0]['duration']['text'];
        result.distance = jsonData['routes'][0]['legs'][0]['distance']['text'];

        return result;
      } else {
        throw Exception('Không tìm thấy thông tin khoảng cách và thời gian.');
      }
    } else {
      throw Exception('Failed to fetch data');
    }
  }
}

class AddressResult{
  String? formatted_address;
  String? name_address;
}

  class DistanceAndTime{
  String? distance;
  String? time;
  }