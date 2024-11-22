import React, { useState, useEffect } from 'react';
import {Box, Typography, CircularProgress, Button, Card, CardContent, List, ListItem, useMediaQuery, useTheme, IconButton,
} from '@mui/material';
import { ArrowBack } from '@mui/icons-material';
import { useLocation, useNavigate } from 'react-router-dom';
import { format } from 'date-fns';
import trLocale from 'date-fns/locale/tr';
import { createSession } from '../../Actions/CreateSessionAction';
import { fetchJourneys } from '../../Actions/FetchJourneyAction';

const JourneyResults = () => {
  const theme = useTheme();
  const isDesktop = useMediaQuery(theme.breakpoints.up('sm'));
  const location = useLocation();
  const navigate = useNavigate();
  const { origin, destination, departureDate } = location.state || {};

  const [loading, setLoading] = useState(true);
  const [journeys, setJourneys] = useState([]);
  //Here we see the Journey Index page.
  //You can see the logos and feature icons of bus vendors. 
  //Departure and arrival time, the bus stop names and the partner names are shown here. You can scroll down and see more. They are listed in a sorted way. Sorting happens in the server side.
  //Its compatiable with mobile version. 
  
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
  
      const journeysData = await fetchJourneys(origin, destination, departureDate);
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

  const formattedDate = format(new Date(departureDate), 'd MMMM EEEE', {
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
                  const handleError = (e) => {
                    e.target.style.display = 'none'; 
                  };
                  return (
                    <img
                      key={feature.id}
                      src={featureIconUrl}
                      alt={feature.name}
                      title={feature.name}
                      style={{ height: '20px', margin: '0 4px' }}
                      onError={handleError}
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
