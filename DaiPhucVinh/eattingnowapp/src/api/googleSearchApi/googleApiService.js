import axios from "axios";

const apiKey = 'AIzaSyAG61NrUZkmMW8AS9F7B8mCdT9KQhgG95s'; // API key
const handleSearchAddress = async (searchAddress) => {
  if (!searchAddress) {
    return;
  }

  const apiUrl = `https://cors-anywhere.herokuapp.com/https://maps.googleapis.com/maps/api/place/textsearch/json?query=${encodeURIComponent(searchAddress)}&region=vn&key=${apiKey}`;
  try {
    const response = await axios.get(apiUrl);
    if (response.data.status === "OK" && response.data.results.length > 0) {
      // Process the data here
      console.log(response)
      const firstResult = response.data.results[0];
      const location = firstResult.geometry.location;
      const name = firstResult.name;
      console.log('Location:', location);
      console.log('Name:', name);
    } else {
      return response.data.status ==="DOK"
    }
    return response;
  } catch (error) {
    console.error('Lỗi khi tìm kiếm địa chỉ:', error);
    alert('Có lỗi xảy ra khi tìm kiếm địa chỉ. Vui lòng thử lại sau.');
  }
};


export {
    handleSearchAddress
}