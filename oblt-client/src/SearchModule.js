import React, { useState, useEffect } from 'react';
import {
  TextField,
  Button,
  Box,
  Typography,
  Autocomplete,
  IconButton,
} from '@mui/material';
import { SwapHoriz, LocationOn } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';

const SearchModule = () => {
  const navigate = useNavigate();

  //TODO noktayı sil
  // phone kımsında media query yaz giriş için
  //tarih kontrolü koy localdb'ye geçmişte kalmasın.


  // Calculate today's and tomorrow's dates
  const todayDate = new Date();
  const today = todayDate.toISOString().split('T')[0];

  const tomorrowDate = new Date();
  tomorrowDate.setDate(tomorrowDate.getDate() + 1);
  const tomorrow = tomorrowDate.toISOString().split('T')[0];

  // Initialize state
  const [departureDate, setDepartureDate] = useState(tomorrow);

  const [value1, setValue1] = useState(null);
  const [inputValue1, setInputValue1] = useState('');
  const [suggestions1, setSuggestions1] = useState([]);

  const [value2, setValue2] = useState(null);
  const [inputValue2, setInputValue2] = useState('');
  const [suggestions2, setSuggestions2] = useState([]);

  const [loading, setLoading] = useState(true);

  const createSession = async () => {
    try {
      // Fetch client IP using a third-party service or a custom API
    const ipResponse = await fetch('https://api64.ipify.org?format=json');
    const { ip } = await ipResponse.json();

    // Detect browser name and version
    const userAgent = navigator.userAgent;
    let browserName = 'Unknown';
    let browserVersion = 'Unknown';

    if (userAgent.includes('Chrome')) {
      browserName = 'Chrome';
      browserVersion = userAgent.match(/Chrome\/([\d.]+)/)[1];
    } else if (userAgent.includes('Firefox')) {
      browserName = 'Firefox';
      browserVersion = userAgent.match(/Firefox\/([\d.]+)/)[1];
    } else if (userAgent.includes('Safari') && !userAgent.includes('Chrome')) {
      browserName = 'Safari';
      browserVersion = userAgent.match(/Version\/([\d.]+)/)[1];
    } else if (userAgent.includes('Edge')) {
      browserName = 'Edge';
      browserVersion = userAgent.match(/Edg\/([\d.]+)/)[1];
    } else if (userAgent.includes('Trident') || userAgent.includes('MSIE')) {
      browserName = 'Internet Explorer';
      browserVersion = userAgent.match(/(?:MSIE |rv:)([\d.]+)/)[1];
    }

    // Prepare the request body
    const requestBody = {
      connection: {
        "ip-address": ip,
      },
      browser: {
        name: browserName,
        version: browserVersion,
      },
    };

    // Send the data to the API
    const response = await fetch('https://localhost:7046/api/Session/create', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      credentials: 'include',
      body: JSON.stringify(requestBody),
    });
      console.log(JSON.stringify(requestBody))
      if (!response.ok) {
        throw new Error(`Failed to create session: ${response.statusText}`);
      }

      const data = await response.json();
      if (data.status === 'Success' && data.data) {
        sessionStorage.setItem('session-id', data.data['session-id']);
        sessionStorage.setItem('device-id', data.data['device-id']);
        return true;
      } else {
        console.error('Failed to create session:', data.message);
        return false;
      }
    } catch (error) {
      console.error('Error creating session:', error);
      return false;
    }
  };

  const fetchBusLocations = async (query = null) => {
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

  const loadInitialData = async () => {
    try {
      const data = await fetchBusLocations(null);
      if (data.length > 0) {
     
        setSuggestions1(data);
        setSuggestions2(data);

        const savedOrigin = localStorage.getItem('lastOrigin');
        const savedDestination = localStorage.getItem('lastDestination');
        const savedDepartureDate = localStorage.getItem('lastDepartureDate');

        let parsedOrigin = null;
        if (savedOrigin) {
          parsedOrigin = JSON.parse(savedOrigin);
        }

        let parsedDestination = null;
        if (savedDestination) {
          parsedDestination = JSON.parse(savedDestination);
        }

        const origin = data.find((loc) => loc.id === parsedOrigin?.id);
        const destination = data.find((loc) => loc.id === parsedDestination?.id);

        if (origin) {
          setValue1(origin);
          setInputValue1(origin.name);
        } else {
          setValue1(data[0]);
          setInputValue1(data[0]?.name || '');
        }

        if (destination) {
          setValue2(destination);
          setInputValue2(destination.name);
        } else if (data.length > 1) {
          setValue2(data[1]); 
          setInputValue2(data[1]?.name || '');
        } else {
          setValue2(data[0]);
          setInputValue2(data[0]?.name || '');
        }

        if (savedDepartureDate) {
          setDepartureDate(savedDepartureDate);
        }
      }
      setLoading(false);
    } catch (error) {
      console.error('Error loading initial data:', error);
      setLoading(false);
    }
  };

  const handleInputChange = async (
    event,
    newInputValue,
    setInputValue,
    setSuggestions,
    reason
  ) => {
    setInputValue(newInputValue || ''); // Update the input value
    if (reason === 'clear' || newInputValue === '') {
      setSuggestions([]); // Clear suggestions if input is cleared
      return;
    }
    if (reason === 'input') {
      try {
        const data = await fetchBusLocations(newInputValue);
        setSuggestions(data.slice(0, 10));
      } catch (error) {
        console.error('Error fetching suggestions:', error);
      }
    }
  };

  const handleValueChange = (event, newValue, setValue, otherValue) => {
    if (newValue && newValue.name === otherValue?.name) {
      alert('The selected locations cannot be the same.');
      return; // Ignore the change
    }
    setValue(newValue); // Update the selected value
  };

  const handleSwap = () => {
    const tempValue = value1;
    const tempInputValue = inputValue1;
    const tempSuggestions = suggestions1;

    setValue1(value2);
    setInputValue1(inputValue2);
    setSuggestions1(suggestions2);

    setValue2(tempValue);
    setInputValue2(tempInputValue);
    setSuggestions2(tempSuggestions);
  };

  const handleSearch = () => {
    if (!value1 || !value2) {
      alert('Please select both origin and destination locations.');
      return;
    }

    // Save the current values to localStorage
    localStorage.setItem('lastOrigin', JSON.stringify(value1));
    localStorage.setItem('lastDestination', JSON.stringify(value2));
    localStorage.setItem('lastDepartureDate', departureDate);

    navigate('/journey-results', {
      state: {
        origin: value1,
        destination: value2,
        departureDate: departureDate,
      },
    });
  };

  useEffect(() => {
    const initialize = async () => {
      const sessionId = sessionStorage.getItem('session-id');
      const deviceId = sessionStorage.getItem('device-id');

      if (!sessionId || !deviceId) {
        const sessionCreated = await createSession();
        if (sessionCreated) {
          await loadInitialData();
        }
      } else {
        await loadInitialData();
      }
    };

    initialize();
  }, []);

  if (loading) {
    return <Typography>Loading...</Typography>;
  }

  return (
    <Box
      display="flex"
      justifyContent="center"
      alignItems="center"
      flexDirection="column"
      padding={4}
      bgcolor="#f9f9f9"
      borderRadius={2}
      boxShadow={3}
      maxWidth="600px"
      margin="auto"
    >
      <Typography
        variant="h4"
        gutterBottom
        fontWeight="bold"
        color="primary.main"
      >
        Bus Location Search
      </Typography>

      <Box display="flex" gap={3} alignItems="center" width="100%">
        <Box width="100%">
          <Box display="flex" alignItems="center" gap={1}>
            <LocationOn color="primary" />
            <Typography variant="subtitle1" color="red" fontWeight="bold">
              From
            </Typography>
          </Box>
          <Autocomplete
            value={value1}
            inputValue={inputValue1}
            onInputChange={(event, newInputValue, reason) =>
              handleInputChange(
                event,
                newInputValue,
                setInputValue1,
                setSuggestions1,
                reason
              )
            }
            onChange={(event, newValue) =>
              handleValueChange(event, newValue, setValue1, value2)
            }
            options={suggestions1}
            getOptionLabel={(option) => option.name || ''}
            renderInput={(params) => (
              <TextField {...params} variant="outlined" fullWidth />
            )}
            freeSolo
          />
        </Box>

        <IconButton color="primary" onClick={handleSwap}>
          <SwapHoriz />
        </IconButton>

        <Box width="100%">
          <Box display="flex" alignItems="center" gap={1}>
            <LocationOn color="primary" />
            <Typography variant="subtitle1" color="red" fontWeight="bold">
              To
            </Typography>
          </Box>
          <Autocomplete
            value={value2}
            inputValue={inputValue2}
            onInputChange={(event, newInputValue, reason) =>
              handleInputChange(
                event,
                newInputValue,
                setInputValue2,
                setSuggestions2,
                reason
              )
            }
            onChange={(event, newValue) =>
              handleValueChange(event, newValue, setValue2, value1)
            }
            options={suggestions2}
            getOptionLabel={(option) => option.name || ''}
            renderInput={(params) => (
              <TextField {...params} variant="outlined" fullWidth />
            )}
            freeSolo
          />
        </Box>
      </Box>

      <Box width="100%" marginTop={2}>
        <TextField
          label="Departure Date"
          type="date"
          fullWidth
          variant="outlined"
          value={departureDate}
          onChange={(e) => setDepartureDate(e.target.value)}
          InputLabelProps={{
            shrink: true,
          }}
          inputProps={{
            min: today, // Set minimum date to today
          }}
        />
        {/* Buttons for today and tomorrow */}
        <Box display="flex" justifyContent="flex-start" marginTop={1} gap={1}>
          <Button
            variant={departureDate === today ? 'contained' : 'outlined'}
            onClick={() => setDepartureDate(today)}
            size="small"
          >
            Today
          </Button>
          <Button
            variant={departureDate === tomorrow ? 'contained' : 'outlined'}
            onClick={() => setDepartureDate(tomorrow)}
            size="small"
          >
            Tomorrow
          </Button>
        </Box>
      </Box>

      <Box display="flex" justifyContent="flex-end" marginTop={2}>
        <Button
          variant="contained"
          color="primary"
          size="large"
          onClick={handleSearch}
        >
          Search
        </Button>
      </Box>
    </Box>
  );
};

export default SearchModule;
