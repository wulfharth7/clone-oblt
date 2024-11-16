import React, { useState, useEffect, useCallback } from 'react';
import { TextField, Button, Box, Typography, Autocomplete, IconButton } from '@mui/material';
import { SwapHoriz, LocationOn } from '@mui/icons-material';

const SearchModule = () => {
  const [input1, setInput1] = useState('');
  const [input2, setInput2] = useState('');
  const [suggestions, setSuggestions] = useState([]);
  const [loading, setLoading] = useState(true);

  const createSession = async () => {
    try {
      const response = await fetch('https://localhost:7046/api/Session/create', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        credentials: 'include',
      });

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

      const response = await fetch('https://localhost:7046/api/buslocation/getbuslocations', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(requestBody),
        credentials: 'include',
      });

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
      const data = await fetchBusLocations(null); // Use null for initial request
      if (data.length > 0) {
        const limitedSuggestions = data.slice(0, 10); // Limit initial suggestions to 10
        setSuggestions(limitedSuggestions);
        setInput1(limitedSuggestions[0]?.name || ''); // Prepopulate input1
        setInput2(limitedSuggestions[0]?.name || ''); // Prepopulate input2
      }
      setLoading(false);
    } catch (error) {
      console.error('Error loading initial data:', error);
      setLoading(false);
    }
  };

  const handleInputChange = async (event, newValue, setInput, setSuggestions) => {
    setInput(newValue || '');
    if (newValue) {
      try {
        const data = await fetchBusLocations(newValue);
        setSuggestions(data.slice(0, 10)); // Limit dynamic suggestions to 10
      } catch (error) {
        console.error('Error fetching suggestions:', error);
      }
    } else {
      setSuggestions([]);
    }
  };

  const handleValueChange = (event, newValue, setInput) => {
    setInput(newValue || '');
  };

  const handleSwap = () => {
    setInput1(input2);
    setInput2(input1);
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
      <Typography variant="h4" gutterBottom fontWeight="bold" color="primary.main">
        Bus Location Search
      </Typography>

      <Box display="flex" gap={3} alignItems="center" width="100%">
        <Box width="100%">
          <Box display="flex" alignItems="center" gap={1}>
            <LocationOn color="primary" />
            <Typography variant="subtitle1" color="red" fontWeight="bold">From</Typography>
          </Box>
          <Autocomplete
            value={input1}
            onInputChange={(event, newValue) =>
              handleInputChange(event, newValue, setInput1, setSuggestions)
            }
            onChange={(event, newValue) => handleValueChange(event, newValue, setInput1)}
            options={suggestions.map((suggestion) => suggestion.name)}
            renderInput={(params) => <TextField {...params} variant="outlined" fullWidth />}
            freeSolo
          />
        </Box>

        <IconButton color="primary" onClick={handleSwap}>
          <SwapHoriz />
        </IconButton>

        <Box width="100%">
          <Box display="flex" alignItems="center" gap={1}>
            <LocationOn color="primary" />
            <Typography variant="subtitle1" color="red" fontWeight="bold">To</Typography>
          </Box>
          <Autocomplete
            value={input2}
            onInputChange={(event, newValue) =>
              handleInputChange(event, newValue, setInput2, setSuggestions)
            }
            onChange={(event, newValue) => handleValueChange(event, newValue, setInput2)}
            options={suggestions.map((suggestion) => suggestion.name)}
            renderInput={(params) => <TextField {...params} variant="outlined" fullWidth />}
            freeSolo
          />
        </Box>
      </Box>

      <Box display="flex" justifyContent="flex-end" marginTop={2}>
        <Button variant="contained" color="primary" size="large">
          Search
        </Button>
      </Box>
    </Box>
  );
};

export default SearchModule;
