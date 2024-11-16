import React, { useState, useEffect } from 'react';
import { TextField, Button, Box } from '@mui/material';

//TO DO
//Refactor the code, its a mess, but for the start its ok.

const SearchModule = () => {
  const [input1, setInput1] = useState('');
  const [input2, setInput2] = useState('');
  const [sessionCreated, setSessionCreated] = useState(false);

  const createSession = async () => {
    try {
      const response = await fetch('https://localhost:7046/api/Session/create', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        credentials: 'include'
      });

      if (!response.ok) {
        throw new Error(`Failed to create session: ${response.statusText}`);
      }

      const data = await response.json();
      if (data.status === 'Success' && data.data) {
        console.log('Session Created:', data);

        sessionStorage.setItem('session-id', data.data['session-id']);
        sessionStorage.setItem('device-id', data.data['device-id']);
        setSessionCreated(true);
      } else {
        console.error('Failed to create session:', data.message);
      }
    } catch (error) {
      console.error('Error creating session:', error);
    }
  };

  const fetchBusLocations = async (input) => {
    try {
      const sessionId = sessionStorage.getItem('session-id');
      const deviceId = sessionStorage.getItem('device-id');

      if (!sessionId || !deviceId) {
        console.error('Session or Device ID is missing');
        return;
      }

      const requestBody = {
        data: input,
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
        credentials: 'include' 
      });

      if (!response.ok) {
        throw new Error(`Failed to fetch bus locations: ${response.statusText}`);
      }

      const data = await response.json();
      console.log('Bus Locations Response:', data);
    } catch (error) {
      console.error('Error fetching bus locations:', error);
    }
  };

  useEffect(() => {
    const sessionId = sessionStorage.getItem('session-id');
    const deviceId = sessionStorage.getItem('device-id');

    if (!sessionId || !deviceId) {
      createSession();
    } else {
      setSessionCreated(true);
    }
  }, []);

  return (
    <Box
      display="flex"
      justifyContent="center"
      alignItems="center"
      flexDirection="column"
      padding={2}
    >
      <h2>Search Module</h2>
      <Box display="flex" gap={2} alignItems="center">
        <TextField
          label="Search Box 1"
          variant="outlined"
          value={input1}
          onChange={(e) => {
            setInput1(e.target.value);
            if (e.target.value && sessionCreated) {
              fetchBusLocations(e.target.value);
            }
          }}
          fullWidth
        />
        <Button variant="contained" color="primary">
          ↔️
        </Button>
        <TextField
          label="Search Box 2"
          variant="outlined"
          value={input2}
          onChange={(e) => {
            setInput2(e.target.value);
            if (e.target.value && sessionCreated) {
              fetchBusLocations(e.target.value);
            }
          }}
          fullWidth
        />
      </Box>
    </Box>
  );
};

export default SearchModule;
