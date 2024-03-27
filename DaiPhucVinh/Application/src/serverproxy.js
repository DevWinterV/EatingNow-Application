const express = require('express');
const { createProxyMiddleware } = require('http-proxy-middleware');
const cors = require('cors');
const axios = require('axios');
const app = express();
const port = 3002; // The port for your proxy server

// Enable CORS to allow requests from your React app (http://localhost:3001)
// Define a CORS configuration
const corsOptions = {
  origin: ['http://localhost:3001', 'http://localhost:3000'], // Danh sách các origin mà bạn muốn cho phép
  credentials: true,
};

// Enable CORS with the specified options
app.use(cors(corsOptions));

// Define a proxy for SignalR requests
const signalRProxy = createProxyMiddleware({
  target: 'http://localhost:3000', // The URL of your SignalR server
  changeOrigin: true,
  ws: true, // Support WebSocket
  pathRewrite: {
    // '^/signalr/hubs': '/signalr/hubs', // Map the request path
  },
});

// Use the SignalR proxy
app.use('/signalr/hubs', signalRProxy);

// Define a route for calculateDistanceAndTime
app.get('/calculateDistanceAndTime', async (req, res) => {
  const { origin, destination } = req.query;


  if (!origin || !destination) {
    return res.status(400).json({ error: 'Missing origin or destination' });
  }

  // const apiKey = 'AIzaSyAG61NrUZkmMW8AS9F7B8mCdT9KQhgG95s';
  // const apiKey = 'AIzaSyDEPicO6JK3TSlMl3AQajyKyQqwLO0FWUw';
  const apiKey = 'AIzaSyDeFN4A3eenCTIUYvCI7dViF-N-V5X8RgA';
  const apiUrl = `https://maps.googleapis.com/maps/api/directions/json?origin=${encodeURIComponent(origin)}&destination=${encodeURIComponent(destination)}&key=${apiKey}`;
  try {
    const response = await axios.get(apiUrl);
    if (response.data.status === 'OK' && response.data.routes.length > 0) {
      const route = response.data.routes[0];
      const leg = route.legs[0];
      const distance = leg.distance.value;
      const duration = leg.duration.value;
      res.json({ distance, duration });
    } else {
      res.status(500).json({ error: 'Failed to retrieve data from Google Directions API' });
    }
  } catch (error) {
    console.error('Error calculating distance and time:', error);
    res.status(500).json({ error: 'An error occurred while calculating distance and time' });
  }
});

// Define a route for search-address
app.get('/search-address', async (req, res) => {
  const searchAddress = req.query.searchAddress;
  
  if (!searchAddress) {
    return res.status(400).json({ error: 'Invalid search address' });
  }
  
 // const apiKey = 'AIzaSyAG61NrUZkmMW8AS9F7B8mCdT9KQhgG95s';
  const apiKey = 'AIzaSyDEPicO6JK3TSlMl3AQajyKyQqwLO0FWUw';
  const apiUrl = `https://maps.googleapis.com/maps/api/place/textsearch/json?query=${encodeURIComponent(searchAddress)}&region=vn&key=${apiKey}`;

  try {
    const response = await axios.get(apiUrl);
    if (response.data.status === 'OK' && response.data.results.length > 0) {
      return res.json(response.data);
    } else {
      return res.status(404).json({ error: 'Address not found' });
    }
  } catch (error) {
    return res.status(500).json({ error: 'An error occurred while searching for the address' });
  }
});

app.listen(port, () => {
  console.log(`Proxy server is running on port ${port}`);
});
