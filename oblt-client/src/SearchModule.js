// src/SearchModule.js
import React, { useState } from 'react';
import { TextField, Button, Box } from '@mui/material';

const SearchModule = () => {
  const [input1, setInput1] = useState('');
  const [input2, setInput2] = useState('');

  const handleSwap = () => {
    // Swap the values of input1 and input2
    setInput1(input2);
    setInput2(input1);
  };

  const handleSearch = () => {
    // Implement search logic here (e.g., make an API call)
    console.log('Searching with:', input1, input2);
  };

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
          onChange={(e) => setInput1(e.target.value)}
          fullWidth
        />
        <Button variant="contained" color="primary" onClick={handleSwap}>
          ↔️
        </Button>
        <TextField
          label="Search Box 2"
          variant="outlined"
          value={input2}
          onChange={(e) => setInput2(e.target.value)}
          fullWidth
        />
      </Box>
      <Button
        variant="contained"
        color="secondary"
        onClick={handleSearch}
        sx={{ marginTop: 2 }}
      >
        Search
      </Button>
    </Box>
  );
};

export default SearchModule;
