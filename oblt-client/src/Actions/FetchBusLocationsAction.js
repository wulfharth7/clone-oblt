export const fetchBusLocations = async (query = null) => {
    try {
      const sessionId = sessionStorage.getItem('session-id');
      const deviceId = sessionStorage.getItem('device-id');

      if (!sessionId || !deviceId) {
        console.error('Session or Device ID is missing');
        return [];
      }

      const requestBody = {
        data: query,
        'device-session': {
          'session-id': sessionId,
          'device-id': deviceId,
        },
        date: new Date().toISOString(),
        language: 'tr-TR',
      };

      const response = await fetch(
        'https://localhost:7046/api/buslocation/getbuslocations',
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(requestBody),
          credentials: 'include',
        }
      );

      if (!response.ok) {
        throw new Error(`Failed to fetch bus locations: ${response.statusText}`);
      }

      const data = await response.json();
      if (data && data.data) {
        return data.data;
      }
      return [];
    } catch (error) {
      console.error('Error fetching bus locations:', error);
      return [];
    }
  };