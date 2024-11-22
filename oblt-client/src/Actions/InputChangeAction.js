export const handleInputChange = async (
    event,
    newInputValue,
    setInputValue,
    setSuggestions,
    reason,
    fetchBusLocations
  ) => {
    setInputValue(newInputValue || ''); 
    if (reason === 'clear' || newInputValue === '') {
      setSuggestions([]); 
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
  