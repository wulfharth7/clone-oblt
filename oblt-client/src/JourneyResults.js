import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  CircularProgress,
  List,
  ListItem,
  ListItemText,
} from '@mui/material';
import { useLocation } from 'react-router-dom';

const JourneyResults = () => {
  const location = useLocation();
  const { origin, destination, departureDate } = location.state || {};

  const [loading, setLoading] = useState(true);
  const [journeys, setJourneys] = useState([]);

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

  const fetchJourneys = async () => {
    try {
      const sessionId = sessionStorage.getItem('session-id');
      const deviceId = sessionStorage.getItem('device-id');

      if (!sessionId || !deviceId) {
        console.error('Session or Device ID is missing');
        return [];
      }

      const requestBody = {
        deviceSession: {
          sessionId: sessionId,
          deviceId: deviceId,
        },
        date: new Date().toISOString(),
        language: 'tr-TR',
        data: {
          originId: origin.id,
          destinationId: destination.id,
          departureDate: departureDate,
        },
      };

      const response = await fetch(
        'https://localhost:7046/api/Journeys/getjourneys',
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
        throw new Error(`Failed to fetch journeys: ${response.statusText}`);
      }

      const data = await response.json();
      if (data && data.data) {
        return data.data;
      }
      return [];
    } catch (error) {
      console.error('Error fetching journeys:', error);
      return [];
    }
  };

  useEffect(() => {
    const initialize = async () => {
      const sessionId = sessionStorage.getItem('session-id');
      const deviceId = sessionStorage.getItem('device-id');

      if (!sessionId || !deviceId) {
        const sessionCreated = await createSession();
        if (!sessionCreated) {
          setLoading(false);
          return;
        }
      }

      const journeysData = await fetchJourneys();
      setJourneys(journeysData);
      setLoading(false);
    };

    initialize();
  }, []);

  if (loading) {
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        height="100vh"
      >
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box padding={4}>
      <Typography variant="h4" gutterBottom>
        Journey Results
      </Typography>
      {journeys.length === 0 ? (
        <Typography>No journeys found.</Typography>
      ) : (
        <List>
          {journeys.map((journeyItem, index) => {
            const {
              partnerName,
              busType,
              totalSeats,
              availableSeats,
              journey,
            } = journeyItem;
            const { stops } = journey;

            // Find origin and destination stops
            const originStop = stops.find((stop) => stop.isOrigin);
            const destinationStop = stops.find((stop) => stop.isDestination);

            return (
              <ListItem key={index}>
                <ListItemText
                  primary={`${originStop?.name} to ${destinationStop?.name}`}
                  secondary={
                    <>
                      <Typography component="span" variant="body2">
                        Departure: {new Date(originStop?.time).toLocaleString()}
                      </Typography>
                      <br />
                      <Typography component="span" variant="body2">
                        Arrival: {new Date(destinationStop?.time).toLocaleString()}
                      </Typography>
                      <br />
                      <Typography component="span" variant="body2">
                        Bus Type: {busType}
                      </Typography>
                      <br />
                      <Typography component="span" variant="body2">
                        Operator: {partnerName}
                      </Typography>
                      <br />
                      <Typography component="span" variant="body2">
                        Seats Available: {availableSeats} / {totalSeats}
                      </Typography>
                    </>
                  }
                />
              </ListItem>
            );
          })}
        </List>
      )}
    </Box>
  );
};

export default JourneyResults;
