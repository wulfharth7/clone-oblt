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
  useMediaQuery,
  useTheme,
  IconButton,
} from '@mui/material';
import { ArrowBack } from '@mui/icons-material';
import { useLocation, useNavigate } from 'react-router-dom';
import { format } from 'date-fns';
import trLocale from 'date-fns/locale/tr';

const JourneyResults = () => {
  const theme = useTheme();
  const isDesktop = useMediaQuery(theme.breakpoints.up('sm'));
  const location = useLocation();
  const navigate = useNavigate();
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
        "device-session": {
          "session-id": sessionId,
          "device-id": deviceId,
        },
        "date": new Date().toISOString(),
        "language": 'tr-TR',
        "data": {
          "origin-id": origin.id,
          "destination-id": destination.id,
          "departure-date": departureDate,
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

  const formattedDate = format(new Date(departureDate), 'do MMMM EEEE', {
    locale: trLocale,
  });

  return (
    <Box padding={4} bgcolor="#f9f9f9" maxWidth="800px" margin="auto">
      {/* Header Section */}
      <Box
        display="flex"
        alignItems="center"
        justifyContent="space-between"
        flexDirection={isDesktop ? 'row' : 'column'}
        marginBottom={3}
      >
        <Button
          variant="contained"
          color="primary"
          startIcon={<ArrowBack />}
          onClick={handleBack}
          sx={{ alignSelf: isDesktop ? 'flex-start' : 'center', marginBottom: isDesktop ? 0 : 2 }}
        >
          Back
        </Button>
        <Box textAlign="center">
          <Typography variant="h5" fontWeight="bold">
            {origin.name} - {destination.name}
          </Typography>
          <Typography variant="subtitle1" color="textSecondary">
            {formattedDate}
          </Typography>
        </Box>
      </Box>

      {/* Journey List Section */}
      {journeys.length === 0 ? (
        <Typography>No journeys found.</Typography>
      ) : (
        <Box sx={{ maxHeight: '70vh', overflow: 'auto' }}>
          <List>
            {journeys.map((journeyItem, index) => {
              const {
                'partner-name': partnerName,
                'partner-id': partnerId,
                features,
                journey: {
                  stops,
                  origin: journeyOrigin,
                  destination: journeyDestination,
                },
              } = journeyItem;

              const logoUrl = `https://s3.eu-central-1.amazonaws.com/static.obilet.com/images/partner/${partnerId}-sm.png`;

              const originStop = stops.find((stop) => stop['is-origin']);
              const destinationStop = stops.find(
                (stop) => stop['is-destination']
              );

              const departureTime = originStop
                ? format(new Date(originStop.time), 'HH:mm')
                : 'N/A';
              const arrivalTime = destinationStop
                ? format(new Date(destinationStop.time), 'HH:mm')
                : 'N/A';

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
                  <Card
                    variant="outlined"
                    sx={{
                      width: '100%',
                      marginBottom: 2,
                      backgroundColor: 'white',
                    }}
                  >
                    <CardContent
                      sx={{
                        display: 'flex',
                        flexDirection: isDesktop ? 'row' : 'column',
                        alignItems: isDesktop ? 'center' : 'center',
                        justifyContent: isDesktop ? 'space-between' : 'center',
                      }}
                    >
                      <Box
                        sx={{
                          display: 'flex',
                          alignItems: 'center',
                          justifyContent: isDesktop ? 'flex-start' : 'center',
                          marginBottom: isDesktop ? 0 : 2,
                          width: isDesktop ? 'auto' : '100%',
                        }}
                      >
                        <img
                          src={logoUrl}
                          alt={`${partnerName} logo`}
                          style={{ height: '50px', marginRight: isDesktop ? '16px' : 0 }}
                        />
                      </Box>

                      <Box
                        sx={{
                          textAlign: 'center',
                          flex: 1,
                          marginBottom: isDesktop ? 0 : 2,
                        }}
                      >
                        <Typography variant="h6" fontWeight="bold">
                          {departureTime} → {arrivalTime}
                        </Typography>
                        <Typography variant="body2" color="textSecondary">
                          {journeyOrigin} - {journeyDestination}
                        </Typography>
                        <Box mt={1}>{featureIcons}</Box>
                      </Box>

                      <Typography variant="body2" fontWeight="bold">
                        {partnerName}
                      </Typography>
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
