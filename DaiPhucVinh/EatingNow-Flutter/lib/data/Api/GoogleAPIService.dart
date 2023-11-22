import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:geolocator/geolocator.dart';

class GoogleAPIService {
  final String apiKey;

  GoogleAPIService(this.apiKey);

  Future<String> fetchPlacesFromLocation(double latitude, double longitude) async {
    final query = '$latitude,$longitude';
    final baseUrl = 'https://maps.googleapis.com/maps/api/place/textsearch/json';
    final response = await http.get(
      Uri.parse('$baseUrl?query=${Uri.encodeComponent(query)}&region=vn&key=$apiKey'),
    );
    if (response.statusCode == 200) {
      final jsonData = json.decode(response.body);
      return jsonData['results'][0]['formatted_address'];
    } else {
      throw Exception('Failed to fetch places');
    }
  }

  Future<String> fetchPlacesFromCurrentUserLocation() async {
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
