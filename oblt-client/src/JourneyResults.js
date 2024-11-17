import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  CircularProgress,
  Button,
  Card,
  CardContent,
  List,
  ListItem,
} from '@mui/material';
import { useLocation, useNavigate } from 'react-router-dom';
import { format } from 'date-fns';
import trLocale from 'date-fns/locale/tr';

const JourneyResults = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { origin, destination, departureDate } = location.state || {};

  const [loading, setLoading] = useState(true);
  const [journeys, setJourneys] = useState([]);

  // Function to create a new session
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

  // Function to fetch journeys
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

  // useEffect to initialize session and fetch journeys
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
  }, [origin.id, destination.id, departureDate]);

  const handleBack = () => {
    navigate('/');
  };

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

  // Format the departure date and day
  const formattedDate = format(new Date(departureDate), 'do MMMM, EEEE', {
    locale: trLocale,
  });

  return (
    <Box padding={4}>
      <Button variant="contained" color="primary" onClick={handleBack}>
        Back
      </Button>
      {/* Title at the top */}
      <Typography variant="h5" gutterBottom>
        {origin.name} - {destination.name}
      </Typography>
      {/* Date and day */}
      <Typography variant="subtitle1" gutterBottom>
        {formattedDate}
      </Typography>
      {journeys.length === 0 ? (
        <Typography>No journeys found.</Typography>
      ) : (
        <Box sx={{ maxHeight: '70vh', overflow: 'auto' }}>
          <List>
            {journeys.map((journeyItem, index) => {
              const {
                partnerName,
                partnerId,
                features,
                journey: {
                  stops,
                  origin: journeyOrigin,
                  destination: journeyDestination,
                },
              } = journeyItem;

              // Construct the logo URL
              const logoUrl = `https://s3.eu-central-1.amazonaws.com/static.obilet.com/images/partner/${partnerId}-sm.png`;

              // Find origin and destination stops
              const originStop = stops.find((stop) => stop.isOrigin);
              const destinationStop = stops.find(
                (stop) => stop.isDestination
              );

              const departureTime = originStop
                ? format(new Date(originStop.time), 'HH:mm')
                : 'N/A';
              const arrivalTime = destinationStop
                ? format(new Date(destinationStop.time), 'HH:mm')
                : 'N/A';

              // Construct feature icons
              const featureIcons = features.map((feature) => {
                const featureIconUrl = `https://s3.eu-central-1.amazonaws.com/static.obilet.com/images/feature/${feature.id}.svg`;
                return (
                  <img
                    key={feature.id}
                    src={featureIconUrl}
                    alt={feature.name}
                    title={feature.name}
                    style={{ height: '20px', margin: '0 4px' }}
                  />
                );
              });

              return (
                <ListItem key={index}>
                  <Card variant="outlined" sx={{ width: '100%' }}>
                    <CardContent>
                      {/* Bus Firm Logo */}
                      <Box
                        display="flex"
                        justifyContent="center"
                        alignItems="center"
                        mb={1}
                      >
                        <img
                          src={logoUrl}
                          alt={`${partnerName} logo`}
                          style={{ height: '50px' }}
                        />
                      </Box>
                      {/* Departure and Arrival Times */}
                      <Box
                        display="flex"
                        justifyContent="center"
                        alignItems="center"
                        mb={1}
                      >
                        <Typography variant="subtitle1" component="div">
                          {departureTime} → {arrivalTime}
                        </Typography>
                      </Box>
                      {/* Origin and Destination */}
                      <Box
                        display="flex"
                        justifyContent="center"
                        alignItems="center"
                        mb={1}
                      >
                        <Typography variant="body2" color="textSecondary">
                          {journeyOrigin} - {journeyDestination}
                        </Typography>
                      </Box>
                      {/* Operator Name */}
                      <Box
                        display="flex"
                        justifyContent="center"
                        alignItems="center"
                        mb={1}
                      >
                        <Typography variant="caption" color="textSecondary">
                          {partnerName}
                        </Typography>
                      </Box>
                      {/* Feature Icons */}
                      <Box
                        display="flex"
                        justifyContent="center"
                        alignItems="center"
                        mt={1}
                      >
                        {featureIcons}
                      </Box>
                    </CardContent>
                  </Card>
                </ListItem>
              );
            })}
          </List>
        </Box>
      )}
    </Box>
  );
};

export default JourneyResults;
