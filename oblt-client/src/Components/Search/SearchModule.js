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
import { handleSwap } from '../../Actions/SwapAction';
import { handleSearch } from '../../Actions/SearchAction';
import { handleInputChange } from '../../Actions/InputChangeAction';
import { loadInitialData } from '../../Actions/LastQueryAction';
import { createSession } from '../../Actions/CreateSessionAction';
import { fetchBusLocations } from '../../Actions/FetchBusLocationsAction';

//This is the main Index Page where we choose our destination and origin cities to go.
//You can choose the date here, type in cities or choose from the list that comes in default from the external api.
//If its your first time using the app, it will be initialized beforehand with default settings.

const SearchModule = () => {
  const navigate = useNavigate();

  const todayDate = new Date();
  const today = todayDate.toISOString().split('T')[0];

  const tomorrowDate = new Date();
  tomorrowDate.setDate(tomorrowDate.getDate() + 1);
  const tomorrow = tomorrowDate.toISOString().split('T')[0];

  const [departureDate, setDepartureDate] = useState(tomorrow);

  const [value1, setValue1] = useState(null);
  const [inputValue1, setInputValue1] = useState('');
  const [suggestions1, setSuggestions1] = useState([]);

  const [value2, setValue2] = useState(null);
  const [inputValue2, setInputValue2] = useState('');
  const [suggestions2, setSuggestions2] = useState([]);

  const [loading, setLoading] = useState(true);

  

  const handleValueChange = (event, newValue, setValue, otherValue) => {
    if (newValue && newValue.name === otherValue?.name) {
      alert('The selected locations cannot be the same.');
      return; // Ignore the change
    }
    setValue(newValue); // Update the selected value
  };

  const onSwap = () => {
    handleSwap(
      value1, setValue1,
      inputValue1, setInputValue1,
      suggestions1, setSuggestions1,
      value2, setValue2,
      inputValue2, setInputValue2,
      suggestions2, setSuggestions2
    );
  };

  const onSearch = () => {
    handleSearch(value1, value2, departureDate, navigate);
  };
  

  useEffect(() => {
    const initialize = async () => {
      const sessionId = sessionStorage.getItem('session-id');
      const deviceId = sessionStorage.getItem('device-id');

      if (!sessionId || !deviceId) {
        const sessionCreated = await createSession();
        if (sessionCreated) {
          await loadInitialData(
            fetchBusLocations,
            setDepartureDate,
            setValue1,
            setInputValue1,
            setSuggestions1,
            setValue2,
            setInputValue2,
            setSuggestions2,
            tomorrow
          );
        }
      } else {
        await loadInitialData(
          fetchBusLocations,
          setDepartureDate,
          setValue1,
          setInputValue1,
          setSuggestions1,
          setValue2,
          setInputValue2,
          setSuggestions2,
          tomorrow
        );
      }
      setLoading(false);
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

      {/* Main Content */}
      <Box
        display="flex"
        flexDirection={{ xs: 'column', sm: 'row' }}
        gap={{ xs: 0.1, sm: 1.7 }} 
        alignItems={{ xs: 'flex-start', sm: 'center' }}
        width="100%"
      >
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
                reason,
                fetchBusLocations
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

        {/* Swap Button */}
        <Box
          display="flex"
          justifyContent="center"
          alignItems="center"
          sx={{
            marginTop: { xs: 0, sm: '10px' },
          }}
        >
          <IconButton
            color="primary"
            onClick={onSwap}
            sx={{
              marginTop: { xs: 0, sm: '5px' },
            }}
          >
            <SwapHoriz />
          </IconButton>
        </Box>

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
                reason,
                fetchBusLocations
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

      {/* Departure Date */}
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
            min: today,
          }}
        />
        {/* Buttons for Today and Tomorrow */}
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

      {/* Search Button */}
      <Box
        display="flex"
        justifyContent="center"
        width="100%"
        sx={{
          marginTop: { xs: '30px', sm: '20px' },
        }}
      >
        <Button
          variant="contained"
          color="primary"
          size="large"
          onClick={onSearch}
        >
          Search
        </Button>
      </Box>
    </Box>
  );
};

export default SearchModule;